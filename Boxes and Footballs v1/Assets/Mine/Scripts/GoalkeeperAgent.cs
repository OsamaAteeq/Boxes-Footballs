using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class GoalkeeperAgent : Agent
{
    //MOVMENT
    private Rigidbody Rb;
    public float force = 2000f;
    public float max_speed = 11f;
    public float max_angular_speed = 2.5f;
    public float shoot_distance = 3f;

    //for testing 
    public KeyCode forward = KeyCode.UpArrow;
    public KeyCode backward = KeyCode.DownArrow;
    public KeyCode right = KeyCode.RightArrow;
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode shoot = KeyCode.Space;
    //for testing



    private bool invert = false;
    private bool shot = false;
    private Vector3 initpos;
    private Vector3 initvelocity;
    //MOVMENT


    [SerializeField] private Transform ballTransform;
    [SerializeField] private GameObject defendinggoal;

    private void Start()
    {
        Rb = GetComponent<Rigidbody>();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        bool up_key = (actions.DiscreteActions[0] == 1);
        bool down_key = (actions.DiscreteActions[0] == 2);

        bool left_key = (actions.DiscreteActions[1] == 1);
        bool right_key = (actions.DiscreteActions[1] == 2);

        bool shoot_key = (actions.DiscreteActions[2] == 1);

        Rb.maxAngularVelocity = max_angular_speed;
        if (up_key && Rb.velocity.magnitude < max_speed) //move forward
        {
            invert = false;
            Rb.AddRelativeForce(0, 0, force * Time.deltaTime); //add force on z axis. time delta time makes up for fps diffrences
        }

        if (down_key && Rb.velocity.magnitude < max_speed)  //move backwards  
        {
            invert = true;
            Rb.AddRelativeForce(0, 0, -1 * force * Time.deltaTime); //add force on z axis. time delta time makes up for fps diffrences
        }

        if ((right_key && invert == false) || (Input.GetKey(left) && invert == true)) //turn right
        {
            Rb.AddTorque(Vector3.up * force);
        }


        if ((left_key && invert == false) || (Input.GetKey(right) && invert == true)) //turn left
        {
            Rb.AddTorque(Vector3.down * force);
        }


        if (transform.InverseTransformDirection(Rb.velocity).z >= 0)    //stop inverted turning when stopped
        {
            invert = false;
        }


        if (shoot_key && shot == false)   //start shoot
        {
            shot = true;

            initvelocity = Rb.velocity;
            initpos = Rb.transform.position;
            Rb.AddRelativeForce(0, 0, force * 15 * Time.deltaTime);
        }


        if (shot == true && Vector3.Distance(Rb.transform.position, initpos) >= shoot_distance) //stop shoot
        {
            shot = false;
            Rb.velocity = initvelocity;
        }

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Vector3.Angle(Rb.transform.position, ballTransform.position));
        sensor.AddObservation(Vector3.Distance(Rb.transform.position, defendinggoal.transform.position));
    }
    /*
    public override void Heuristic(in ActionBuffers actionsOut) //for test purposes only
    {
        ActionSegment<int> actions = actionsOut.DiscreteActions;
        if (Input.GetKey(forward))
        {
            actions[0] = 1;
        }
        if (Input.GetKey(backward))
        {
            actions[0] = 2;
        }
        if (Input.GetKey(left))
        {
            actions[1] = 1;
        }
        if (Input.GetKey(right))
        {
            actions[1] = 2;
        }
        if (Input.GetKey(shoot))
        {
            actions[2] = 1;
        }

    }
    */
    public void Update()
    {
        if (defendinggoal.GetComponent<GoalScript>().scored == true)
        {
            AddReward(-1000);
            Debug.Log(GetCumulativeReward());
            EndEpisode();
        }
        float agent_from_goal = Vector3.Distance(defendinggoal.GetComponent<Transform>().position, Rb.position);
        float ball_from_goal = Vector3.Distance(defendinggoal.GetComponent<Transform>().position, ballTransform.position);
        if (agent_from_goal >= 1 && agent_from_goal <= 5)
        {
            AddReward(100);
        }
        else
        {
            AddReward(-0.5f*agent_from_goal);
            AddReward(0.01f * agent_from_goal);
        }
        if (ball_from_goal <= 5)
        {
            AddReward(-1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "ball")
        {
            AddReward(200);
            Debug.Log(GetCumulativeReward());
        }
        if (collision.collider.tag == "boundary")
        {
            AddReward(-5);
            Debug.Log(GetCumulativeReward());
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "ball" || collision.collider.tag == "boundary")
        {
            AddReward(-20);
            Debug.Log(GetCumulativeReward());
        }
    }
}
