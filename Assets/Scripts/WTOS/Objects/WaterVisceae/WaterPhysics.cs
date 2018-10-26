using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterPhysics : MonoBehaviour {
    [SerializeField]
    float baseCoeficient = 50.0f,bounceDamp = 0.3f;

    [SerializeField]
    List<WaterInteractableObject> watList;
    public float waterLevel = 1.0f,floatHeight = 2.0f;
    BoxCollider waterCol;

    void Awake(){
        waterCol = GetComponent<BoxCollider>();
        watList = new List<WaterInteractableObject>();
        watList.Clear();
    }

    void FixedUpdate(){
        waterLevel = waterCol.bounds.max.y;
    
        foreach(WaterInteractableObject water in watList){
            Vector3 affectedPoint = water.transform.position;

            float forceCoeficient = 1.0f - (Mathf.Min(0.0f,(affectedPoint.y - waterLevel)) / floatHeight);
            float maximumDeviation = water.affectedRB.mass / baseCoeficient;

            if(forceCoeficient > 0.0f){
                Vector3 upLift = -Physics.gravity * (forceCoeficient - water.velocityMag.y * bounceDamp) * baseCoeficient * water.forceCoefficient;
//                if(!rb.GetComponent<Visceae>().rigidbodyOnTop){
//                    upLift *= Mathf.Min(1.0f,maximumDeviation);
//                }
                upLift *= Random.Range(0.9f,1.05f);

                water.affectedRB.AddForceAtPosition(upLift,affectedPoint,ForceMode.Force);
            }
        }
    }

    void OnTriggerEnter(Collider col){
        WaterInteractableObject water = col.GetComponent<WaterInteractableObject>();
        if(water && water.affectedRB != null){
            watList.Add(water);
        }


    }

    void OnTriggerStay(Collider col){
//        if(col.attachedRigidbody){
//            Vector3 affectedPoint = col.transform.position;
//            float forceCoeficient = 1.0f - ((affectedPoint.y - waterLevel) / floatHeight);
//            float maximumDeviation = col.attachedRigidbody.mass / baseCoeficient;
//
//            if(forceCoeficient > 0.0f){
//                Vector3 upLift = -Physics.gravity * (forceCoeficient - col.attachedRigidbody.velocity.y * bounceDamp) * baseCoeficient * Mathf.Min(1.0f,maximumDeviation);
//                col.attachedRigidbody.AddForceAtPosition(upLift,affectedPoint,ForceMode.Force);
//            }
//        }
    }

    void OnTriggerExit(Collider col){
        WaterInteractableObject water = col.GetComponent<WaterInteractableObject>();
        if(water && water.affectedRB != null){
            watList.Remove(water);
        }
    }
}
