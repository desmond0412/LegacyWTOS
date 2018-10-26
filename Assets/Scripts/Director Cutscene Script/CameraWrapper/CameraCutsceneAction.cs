using CinemaDirector;
using CinemaDirector.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CutsceneItem ("Camera", "In Cut Scene Action", CutsceneItemGenre.ActorItem)]
class CameraCutsceneAction : CinemaActorAction
{

	public float toFieldOfView = 30;
	public float transitionInDuration = 0.5f;
	public float transitionOutDuration = 0.5f;

	private float fromFieldOfView;

	public override void Trigger (GameObject Actor)
	{
		if (Actor != null) {
			fromFieldOfView = Actor.GetComponent<Camera> ().fieldOfView;
		}

		StartCoroutine (Restore (Actor.GetComponent<Camera> (), transitionInDuration, fromFieldOfView, toFieldOfView));

	}

	public override void End (GameObject Actor)
	{
		if (Actor != null) {
			StartCoroutine (Restore (Actor.GetComponent<Camera> (), transitionOutDuration, toFieldOfView, fromFieldOfView));
		}
	}


	private IEnumerator Restore (Camera myCamera, float transitionDuration, float fromFOV, float toFOV)
	{
		float duration = transitionDuration;
		float percentage = 0;
		float startTime = Time.time;
		if (duration > 0) {
			
			while (percentage < 1.0f) {
				percentage = Mathf.Clamp01 ((Time.time - startTime) / duration);
				myCamera.fieldOfView = Mathf.Lerp(fromFOV,toFOV,percentage);
				yield return new WaitForEndOfFrame ();

			}
		}

		myCamera.fieldOfView = toFOV;
	}


}

