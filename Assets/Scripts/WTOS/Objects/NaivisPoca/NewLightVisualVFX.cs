using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class NewVisualData{
	public float MaxScale;
	public Vector3 LightAngle;
	public Vector3 RealPocaAngle;
	public float RayMultiplier = 7f;
	public float ParticleDustLength;
}

public class NewLightVisualVFX : MonoBehaviour {
	
	public bool DEBUG_MODE = false;

	public GameObject lightParticles;
	ParticleSystem particleCreated;

	public NewVisualData lightData;
	public PocaPetal thisPocaUnusable;

	public bool Triggered{ get; private set;}



	ParticleSystem.ShapeModule shapeMod;
	Vector3 transformUp;
	ColorAnimator cAnimator;

	Naivis naivisSource;
	float scaleMultiplierVisualLight = 0.8f; //visual light scale multiplier , scaled from v3.one
	bool isRayCasted = false;

	void Awake(){
		Triggered = false;
		if(!(cAnimator = this.gameObject.GetComponent<ColorAnimator>()))
			cAnimator =	this.gameObject.AddComponent<ColorAnimator>();
		

		cAnimator.OnColorChanged += CAnimator_OnColorChanged;
		cAnimator.OnColorChangedCompleted += CAnimator_OnColorChangedCompleted;
		cAnimator.fadingTime = 0.5f;
	}

	//triggered by activating
	public void EnableLight(Naivis source,Color lightColorSource){
		if (!Triggered) {
			Triggered = true;
			naivisSource = source;

			GameObject temp = Instantiate (lightParticles, transform.position, transform.rotation) as GameObject;
			particleCreated = temp.GetComponent<ParticleSystem> ();
			particleCreated.transform.localScale = Vector3.one * scaleMultiplierVisualLight;
			particleCreated.transform.SetParent (transform.parent);

			shapeMod = particleCreated.transform.GetChild (0).GetComponent<ParticleSystem> ().shape;
			shapeMod.length = lightData.ParticleDustLength;
			particleCreated.startColor = lightColorSource;


			particleCreated.Play ();
			particleCreated.transform.localEulerAngles = lightData.LightAngle;
			particleCreated.transform.localScale = new Vector3 (particleCreated.transform.localScale.x, lightData.MaxScale, particleCreated.transform.localScale.z);
			transformUp = particleCreated.transform.up;
			isRayCasted = false;
			MovingLight ();
		}
	}

	void Update(){

		//for debugging use
		if (DEBUG_MODE) {
			CastRay (true, Color.yellow);
			UpdateParameters ();
		}
	}
	void UpdateParameters(){
		if (particleCreated != null) {
			if (thisPocaUnusable != null)
				thisPocaUnusable.transform.localEulerAngles = lightData.RealPocaAngle;
			particleCreated.transform.localEulerAngles = lightData.LightAngle;
			particleCreated.transform.localScale = new Vector3 (particleCreated.transform.localScale.x, lightData.MaxScale, particleCreated.transform.localScale.z);
			shapeMod.length = lightData.ParticleDustLength;
		}
	}

	public void DisableLight(){
		if (Triggered) {
			Triggered = false;
			naivisSource = null;

			Material mat = particleCreated.GetComponent<ParticleSystemRenderer>().material;
			Color sourceColor = mat.GetColor("_TintColor");
			Color targetColor = Color.clear;
			mat.SetColor("_TintColor",sourceColor );

			cAnimator.Trigger(sourceColor,targetColor,particleCreated.gameObject);

			mat = particleCreated.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material;
			mat.SetColor("_TintColor",sourceColor );

			cAnimator.Trigger(sourceColor,targetColor,particleCreated.transform.GetChild(0).gameObject);

			CastRay (false,Color.clear);
			particleCreated.Stop ();
			Destroy (particleCreated.gameObject, 5f);
		}
	}

	void MovingLight(){
		Material mat = particleCreated.GetComponent<ParticleSystemRenderer>().material;
		Color sourceColor = Color.clear;
		Color targetColor = mat.GetColor("_TintColor");
		mat.SetColor("_TintColor",sourceColor );

		cAnimator.Trigger(sourceColor,targetColor,particleCreated.gameObject);

		mat = particleCreated.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material;
		mat.SetColor("_TintColor",sourceColor );

		cAnimator.Trigger(sourceColor,targetColor,particleCreated.transform.GetChild(0).gameObject);
}

	void CAnimator_OnColorChangedCompleted (GameObject sender, float percentage,Color color)
	{
		
	}

	void CAnimator_OnColorChanged (GameObject sender, float percentage, Color color)
	{
		Material mat = sender.GetComponent<ParticleSystemRenderer>().material;
		mat.SetColor("_TintColor",color);

		if (sender.transform.childCount > 0) {
			mat = sender.transform.GetChild (0).GetComponent<ParticleSystemRenderer> ().material;
			mat.SetColor ("_TintColor", color);
		}

		if(percentage > 0.2f && !isRayCasted && Triggered )
		{
			isRayCasted = true;
			CastRay (true,sender.GetComponent<ParticleSystem>().startColor);
		}
	}

	void CastRay(bool isTurnOn, Color lightSourceColor){
		ShootRay (transform.position,(particleCreated != null) ? particleCreated.transform.up : transformUp, lightData.MaxScale * lightData.RayMultiplier,isTurnOn, lightSourceColor);
	}

	void ShootRay(Vector3 OriginPosition, Vector3 Direction,float LightDistance,bool isTurnOn, Color lightSourceColor){
		RaycastHit[] allObjectHit = Physics.RaycastAll (OriginPosition, Direction, LightDistance);
		Debug.DrawLine (OriginPosition, OriginPosition + Direction * LightDistance);
		bool isCastingFinished = true;
		if (allObjectHit != null) {
			allObjectHit = allObjectHit.Where (x => x.transform.GetComponent<ShadowDoorTrigger> () != null || x.transform.GetComponent<ShadeHole> () != null || x.transform.GetComponent<PocaPetal> () != null).ToArray ();
			foreach (RaycastHit objectHit in allObjectHit) {
				ShadeHole shh = objectHit.transform.gameObject.GetComponent<ShadeHole> ();
				PocaPetal pu = objectHit.transform.gameObject.GetComponent<PocaPetal> ();
				ShadowDoorTrigger sdo = objectHit.transform.gameObject.GetComponent<ShadowDoorTrigger> ();
				if (pu) {
					if (!thisPocaUnusable) {
						pu.HasLight (isTurnOn, lightSourceColor, naivisSource);
						isCastingFinished = false;
					} else {
						if (thisPocaUnusable != pu) {
							pu.HasLight (isTurnOn, lightSourceColor, naivisSource);
							isCastingFinished = false;
						}
					}
				}
				if (shh) {
					shh.TriggerShadhole (!isTurnOn, (!isTurnOn) ? cAnimator.fadingTime*0.2f : 0f);
				}
				if (sdo) {
					sdo.TriggerShadowDoor (isTurnOn , (!isTurnOn) ? cAnimator.fadingTime*0.2f : 0f);
				}
			}
		}
		if (isCastingFinished && isTurnOn) {
			if(naivisSource != null)
				naivisSource.FinishLightCasting ();
		}
	}
}
