using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Rocket_Android : MonoBehaviour
{
    [SerializeField] readonly bool debugMode = false;
    [SerializeField] float thrustPower = 950f;
    [SerializeField] float rotateSpeed = 175f;
    [SerializeField] AudioClip sfxThrust;
    [SerializeField] AudioClip sfxDeath;
    [SerializeField] AudioClip sfxWin;

    [SerializeField] ParticleSystem fxThrust;
    [SerializeField] ParticleSystem fxDeath;
    [SerializeField] ParticleSystem fxWin;
    static int sceneLevel = 0; //Level 1
    static int sceneMaxlevel;
    private float levelLoadDelay = 3f;

    enum State { Alive, Dying, Transcending };
    State playerState = State.Alive;

    Vector3 cameraOffset;

    [SerializeField] Joystick joystick;
    BtnThrust btnThrust;
    Rigidbody rigidBody;
    AudioSource audioThrustController;
    AudioSource audioController;

    // Start is called before the first frame update
    void Start()
    {
        sceneMaxlevel = SceneManager.sceneCountInBuildSettings - 1;

        rigidBody = GetComponent<Rigidbody>();
        audioController = GetComponents<AudioSource>()[0];
        audioThrustController = GetComponents<AudioSource>()[1];

        btnThrust = GameObject.Find("BtnThrust").GetComponent<BtnThrust>();

        cameraOffset = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        Camera.main.transform.position = transform.position + cameraOffset;

        if (playerState == State.Alive)
        {
            PlayerThrust();
            PlayerRotate();
        }

        if (debugMode)
        {
            if (Input.GetKey(KeyCode.L)) { Won(); }
        }
    }

    private void ReloadScene()
    {
        fxDeath.Stop();
        SceneManager.LoadScene(sceneLevel);
    }
    private void NextScene()
    {
        fxWin.Stop();
        if (sceneLevel < sceneMaxlevel) { sceneLevel++; }
        else { sceneLevel = 0; }
        SceneManager.LoadScene(sceneLevel);
    }

    private void Death()
    {
        playerState = State.Dying;
        audioThrustController.volume = 0;
        audioController.PlayOneShot(sfxDeath);
        Handheld.Vibrate();
        fxDeath.Play();
        Invoke("ReloadScene", levelLoadDelay);
    }
    private void Won()
    {
        if (playerState == State.Transcending) { return; }
        playerState = State.Transcending;
        audioThrustController.volume = 0;
        audioController.PlayOneShot(sfxWin);
        fxWin.Play();
        Invoke("NextScene", levelLoadDelay);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (playerState != State.Alive || debugMode) { return; } //ignore collisions when dead

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;

            case "Goal":
                Won();
                break;

            default:
                fxThrust.Stop();
                Death();
                break;
        }
    }

    private void PlayerThrust()
    {
        if (btnThrust.IsPressed())
        {
            Thrust();
            fxThrust.Play();
        }
        else
        {
            if (audioThrustController.volume != 0) { audioThrustController.volume = 0; }
            fxThrust.Stop();
        }
    }

    private void Thrust()
    {
        float frameThrustPower = thrustPower * Time.deltaTime;

        rigidBody.AddRelativeForce(Vector3.up * frameThrustPower);

        if (audioThrustController.volume != 1) { audioThrustController.volume = 1; }
        if (!audioThrustController.isPlaying){audioThrustController.Play();}
    }

    private void PlayerRotate()
    {
        if (joystick.Horizontal < 0f)
        {
            RotateDirection(rotateSpeed * -joystick.Horizontal * Time.deltaTime);
        }
        else if (joystick.Horizontal > 0f)
        {
            RotateDirection(-rotateSpeed * joystick.Horizontal * Time.deltaTime);
        }
    }

    private void RotateDirection(float frameRotateSpeed)
    {
        //stop natural phystics control and take control of rotation
        rigidBody.freezeRotation = true;
        transform.Rotate(Vector3.forward * frameRotateSpeed);
        //resume natural phystics control
        rigidBody.freezeRotation = false;
    }
}
