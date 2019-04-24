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
    [SerializeField] AudioClip mainEngine, deathClip, successClip;
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
        //rocketRigidBody.freezeRotation = true;
        rocketRigidBody.angularVelocity = Vector3.zero;//zero rotation due to physics engine
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
                engineParticle.Stop();
                break;

            case "Finish":
                state = State.TRANSCENDING;
                engineParticle.Stop();
                successParticle.Play();
                PlayAudio(successClip);
                Invoke("NextScene", 3f);
                break;

            case "Death":
                state = State.DYING;
                PlayAudio(deathClip);
                engineParticle.Stop();
                deathParticle.Play();                               
                Invoke("NextScene", 3f);


                break;
            default:
                state = State.DYING;
                break;
        }
    }





    public void PlayAudio(AudioClip a )
    {
        rocketAudio.Stop();
        rocketAudio.PlayOneShot(a);
        Debug.Log("Dead");
    }


    private void NextScene()
    {
        SceneManager.LoadScene(0);
    }










































}//Rocket
 