using UnityEngine;
using System.Collections;

public class AtatarAnimation : MonoBehaviour {

	public GameObject atatarJawEnd;
	public GameObject atatarFoot;

	void EndAttackMiss(){
		GetComponentInParent<Atataraina> ().EndAttackMiss ();
	}
}
