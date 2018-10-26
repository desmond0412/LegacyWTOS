using CinemaDirector;
using CinemaDirector.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using SmartLocalization;

[CutsceneItem("Myuu Journal", "Parallax Action", CutsceneItemGenre.ActorItem)]
class ParallaxTriggerAction: CinemaActorAction
{
	public float fadeTime  = 1f;

	public override void Trigger(GameObject Actor)
	{
		if(Actor != null)
		{
			ParallaxController pc = Actor.GetComponent<ParallaxController>();
			if (pc==null)
			{
				print ("No Parallax Controller");
				return;
			}
			pc.ChangeBGGradually(fadeTime);
		}
	}

	public override void End (GameObject Actor)
	{
	}
}

