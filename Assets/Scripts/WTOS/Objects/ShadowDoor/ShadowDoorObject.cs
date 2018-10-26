using UnityEngine;
using System.Collections;

public class ShadowDoorObject : MonoBehaviour {

	[SerializeField] ShadeHole shadhole;

	public void Trigger(bool isOn){
		shadhole.TriggerShadhole (isOn, 0f);
	}
}
