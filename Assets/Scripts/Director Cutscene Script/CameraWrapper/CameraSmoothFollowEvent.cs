using CinemaDirector;
using CinemaDirector.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CutsceneItem("Camera", "Smooth Follow Event", CutsceneItemGenre.ActorItem)]
class CameraSmoothFollowEvent : CinemaActorEvent
{
	public Transform target;

	public bool isWithReinit = true;

	private float originSmoothTime;
	public override void Trigger(GameObject Actor)
	{
		if(Actor != null)
		{
			Actor.GetComponent<Artoncode.Core.SmoothFollow>().target = target;
			if(isWithReinit)
				Actor.GetComponent<Artoncode.Core.SmoothFollow>().Reinit();

//			if(Actor.GetComponent<Artoncode.Core.SmoothFollow>().lookAtTarget)
//			{
//				Actor.GetComponent<Artoncode.Core.SmoothFollow>().LookAt = target.position + Actor.GetComponent<Artoncode.Core.SmoothFollow>().lookAtOffset;	
//			}
		}
	}






}

