using UnityEngine;
using System.Collections;

public class DieTriggerDrown : DieTrigger {

	public override void Start()
	{
		base.Start();
		causes = DieCausesType.Drown;
	}

    protected override void OnTriggerEnter(Collider other)
    {
        if(isTriggered)return;

        if(other.CompareTag("Player"))
        {
            if(MainObjectSingleton.shared(MainObjectType.Player).GetComponent<LevController>().isGrounded) return;
            isTriggered = true;
            Trigger();
        }
    }

	public override void DieAnimationFlow(){
		MainObjectSingleton.shared(MainObjectType.Player).GetComponent<Rigidbody>().drag = 15;
		base.DieAnimationFlow();
	}

	protected override void CameraStartFocusOnLev()
	{
		camPosOffset += new Vector3(0,1,0);
		camLookAtOffset += new Vector3(0,1,0);
		base.CameraStartFocusOnLev();
	}

}
