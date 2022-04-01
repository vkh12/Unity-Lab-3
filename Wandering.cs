using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wandering : MonoBehaviour
{
    public bool ReadyToExecute;
    public bool wasToRight;
    public bool done;
    // Start is called before the first frame update
    void Start()
    {
        done = false;
        ReadyToExecute = true;
        wasToRight = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!done)
        {
            Main();
        }
        
    }

    public void Main()
    {
        if(ReadyToExecute)
        {
            CheckSensors();
            Debug.Log("Checking Sensors...");
            ReadyToExecute = false;
        }
    }
    public IEnumerator MoveForward()
    {
        Debug.Log("Moving forward...");
        for (int i = 0; i < 180; i++)
        {
            this.transform.position += (this.transform.forward / 180);
            yield return null;
        }
        SetReady();
    }

    public IEnumerator TurnCCW()
    {
        Debug.Log("Turning...");
        for (int i = 0; i < 180; i++)
        {
            this.transform.Rotate(new Vector3(0f, -.5f, 0f));
            yield return null;
        }
        SetReady(); 
    }

    public IEnumerator TurnCW()
    {
        Debug.Log("Turning...");
        for (int i = 0; i < 180; i++)
        {
            this.transform.Rotate(new Vector3(0f, .5f, 0f));
            yield return null;
        }
        SetReady();
    }

    public void SetReady()
    {
        if (!ReadyToExecute)
            ReadyToExecute = true;
    }

    public void CheckSensors()
    {
        bool inFront = false;
        bool toRight = false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, this.transform.forward, out hit, 5f))
        {
            Debug.Log("There is something in front of me!");
            inFront = true;
        }
        if (Physics.Raycast(transform.position, this.transform.right, out hit, 5f))
        {
            Debug.Log("There is something to the right of me!");
            toRight = true;
        }
        if(inFront == false && toRight == false) //No walls around
        {
            if (wasToRight == false)
            {
                StartCoroutine(MoveForward());
                Debug.Log("No walls to left or right sensed.");
            }
            else
            {
                StartCoroutine(TurnCW());
                Debug.Log("No walls to left or right sensed. There was previously a wall to right.");
                wasToRight = false;
            }
        }
        else if(inFront == true) //Wall in front
        {
            StartCoroutine(TurnCCW());
            Debug.Log("Wall in front sensed.");
            wasToRight = false;
        }
        else if (inFront == false && toRight == true) //Wall to the right only
        {
            StartCoroutine(MoveForward());
            Debug.Log("Wall to the right only sensed.");
            wasToRight = true;
        }
    }

    public void turnOff()
    {
        done = true;
    }
}