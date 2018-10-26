using CinemaDirector;
using CinemaDirector.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using TMPro;
using SmartLocalization;

[CutsceneItem("Dialog", "Parallax Talk Action", CutsceneItemGenre.ActorItem)]
class DialogParallaxTriggerAction: CinemaActorAction
{
	public string dialogId = "";
	public float fadeInTime  = 0.3f;
	public float fadeOutTime = 0.3f;

	private TextMeshProUGUI aText;

	public override void Trigger(GameObject Actor)
	{
		if(Actor != null)
		{
			aText = Actor.GetComponent<TextMeshProUGUI>();
			if (aText==null)
			{
				print ("No Text Component");
				return;
			}
			aText.text = LanguageManager.Instance.GetTextValue(dialogId);
			if (fadeInTime>0f) {
				aText.color = Color.clear;
				iTween.ValueTo (gameObject, iTween.Hash ("from",Color.clear,
				                                         "to",Color.black,
				                                         "time",fadeInTime,
				                                         "onupdate","UpdateFade",
				                                         "easetype","easeInCubic"));
			}
		}
	}
	void UpdateFade(Color c)
	{
		aText.color = c;
	}

	public override void End (GameObject Actor)
	{
		if(Actor != null)
		{
			aText = Actor.GetComponent<TextMeshProUGUI>();
			if (aText==null)
			{
				print ("No Text Component");
				return;
			}
			if (fadeOutTime>0f) {
				aText.color = Color.black;
				iTween.ValueTo (gameObject, iTween.Hash ("from",Color.black,
				                                         "to",Color.clear,
				                                         "time",fadeOutTime,
				                                         "onupdate","UpdateFade",
				                                         "easetype","easeInCubic"));
			}
		}
	}
}

