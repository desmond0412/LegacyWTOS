using CinemaDirector;
using CinemaDirector.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using SmartLocalization;

[CutsceneItem("Myuu Journal", "Sprite Action", CutsceneItemGenre.ActorItem)]
class SpriteTriggerAction: CinemaActorAction
{
	public Sprite spriteSource;
	public float fadeInTime  = 0.3f;
	public float fadeOutTime = 0.3f;

	private Image aImage;
	private Color clearColor;

	void Awake()
	{
		clearColor = new Color (1f, 1f, 1f, 0f);
	}


	public override void Trigger(GameObject Actor)
	{
		if(Actor != null)
		{
			aImage = Actor.GetComponent<Image>();
			if (aImage==null)
			{
				print ("No Image Component");
				return;
			}
			aImage.sprite = spriteSource;
			if (fadeInTime>0f) {
				aImage.color = clearColor;
				iTween.ValueTo (gameObject, iTween.Hash ("from",clearColor,
				                                         "to",Color.white,
				                                         "time",fadeInTime,
				                                         "onupdate","UpdateFade",
				                                         "easetype","easeInCubic"));
			}
		}
	}
	void UpdateFade(Color c)
	{
		aImage.color = c;
	}

	public override void End (GameObject Actor)
	{
		if(Actor != null)
		{
			aImage = Actor.GetComponent<Image>();
			if (aImage==null)
			{
				print ("No Image Component");
				return;
			}
			if (fadeOutTime>0f) {
				aImage.color = Color.white;
				iTween.ValueTo (gameObject, iTween.Hash ("from",Color.white,
				                                         "to",clearColor,
				                                         "time",fadeOutTime,
				                                         "onupdate","UpdateFade",
				                                         "easetype","easeInCubic"));
			}
		}
	}
}

