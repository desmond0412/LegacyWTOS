using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

	public bool isPausingWhileTriggered = true;

	[Header ("Trigger Movement Path(this gameobject)")]
	[SerializeField] iTween.EaseType easetypeTrigger;
	[SerializeField] float timeFinishOneTrigger = 5f;

	[Header ("Basic Movement Path(children)")]
	[SerializeField] iTween.EaseType easetypeBasic;
	[SerializeField] iTween.LoopType looptypeBasic;
	[SerializeField] float timeFinishOneBasic = 5f;

	bool StillTriggering = false;
	GameObject childItween;
	bool isOn = true;

	void Awake(){
		if (transform.childCount > 0) {
			childItween = transform.GetChild (0).gameObject;
			Vector3[] path = childItween.GetComponent<iTweenPath> ().nodes.ToArray ();
			Vector3 temp = path [0];
			for (int i = 0; i < path.Length; i++)
				path [i] -= temp;
			iTween.MoveTo (childItween, iTween.Hash ("path", path,
				"time", timeFinishOneBasic,
				"easetype", easetypeBasic,
				"looptype", looptypeBasic,
				"movetopath", false,
				"islocal", true));
		}
	}

	public void Trigger(){
		if (StillTriggering)
			return;
		StillTriggering = true;
		if (childItween != null && isPausingWhileTriggered)
			iTween.Pause (childItween);
		Vector3[] path = GetComponent<iTweenPath> ().nodes.ToArray ();
		if (!isOn)
			System.Array.Reverse (path);
		iTween.MoveTo (gameObject,iTween.Hash("path",path,
			"time",timeFinishOneTrigger,
			"easetype",easetypeTrigger,
			"looptype",iTween.LoopType.none,
			"movetopath",false,
			"oncomplete","FinishTriggering",
			"oncompletetarget",this.gameObject));
	}

	void FinishTriggering(){
		StillTriggering = false;
		isOn = !isOn;
		if (childItween != null && isPausingWhileTriggered)
			iTween.Resume (childItween);
	}
}
