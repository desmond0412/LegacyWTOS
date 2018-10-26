using CinemaDirector;
using CinemaDirector.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;


[CutsceneItem("Lev", "Stop Move Event", CutsceneItemGenre.ActorItem)]
class LevMoveStopEvent : CinemaActorEvent
{
	public float deceleration = 4;

	private float restoreAfter = 3;
	private float originAcceleration;
	private LevController levController;

	public override void Trigger(GameObject Actor)
	{
		if(Actor != null)
		{
			levController = Actor.GetComponent<LevController>();
			if(levController == null )
			{
				Debug.LogError("Actor is does not have Lev Controller");
				return;					
			}

			originAcceleration = levController.levSetting.runAcceleration;
			levController.levSetting.runAcceleration = deceleration;
			levController.stopMoving();
			StartCoroutine(RestoreAcceleration());
		}
	}


	private IEnumerator RestoreAcceleration()
	{
		yield return new WaitForSeconds (restoreAfter);
		levController.levSetting.runAcceleration = originAcceleration;

	}


}

