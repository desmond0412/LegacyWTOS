using UnityEngine;
using System.Collections;

public class ShadowDoorTrigger : MonoBehaviour {

	[SerializeField] ShadowDoorObject shadowDoor;

	Coroutine triggerCoroutine;

	public void TriggerShadowDoor(bool isOn,float delay){
		if (triggerCoroutine != null)
			StopCoroutine (triggerCoroutine);
		triggerCoroutine = StartCoroutine (TriggerMainObject (isOn, delay));
	}

	IEnumerator TriggerMainObject(bool isOn,float delay)
	{
		yield return new WaitForSeconds(delay);
		shadowDoor.Trigger(isOn);
		triggerCoroutine = null;
	}
}
