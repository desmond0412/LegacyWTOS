using UnityEngine;
using System.Collections;

public class Visceae : WaterInteractableObject {
    public bool rigidbodyOnTop = false;

    void OnCollisionEnter(Collision col){
        if(col.collider.attachedRigidbody && col.gameObject.CompareTag("Player")){
            rigidbodyOnTop = true;
        }
    }

    void OnCollisionExit(Collision col){
        if(col.collider.attachedRigidbody && col.gameObject.CompareTag("Player")){
            GetComponent<Rigidbody>().AddForce(new Vector3(0.0f,
				-1 * Mathf.Sqrt (-2 * col.gameObject.GetComponent<LevController>().levSetting.maxJumpHeight * Physics.gravity.y),
                0.0f),ForceMode.VelocityChange);

            rigidbodyOnTop = false;
        }
    }
}
