using UnityEngine;
using System.Collections;

public abstract class PickableObject : MonoBehaviour {

	public enum LayerObject {
		Back,
		Mid,
		Front
	}
	public LayerObject objectLayer = LayerObject.Mid;

	protected bool isPicked = false;
	public Rigidbody rb;
	public string pickAnimation;
	public string carryAnimation;
	public string dropAnimation;
	public Transform root;
	public Transform pickHandle;
	public bool isAccessible = true;
    public bool isLockedToPicked = false;
    Collider[] cols;

    virtual public void OnStartLocked(){

    }

	virtual public void PickFunction(){
		if (!isAccessible) return;
        cols = GetComponentsInChildren<Collider>();
        foreach(Collider c in cols){
            if(!c.isTrigger){
                c.enabled = false;
            }
        }

		isPicked = true;
		if (rb) {
			rb.isKinematic = true;
		}
	}

	virtual public void UseFunction(){
		
	}

	virtual public void DropFunction(Vector3 velocity) {
        foreach(Collider c in cols){
            if(!c.isTrigger){
                c.enabled = true;
            }
        }

		isPicked = false;
		if (rb) {
			rb.isKinematic = false;
			rb.velocity = velocity;
		}
	}

    virtual public void OnEndLocked(){

    }
}
