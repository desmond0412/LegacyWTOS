using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Naivis : UsableObject
{

	public delegate void NaivisDelegate ();

	public event NaivisDelegate OnLightTurnedOn;
	public event NaivisDelegate OnLightTurnedOff;
	public event NaivisDelegate OnLightFinishedCasting;

	public Color lightColorSource;
	public NewLightVisualVFX NaivisLight;
	public bool state;
	public float DespawnDelay = 10f;
	public bool isOneTimeTrigger = false;

	[HideInInspector]
	public bool isLocked = false;

	Animator anim;
	float timelimit = 0f;


	public bool TURNON_MODE = false;
	//true == trigger light at start
	public bool DEBUG_MODE = false;

	void Awake ()
	{
		anim = GetComponent<Animator> ();
		if (DEBUG_MODE)
			UseFunction ();
	}



	public override void UseFunction ()
	{
		if (!isAccessible)
			return;
		isLocked = isOneTimeTrigger;
		base.UseFunction ();
		anim.SetBool ("Use", true);

			
	}

	void Update ()
	{
		if (isLocked)
			return;
		
		if (TURNON_MODE)
			return;

		if (timelimit > 0.0f) {
			timelimit -= Time.deltaTime;
			if (timelimit <= 0.0f) {
				timelimit = 0.0f;
				anim.SetBool ("Use", false);
			}
		}

	}

	//animator call
	void Deactivate ()
	{
		state = false;
		if (OnLightTurnedOff != null)
			OnLightTurnedOff ();
		NaivisLight.DisableLight ();
	}

	void Activate ()
	{
		state = true;
		if (OnLightTurnedOn != null)
			OnLightTurnedOn ();
		NaivisLight.EnableLight (this, lightColorSource);
		timelimit += DespawnDelay;
	}


	public void ForcedTurnOnLight ()
	{
		anim.SetBool ("Use", true);
		anim.speed = 1000f;
		StartCoroutine (RevertSpeedAnim ());
		TURNON_MODE = true;
	}

	IEnumerator RevertSpeedAnim ()
	{
		yield return null;
		yield return null;
		yield return null;
		anim.speed = 1f;
	}
	//force the light to turned off
	public void ForcedTurnOffLight ()
	{
		TURNON_MODE = false;
		timelimit = 0.00001f;
	}

	public void FinishLightCasting ()
	{
		if (OnLightFinishedCasting != null)
			OnLightFinishedCasting ();
	}
}
