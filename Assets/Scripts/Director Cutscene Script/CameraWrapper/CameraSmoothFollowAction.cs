using CinemaDirector;
using CinemaDirector.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CutsceneItem("Camera", "Smooth Follow Action", CutsceneItemGenre.ActorItem)]
class CameraSmoothFollowAction : CinemaActorAction
{
	public Vector3 newLookAtOffset = Vector3.zero;
	public Vector3 newPosOffset = Vector3.zero;
	public float newTransitionTime = 1.0f;

	private Vector3 originPosOffset;
	private Vector3 originLookAtOffset;
	private float originTransitionTime;



	public override void Trigger(GameObject Actor)
	{
		if(Actor != null)
		{
			originPosOffset 		= Actor.GetComponent<Artoncode.Core.SmoothFollow>().positionOffset;
			originLookAtOffset		= Actor.GetComponent<Artoncode.Core.SmoothFollow>().lookAtOffset;
			originTransitionTime	= Actor.GetComponent<Artoncode.Core.SmoothFollow>().smoothTime;

			Actor.GetComponent<Artoncode.Core.SmoothFollow>().lookAtOffset = newLookAtOffset;
			Actor.GetComponent<Artoncode.Core.SmoothFollow>().positionOffset = newPosOffset;
			Actor.GetComponent<Artoncode.Core.SmoothFollow>().smoothTime = newTransitionTime;
		}
	}

	public override void End (GameObject Actor)
	{
		Actor.GetComponent<Artoncode.Core.SmoothFollow>().lookAtOffset = originLookAtOffset;
		Actor.GetComponent<Artoncode.Core.SmoothFollow>().positionOffset = originPosOffset;
//		Actor.GetComponent<Artoncode.Core.SmoothFoldlow>().smoothTime = originTransitionTime;
		StartCoroutine(RevertBackSmoothTime(Actor));
	}

	IEnumerator RevertBackSmoothTime(GameObject Actor)
	{
		yield return new WaitForSeconds(Actor.GetComponent<Artoncode.Core.SmoothFollow>().smoothTime*3);
		Actor.GetComponent<Artoncode.Core.SmoothFollow>().smoothTime = originTransitionTime;
	}




}

