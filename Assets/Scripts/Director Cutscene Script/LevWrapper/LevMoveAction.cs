using CinemaDirector;
using CinemaDirector.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CutsceneItem("Lev", "Move Action", CutsceneItemGenre.ActorItem)]
class LevMoveAction : CinemaActorAction
{
	public LevController.Direction direction = LevController.Direction.Left;
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

			switch(direction)
			{
				case LevController.Direction.Left : levController.moveLeft(); break;
				case LevController.Direction.Right : levController.moveRight(); break;
			}
		}
	}

	public override void UpdateTime (GameObject Actor, float time, float deltaTime)
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

			switch(direction)
			{
			case LevController.Direction.Left : levController.moveLeft(); break;
			case LevController.Direction.Right : levController.moveRight(); break;
			}
		}
	}

	public override void End(GameObject Actor)
	{
		if(Actor != null)
		{
			levController = Actor.GetComponent<LevController>();

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

