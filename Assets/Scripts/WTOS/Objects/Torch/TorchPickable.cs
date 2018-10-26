using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TorchPickable : PickableObject {

	public float MovingSpeed = .5f;
	public Vector3 WindVelo = Vector3.zero;
	public GameObject VFX;
	ParticleSystem[] particles;
	ParticleSystem.ForceOverLifetimeModule[] pForceModule;
	ParticleSystem.VelocityOverLifetimeModule[] pVeloModule;
	Vector3[] startingMaxForce;
	Vector3 lastPosition;

	Coroutine useCoroutine;

	void Awake () {
		particles = VFX.GetComponentsInChildren<ParticleSystem>();
		List<ParticleSystem.ForceOverLifetimeModule> tempPfom = new List<ParticleSystem.ForceOverLifetimeModule>();
		particles.ForEach(t=>tempPfom.Add(t.forceOverLifetime));
		pForceModule = tempPfom.ToArray ();
		List<Vector3> tempV3 = new List<Vector3> ();
		tempPfom.ForEach (t => tempV3.Add (new Vector3(t.x.constantMax,t.y.constantMax,t.z.constantMax)));
		startingMaxForce = tempV3.ToArray ();

		List<ParticleSystem.VelocityOverLifetimeModule> tempVelo = new List<ParticleSystem.VelocityOverLifetimeModule>();
		particles.ForEach(t=>tempVelo.Add(t.velocityOverLifetime));
		pVeloModule = tempVelo.ToArray ();
		SetVeloModule (WindVelo);
		lastPosition = VFX.transform.position;
	}

	void Start () {
		AkSoundEngine.PostEvent ("Start_firetorch", gameObject);
	}

	public void StopAudio()
	{
		AkSoundEngine.PostEvent ("Stop_firetorch", gameObject);
	}

	public static void StopAllTorches()
	{
		
		TorchPickable[] torches = GameObject.FindObjectsOfType<TorchPickable>();
		foreach (var item in torches) {
			item.StopAudio();
		}
	}

	void Update () {
		SetForceModule ((lastPosition != VFX.transform.position));
		lastPosition = VFX.transform.position;
	}

	void SetForceModule(bool isMoving){
		Vector3 temp = (lastPosition - VFX.transform.position)*MovingSpeed;
		if (isMoving) {
			for (int i = 0; i < pForceModule.Length; i++) {
				ParticleSystem.MinMaxCurve pfx = pForceModule [i].x;
//				pfx.constantMax += temp.x;
				pfx.constantMax = startingMaxForce[i].x + temp.x;
				pForceModule [i].x = pfx;

				pfx = pForceModule [i].y;
//				pfx.constantMax += temp.y;
				pfx.constantMax = startingMaxForce[i].y + temp.y;
				pForceModule [i].y = pfx;

				pfx = pForceModule [i].z;
//				pfx.constantMax += temp.z;
				pfx.constantMax = startingMaxForce[i].z + temp.z;
				pForceModule [i].z = pfx;
			}
		} else {
			for (int i = 0; i < pForceModule.Length; i++) {
				ParticleSystem.MinMaxCurve pfx = pForceModule [i].x;
				pfx.constantMax = startingMaxForce[i].x;
				pForceModule [i].x = pfx;

				pfx = pForceModule [i].y;
				pfx.constantMax = startingMaxForce[i].y;
				pForceModule [i].y = pfx;

				pfx = pForceModule [i].z;
				pfx.constantMax = startingMaxForce[i].z;
				pForceModule [i].z = pfx;
			}
		}
	}

	void SetVeloModule(Vector3 velo){
		for (int i = 0 ; i < pVeloModule.Length; i++) {
			ParticleSystem.MinMaxCurve pfx = pVeloModule[i].x;
			pfx.constantMax += velo.x;
			pfx.constantMin += velo.x;
			pVeloModule[i].x = pfx;

			pfx = pVeloModule[i].y;
			pfx.constantMax += velo.y;
			pfx.constantMin += velo.y;
			pVeloModule[i].y = pfx;

			pfx = pVeloModule[i].z;
			pfx.constantMax += velo.z;
			pfx.constantMin += velo.z;
			pVeloModule[i].z = pfx;
		}
	}
		
	public override void PickFunction ()
	{
		if(!isAccessible) return;
		base.PickFunction ();
		GetComponent<BoxCollider> ().enabled = false;
	}
}
