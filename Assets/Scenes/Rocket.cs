using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 30f; 
    Rigidbody rigidBody;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>(); // generics and getcomponent(Rigidbody) from unity
        audioSource = GetComponent<AudioSource>();//get audio clip from unity 
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }


    void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                print("okay");
                break;
            case "Fuel":
                print("fuel");
                break;
            default:
                print("dead");
                break;
        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space)) // detecting if space key is hit (every frame) KeyCode is an enum . 
        {
            //CAN THRUST WHILE ROTATING
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);//position of the ship (x,y,z) is the parameter(Vector3)
            if (audioSource.isPlaying == false) // so that it doesn't layer
            {
                audioSource.Play();
            }

        }
        else
        {
            audioSource.Stop();
        }
    }


    private void Rotate()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation 

        
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))//else if used as rocket shouldn't rotate on bs
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; //resume physics control of rotation 
    }

   
}
