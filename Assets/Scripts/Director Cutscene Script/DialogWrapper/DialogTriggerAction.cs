using CinemaDirector;
using CinemaDirector.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[CutsceneItem("Dialog", "Talk Action", CutsceneItemGenre.ActorItem)]
class DialogTriggerAction: CinemaActorAction
{
	public DialogSO dialog = null;
	public float fadeInTime  = 0.5f;
	public float fadeOutTime = 0.5f;
	public bool keepWithNext = false;
	public Vector2 extraOffset = Vector2.zero;
	private ActorSingleton actor;

	void Awake()
	{
		ResetDialogSO();
	}

	void ResetDialogSO()
	{
		dialog.lifetime = duration;
		dialog.fadeInTime = fadeInTime;
		dialog.fadeOutTime = fadeOutTime;
		dialog.isWithNext = keepWithNext;
		dialog.extraOffset = extraOffset;
	}

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
			ResetDialogSO();
		
			actor.Talk(dialog);
		}
	}

	public override void End (GameObject Actor)
	{

	}
}

