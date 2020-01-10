using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float thrustPower = 25;
    [SerializeField] float rotateSpeed = 175f;
    [SerializeField] AudioClip sfxThrust;
    [SerializeField] AudioClip sfxDeath;
    [SerializeField] AudioClip sfxWin;
    static int sceneLevel = 0; //Level 1
    static int sceneMaxlevel = 1;

    enum State { Alive, Dying, Transcending};
    State playerState = State.Alive;

    Rigidbody rigidBody;
    AudioSource audioController;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioController = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerState == State.Alive)
        {
            PlayerThrust();
            PlayerRotate();
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(sceneLevel);
    }
    private void NextScene()
    {
        if (sceneLevel < sceneMaxlevel) { sceneLevel++; };
        SceneManager.LoadScene(sceneLevel);
    }

    private void Death()
    {
        playerState = State.Dying;
        audioController.Stop();
        audioController.PlayOneShot(sfxDeath);
        Invoke("ReloadScene", 3f);
    }
    private void Won()
    {
        playerState = State.Transcending;
        audioController.Stop();
        audioController.PlayOneShot(sfxWin);
        Invoke("NextScene", 3f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (playerState != State.Alive) { return; } //ignore collisions when dead

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;

            case "Goal":
                Won();
                break;

            default:
                Death();
                break;
        }
    }

    private void PlayerThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Thrust();
        }
        else
        {
            audioController.Stop();
        }
    }

    private void Thrust()
    {
        float frameThrustPower = thrustPower;

        rigidBody.AddRelativeForce(Vector3.up * frameThrustPower);
        if (!audioController.isPlaying)
        {
            audioController.PlayOneShot(sfxThrust);
        }
    }

    private void PlayerRotate()
    {
        //stop natural phystics control and take control of rotation
        rigidBody.freezeRotation = true;
        float frameRotateSpeed = rotateSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * frameRotateSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * frameRotateSpeed);
        }
        rigidBody.freezeRotation = false; //resume natural phystics control

    }

}
