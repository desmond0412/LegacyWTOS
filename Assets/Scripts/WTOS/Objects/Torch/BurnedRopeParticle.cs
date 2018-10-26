using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BurnedRopeParticle : MonoBehaviour {

	ParticleSystem[] ps;
	ParticleSystem.ShapeModule[] shapeMod;
	Vector3 lastScale;
	Vector3 lastPos;

	public float startDelay = 1f;
	public float delayFireAll = 2f;
	public Vector3 firstScale;
	public Vector3 firstPos;

	void Start(){
		ps = GetComponentsInChildren<ParticleSystem> ();
		lastScale = ps[0].shape.scale;
		lastPos = ps [0].transform.position;

		List<ParticleSystem.ShapeModule> tempPsp = new List<ParticleSystem.ShapeModule>();
		ps.ForEach(t=>tempPsp.Add(t.shape));
		shapeMod = tempPsp.ToArray ();

		shapeMod.ForEach (t => t.scale = firstScale);
		transform.Translate (firstPos, Space.Self);

//		StartCoroutine (StartFire ());
	}

	public void LightMyFire(){
		StartCoroutine (StartFire ());
	}

	IEnumerator StartFire(){
		yield return new WaitForSeconds (startDelay);
		ps [0].Play (true);
		iTween.MoveTo (gameObject, iTween.Hash ("position",lastPos,
			"time",delayFireAll,
			"easetype",iTween.EaseType.linear,
			"looptype",iTween.LoopType.none));
		float waktu = delayFireAll;
		Vector3 temp = lastScale - firstScale;
		while (waktu > 0f) {
			waktu -= Time.deltaTime;
			shapeMod.ForEach (t => t.scale += temp*Time.deltaTime/delayFireAll);
			yield return null;
		}
	}
}
