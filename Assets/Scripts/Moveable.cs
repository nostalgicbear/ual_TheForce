using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Moveable : MonoBehaviour {

    public bool canBeControlled;
    public Rigidbody rBody;
    public LineRenderer lineRenderer;
	// Use this for initialization
	void Start () {
        rBody = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
