using UnityEngine;
using System.Collections;
using Artoncode.Core;

public class L16CopaScript : MonoBehaviour {
	public SmoothFollowAreaTrigger area;

//    [SerializeField]
//    BoxCollider cameraCollider;
//
	float transition = 2;
	float delay = 8.0f;
    Vector3 newCenter = new Vector3(0.0f,-3.0f,0.0f);
	KopaScript copScript;

    void Start(){
		copScript = GetComponent<KopaScript>();
		copScript.OnKopaUnplugged += CopScript_OnKopaUnplugged;
    }

    void CopScript_OnKopaUnplugged (GameObject sender)
    {
		StartCoroutine(delayTrigger());
    }

    void CopScript_OnKopaPlugged (GameObject sender){

    }

	private void TriggerCamera()
	{
		area.GetComponent<BoxCollider>().center = newCenter;
		area.RevertSettings();
		area.smoothTransition = transition;
		area.AssignNewSettings();
	}

	IEnumerator delayTrigger()
	{
		yield return new WaitForSeconds(delay);
		TriggerCamera();
	}


}
