using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movment : MonoBehaviour
{
    // Start is called before the first frame update

    public Rigidbody Rb;
    public float force = 1000f;
    public float max_speed = 11f;
    public float max_angular_speed = 2.5f;
    public float shoot_distance = 3f;
    public KeyCode forward = KeyCode.UpArrow;
    public KeyCode backward = KeyCode.DownArrow;
    public KeyCode right = KeyCode.RightArrow;
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode shoot = KeyCode.Space;


    private bool invert = false;
    private bool shot = false;
    private Vector3 initpos;
    private Vector3 initvelocity;

    // Update is called once per frame
    void FixedUpdate()  //fixed update is for physics
    {
        Rb.maxAngularVelocity = max_angular_speed;
        if (Input.GetKey(forward) && Rb.velocity.magnitude < max_speed) //move forward
        {
            invert = false;
            Rb.AddRelativeForce(0, 0, force * Time.deltaTime); //add force on z axis. time delta time makes up for fps diffrences
        }
        
        if (Input.GetKey(backward) && Rb.velocity.magnitude < max_speed)  //move backwards  
        {
            invert = true;
            Rb.AddRelativeForce(0, 0, -1 * force * Time.deltaTime); //add force on z axis. time delta time makes up for fps diffrences
        }

        if ((Input.GetKey(right) && invert == false) || (Input.GetKey(left) && invert == true)) //turn right
        {
            Rb.AddTorque(Vector3.up * force);
        }


        if ((Input.GetKey(left) && invert == false) || (Input.GetKey(right) && invert == true)) //turn left
        {
            Rb.AddTorque(Vector3.down * force);
        }


        if (transform.InverseTransformDirection(Rb.velocity).z >= 0)    //stop inverted turning when stopped
        {
            invert = false;
        }


        if (Input.GetKey(shoot) && shot == false)   //start shoot
        {
            shot = true;
            
            initvelocity = Rb.velocity;
            initpos = Rb.transform.position;
            Rb.AddRelativeForce(0, 0, force*20 * Time.deltaTime);
        }


        if (shot == true && Vector3.Distance(Rb.transform.position, initpos) >= shoot_distance) //stop shoot
        {
            shot = false;
            Rb.velocity = initvelocity;
        }


    }
}
