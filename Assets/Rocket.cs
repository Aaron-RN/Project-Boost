using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioController;
    [SerializeField] float thrustPower = 25;
    [SerializeField] float rotateSpeed = 175f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioController = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerThrust();
        PlayerRotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Ok");
                break;

            case "Goal":
                print("Winner");
                break;

            default:
                print("Dead");
                break;
        }
    }

    private void PlayerThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float frameThrustPower = thrustPower;

            rigidBody.AddRelativeForce(Vector3.up * frameThrustPower);
            if (!audioController.isPlaying)
            {
                audioController.Play();
            }
        }
        else
        {
            audioController.Stop();
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
