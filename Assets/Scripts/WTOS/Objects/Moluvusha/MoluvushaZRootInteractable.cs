using UnityEngine;
using System.Collections;

public class MoluvushaZRootInteractable : ZRootInteractableObject{
    public Animator molAnim;

    [SerializeField]
    Moluvusha molScript;

    [SerializeField]
    Transform trashPool;

    int plugTrigger;

    void Awake(){
        plugTrigger = Animator.StringToHash("Plugged");
    }

    void Start(){
        if(molScript == null){
            molScript = GetComponentInParent<Moluvusha>();

            if(molAnim == null){
                molScript.gameObject.GetComponent<Animator>();
            }
        }
    }
        
    public override void PullForce(Vector3 direction, float forcePower)
    {
        molAnim.SetTrigger(plugTrigger);
        molScript.gameObject.GetComponentInChildren<MoluvushaPickable>().objectLayer = PickableObject.LayerObject.Mid;

        if(molScript.isAttached){
            if(GetComponentInParent<MoluvushaHolder>()){
                GetComponentInParent<MoluvushaHolder>().RemoveMoluvusha();
            }

            rb.isKinematic = false;
            molScript.isAttached = false;
        }

        base.PullForce(direction, forcePower);
    }
}
