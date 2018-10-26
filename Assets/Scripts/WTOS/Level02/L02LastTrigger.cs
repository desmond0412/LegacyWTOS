using UnityEngine;
using System.Collections;

public class L02LastTrigger : MonoBehaviour {

	bool isTriggered = false;
	public Atataraina raitata;

	void OnTriggerEnter(Collider col){
		LevController temp = col.GetComponentInParent<LevController> ();
		if (temp != null && col.CompareTag("Player")) {
			if (!isTriggered){ //lev die
				isTriggered = true;
				raitata.SpeedUp();
			}
		}
	}
}
