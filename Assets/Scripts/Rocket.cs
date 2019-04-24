using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rocketRigidBody;
    AudioSource rocketAudio;
    [SerializeField] private float speed=100f;
    [SerializeField] private float rotationspeed = 250;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathClip;



    public enum State
    {
        ALIVE,
        DYING,
        TRANSCENDING,
    }
    State state = State.ALIVE;
    
    void Start()
    {

        
        rocketRigidBody = GetComponent<Rigidbody>();
        rocketAudio = GetComponent<AudioSource>();
        
    }

    
    void Update()
    {

        
        Thrust();
        Rotate();

    }

    

    private void Rotate()
    {
        rocketRigidBody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationspeed * Time.deltaTime);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * rotationspeed * Time.deltaTime);
        }
        if (!Input.GetKey(KeyCode.Space))
            rocketAudio.Stop();

        rocketRigidBody.freezeRotation = false;
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rocketRigidBody.AddRelativeForce(Vector3.up * speed * Time.deltaTime,ForceMode.Impulse);
            if (!rocketAudio.isPlaying)
                rocketAudio.PlayOneShot(mainEngine);
        }
    }
    public void PlayDeathAudio()
    {
        
        rocketAudio.PlayOneShot(deathClip);
        Debug.Log("playing");
    }
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                state = State.ALIVE;
                break;

            case "Fuel":
                break;

            case "Finish":
                PlayDeathAudio();
                state = State.TRANSCENDING;
                Invoke("NextScene", 3f);

                break;
            default:
                state = State.DYING;
                break;
        }
    }
    


    private void NextScene()
    {
        SceneManager.LoadScene(0);
    }
}
 