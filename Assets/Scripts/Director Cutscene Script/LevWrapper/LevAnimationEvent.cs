using CinemaDirector;
using CinemaDirector.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum LevCutsceneAnimation
{
	level02Arrival,
	level02MeetAtatar,
	level05GuribFall,
	level07AlookUpToNaivis,
}

[CutsceneItem("Lev", "Animation Event", CutsceneItemGenre.ActorItem)]
class LevAnimationEvent : CinemaActorEvent
{
	public LevCutsceneAnimation CutsceneType;

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

			levController.playCustomAnimation (GetAnimStringName (CutsceneType),0.25f);
		}
	}


	private string GetAnimStringName(LevCutsceneAnimation type)
	{
		switch (type) 
		{

		case LevCutsceneAnimation.level02Arrival: 
			return "AC_Lev_NoGauntlet_Thinking";

		case LevCutsceneAnimation.level02MeetAtatar: 
			return "AC_Lev_NoGauntlet_Atatar_Dodge";

		case LevCutsceneAnimation.level05GuribFall: 
			return "AC_Lev_NoGauntlet_P03_CS1";
		case LevCutsceneAnimation.level07AlookUpToNaivis:
			return "AC_Lev_Naivis_LookUp";

		}
		return "";
	}
}

