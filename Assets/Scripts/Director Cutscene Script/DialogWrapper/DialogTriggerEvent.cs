using CinemaDirector;
using CinemaDirector.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[CutsceneItem("Dialog", "Trigger Talk Event", CutsceneItemGenre.ActorItem)]
class DialogTriggerEvent : CinemaActorEvent
{
	public DialogSO dialog = null;
	private ActorSingleton actor;

	public override void Trigger(GameObject Actor)
	{
		if(Actor != null)
		{
			actor = Actor.GetComponent<ActorSingleton>();
			if(actor == null )
			{
				Debug.LogError("Actor is does not have Actor Singleton");
				return;					
			}

			actor.Talk(dialog);
		}
	}


}

