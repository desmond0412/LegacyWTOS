using UnityEngine;
using System.Collections;

public class Boat : MonoBehaviour {
    public Rigidbody affectedRB;

    public bool enableMovement;
    public Vector3 boatVelocity;

    void Start(){
        if(!affectedRB){
            affectedRB = GetComponent<Rigidbody>();
        }
    }

    void FixedUpdate(){
        if(enableMovement){
            affectedRB.AddForce(boatVelocity,ForceMode.Acceleration);
        }
    }
}
