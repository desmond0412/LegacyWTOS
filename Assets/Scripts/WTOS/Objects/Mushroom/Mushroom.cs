using UnityEngine;
using System.Collections;

public class Mushroom : MonoBehaviour {

	public GameObject Lev;
	Vector3 additional = new Vector3(3.5f,-1.3f);
	float delay = 3f;

	void Start(){
		iTween.MoveBy (Lev, iTween.Hash ("y",-2.5f,
			"time", delay,
			"easetype",iTween.EaseType.easeOutBounce,
			"looptype",iTween.LoopType.none));
		StartCoroutine (AdditionalMove ());
	}

	void Update () {
		
	}

	IEnumerator AdditionalMove(){
		float waktu = delay;
		while (waktu > 0f) {
			waktu -= Time.deltaTime;
			Lev.transform.Translate (additional * Time.deltaTime / delay);
			yield return null;
		}
	}
}
