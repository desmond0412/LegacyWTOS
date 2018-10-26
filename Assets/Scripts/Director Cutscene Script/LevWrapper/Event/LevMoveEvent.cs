using CinemaDirector;
using CinemaDirector.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[CutsceneItem("Lev", "Move Event", CutsceneItemGenre.ActorItem)]
class LevMoveEvent : CinemaActorEvent
{
	[SerializeField]
	private LevController.Direction direction;

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

			switch(direction)
			{
				case LevController.Direction.Left : levController.moveLeft(); break;
				case LevController.Direction.Right : levController.moveRight(); break;
			}
		}
	}


}

