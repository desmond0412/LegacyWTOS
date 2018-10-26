using UnityEngine;
using System.Collections;

public class L02DieAtatarTrigger : DieTrigger {

	public float animationDelay;

	public override void Trigger(bool isCulpritIncluded = true){
		MainObjectSingleton.shared(MainObjectType.Player).GetComponent<LevController>().setInputEnable(false);
		MainObjectSingleton.shared(MainObjectType.Player).GetComponent<LevController>().stopMoving();
		StartCoroutine(GameoverWithDelay(animationDelay,isCulpritIncluded));
	}

	IEnumerator GameoverWithDelay(float delay, bool isCulpritIncluded)
	{
		yield return new WaitForSeconds(delay);
		base.Trigger(isCulpritIncluded);
	}

	protected override void CameraStartFocusOnLev ()
	{
		camPosOffset = new Vector3 (0,2,-13);
		camLookAtOffset = new Vector3 (0,2,0);
		base.CameraStartFocusOnLev ();
	}




}
