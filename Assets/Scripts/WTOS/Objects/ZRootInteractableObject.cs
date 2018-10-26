using UnityEngine;
using System.Collections;

public class ZRootInteractableObject : MonoBehaviour {
    public enum InteractType{
        Pull,
        Usable,
        Grapple
    }

    public Rigidbody rb;
	public Transform zRootGrabHandle;
	public float zRootGrabOffset = 0.4f;
	public ZRoot.ZRootClawType zRootClawType = ZRoot.ZRootClawType.Handle;
    public bool isEnable = true;
    public InteractType interactDefinition;
	public float forceMultiplier = 1;

    public void HitByZRoot(ZRoot zr,Rigidbody rb){
        if(!isEnable)
            return;

		Vector3 dir = rb.transform.position - transform.position;

        switch(interactDefinition){
            case InteractType.Pull:
				PullForce(dir.normalized, dir.magnitude * forceMultiplier);
                break;
            case InteractType.Usable:
                UseObject();
                break;
            case InteractType.Grapple:
				GrappleForce(-dir.normalized, dir.magnitude * forceMultiplier, zr);
                break;
            default:
                break;
        }
    }

    virtual public void PullForce(Vector3 direction,float forcePower){
        rb.AddForce(direction * forcePower, ForceMode.VelocityChange);
    }

    virtual public void GrappleForce(Vector3 direction, float forcePower,ZRoot targetZR){
        targetZR.GrappleForce(direction, forcePower);
    }

    virtual public void UseObject(){

    }
}
