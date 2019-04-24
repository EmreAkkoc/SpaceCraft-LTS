using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rocketRigidBody;
    AudioSource rocketAudio;
    [SerializeField] private float speed=20f;
    [SerializeField] private float rotationspeed = 120f;
    [SerializeField] AudioClip mainEngine, deathClip;
    [SerializeField] ParticleSystem engineParticle, successParticle, deathParticle;

    public enum State  { ALIVE, DYING, TRANSCENDING, }
    State state = State.ALIVE;



    void Start()
    {

        
        rocketRigidBody = GetComponent<Rigidbody>();
        rocketAudio = GetComponent<AudioSource>();
        
    }

    
    void Update()
    {


        if (state == State.ALIVE)
        {
            Thrust();
            Rotate();
        }

        

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
        

        rocketRigidBody.freezeRotation = false;
    }




    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rocketRigidBody.AddRelativeForce(Vector3.up * speed * Time.deltaTime, ForceMode.Impulse);
            engineParticle.Play();
            if (!rocketAudio.isPlaying)
                rocketAudio.PlayOneShot(mainEngine);
        }
        else
        {
            
            rocketAudio.Stop();
            engineParticle.Stop();
        }

    }


   


     void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                state = State.ALIVE;
                break;

            case "Finish":
                state = State.TRANSCENDING;
                successParticle.Play();
                Invoke("NextScene", 3f);
                break;

            case "Death":
                PlayDeathAudio();
                deathParticle.Play();
                engineParticle.Stop();
                state = State.DYING;
               

                break;
            default:
                state = State.DYING;
                break;
        }
    }





    public void PlayDeathAudio()
    {
        rocketAudio.Stop();
        rocketAudio.PlayOneShot(deathClip);
        Debug.Log("Dead");
    }


    private void NextScene()
    {
        SceneManager.LoadScene(0);
    }










































}//Rocket
 