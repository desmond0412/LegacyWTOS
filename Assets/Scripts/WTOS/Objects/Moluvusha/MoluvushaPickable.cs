using UnityEngine;
using System.Collections;

public class MoluvushaPickable : PickableObject {
    [SerializeField]
    Moluvusha molScript;

    Collider pickableCollider,eatableCollider;

    string hangingPick = "AC_Lev_Moluvusha_Pick";
    string groundPick = "AC_Lev_A1_PickUp_Ground";

    void Start(){
        if(!molScript){
            molScript = GetComponent<Moluvusha>();
        }

        pickableCollider = molScript.pickableCollider;
        eatableCollider = molScript.eatableCollider;
    }

    void Update(){
        pickAnimation = molScript.isAttached ? hangingPick : groundPick;
    }

    public override void OnStartLocked()
    {
        base.OnStartLocked();

        pickableCollider.enabled = false;
        eatableCollider.enabled = false;
    }

    public override void PickFunction()
    {
		if(!isAccessible) return;

        if(molScript.isAttached){
            molScript.PullMoluvusha();
        }

        base.PickFunction();
    }

    public override void DropFunction(Vector3 velocity)
    {
        pickableCollider.enabled = true;
        eatableCollider.enabled = true;

		base.DropFunction(velocity);
    }
}

