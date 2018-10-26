using UnityEngine;
using System.Collections;

public class WaterInteractableObject : MonoBehaviour {
    public Rigidbody affectedRB;
    public float forceCoefficient = 1.0f;
    public Vector3 velocityMag;

    Vector3 lastPos;

    void Awake(){
        lastPos = transform.position;
    }

    void FixedUpdate(){
        Vector3 mag = transform.position - lastPos;
        velocityMag = mag/Time.fixedDeltaTime;
        lastPos = transform.position;
    }
}
