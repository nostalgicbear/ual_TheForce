using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.Events;
using System;

public class OurCustomControls : MonoBehaviour
{
    private List<Moveable> moveables;
    private List<Moveable> controllables = new List<Moveable>();

    public UnityEvent onTouchpadPressed;
    public SteamVR_Action_Boolean pushButton;
    public SteamVR_Action_Boolean pullButton;
    public Material inView, outOfView;
    public Transform leftHand;
    public Transform rightHand;

    private void Awake()
    {
        moveables = new List<Moveable>(FindObjectsOfType<Moveable>());
       
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        DrawRays();

        ChangeObjectMaterial();

        
        ListenForButtonPress();
    }

    /// <summary>
    /// Changes the cubes material basexd on whether or not they can be seen
    /// </summary>
    private void ChangeObjectMaterial()
    {

        foreach(Moveable moveable in moveables)
        {
            Vector3 targetDir = moveable.transform.position - transform.position;
            float angle = Vector3.Angle(targetDir, transform.forward);

            if (angle <= 45)
            {
                moveable.GetComponent<Renderer>().material = inView;
                AddMoveableToControllables(moveable);
            }
            else
            {
                moveable.GetComponent<Renderer>().material = outOfView;
                RemoveMoveableFromControllables(moveable);
            }
        }
       
    }

    void AddMoveableToControllables(Moveable m)
    {

        if(!controllables.Contains(m))
        {
            controllables.Add(m);
        }
    }

    void RemoveMoveableFromControllables(Moveable m)
    {
        if (controllables.Contains(m))
        {
            m.lineRenderer.enabled = false;
            controllables.Remove(m);
        }
    }

    /// <summary>
    /// Just draws the rays so we can visualize the arc of vision
    /// </summary>
    private void DrawRays()
    {
        float distance = 45f;
        Debug.DrawRay(transform.position, (transform.forward + transform.right) * distance, Color.green);
        Debug.DrawRay(transform.position, (transform.forward - transform.right) * distance, Color.green);
    }

    private void ListenForButtonPress()
    {
        if (pushButton.GetState(SteamVR_Input_Sources.LeftHand))
        {
            Debug.Log("Left: push");
            MoveObjects(leftHand, true);
        }

        if (pushButton.GetState(SteamVR_Input_Sources.RightHand))
        {
            Debug.Log("Right: push");
            MoveObjects(rightHand, true);
        }

        if (pullButton.GetState(SteamVR_Input_Sources.LeftHand))
        {
            Debug.Log("Left: pull");

            MoveObjects(leftHand, false);
        }

        if (pullButton.GetState(SteamVR_Input_Sources.RightHand))
        {
            Debug.Log("Right: pull");

            MoveObjects(rightHand, false);
        }

        if(pushButton.GetStateUp(SteamVR_Input_Sources.Any) || pullButton.GetStateUp(SteamVR_Input_Sources.Any))
        {
            foreach (Moveable m in controllables)
            {
                m.lineRenderer.enabled = false;
            }
        }



    }

    private void MoveObjects(Transform hand, bool push)
    {
        foreach(Moveable m in controllables)
        {
            Vector3 direction = Vector3.zero;
            if (push)
            {
                direction = hand.forward;
            }
            else {
                direction = hand.forward * -1;
            }

            if(!m.lineRenderer.enabled)
            {
                m.lineRenderer.enabled = true;
            }

            if (m.lineRenderer.enabled)
            {
                m.lineRenderer.SetPosition(0, direction * 10.0f);
                m.lineRenderer.SetPosition(1, m.transform.position);
            }
            m.rBody.AddForce(direction * 15.0f, ForceMode.Acceleration);
        }
    }
}
