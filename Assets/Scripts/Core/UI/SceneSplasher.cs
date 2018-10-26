using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Artoncode.Core.UI
{

	[RequireComponent (typeof(Image))]
	public class SceneSplasher : SingletonMonoBehaviour<SceneSplasher>
	{

		public delegate void SceneSplasherDelegate ();

		public event SceneSplasherDelegate OnSceneSplashEndCompleted;

		public List<Sprite> splashImage;
		public float fadeStart = 1.0f;
		public float fadeEnd = 1.5f;
		public float splashTime = 4.5f;

		public Vector3 scaleStart = new Vector3 (1.1f,1.1f,1.0f);
		public Vector3 scaleEnd = new Vector3 (1.0f,1.0f,1.0f);

		private int spriteIdx;
		private Image image;
		private Color clearWhite = new Color (1.0f, 1.0f, 1.0f, 0.0f);

		public override void Awake ()
		{
			base.Awake();
			image = this.GetComponent<Image> ();
			spriteIdx = 0;
		}

		public void Play ()
		{
			spriteIdx = 0;
			RunCurrentSplash();
		}

		void RunCurrentSplash()
		{
			image.enabled = true;
			if (image.sprite != splashImage [spriteIdx]) {
				image.sprite = splashImage[spriteIdx];
				image.color = clearWhite;
			}
			//fade in
			iTween.ValueTo (gameObject, iTween.Hash ("from",clearWhite,
			                                         "to",Color.white,
			                                         "time",fadeStart,
			                                         "delay",0f,
			                                         "onupdate","UpdateFade",
			                                         "ignoretimescale",(Time.timeScale==0f),
			                                         "easetype","easeInSine"));
			//scale down
			transform.localScale = scaleStart;
			iTween.ScaleTo (gameObject, iTween.Hash("scale",scaleEnd,
			                                        "time",splashTime,
			                                        "oncomplete","SplashEnd",
			                                        "ignoretimescale",(Time.timeScale==0f),
			                                        "easetype","linear"));


			//fade out
			iTween.ValueTo (gameObject, iTween.Hash ("from",Color.white,
			                                         "to",clearWhite,
			                                         "time",fadeEnd,
			                                         "delay",splashTime - fadeEnd,
			                                         "onupdate","UpdateFade",
			                                         "ignoretimescale",(Time.timeScale==0f),
			                                         "easetype","easeOutSine"));

		}
		void UpdateFade(Color c) {
			image.color = c;
		}

		void SplashEnd()
		{
			spriteIdx++;
			if (spriteIdx < splashImage.Count) {
				RunCurrentSplash();
			} else {
				if (OnSceneSplashEndCompleted != null)
					OnSceneSplashEndCompleted ();
			}
		}

//		private IEnumerator Splash ()
//		{
//			float startTime;
//			Color startColor;
//			float percentage = 0;
//
//			while (spriteIdx < splashImage.Count) {
//				image.enabled = true;
//				if (image.sprite != splashImage [spriteIdx]) {
//					image.sprite = splashImage [spriteIdx];
//					image.color = clearWhite;
//				}	
//
//				startTime = Time.time;
//				startColor = image.color;
//				percentage = 0;
//				while(percentage < 1.0f)
//				{
//					yield return new WaitForEndOfFrame ();
//					percentage = Mathf.Clamp01((Time.time - startTime) * fadeSpeed);
//					image.color = Color.Lerp (startColor, Color.white, percentage);				
//				}
////				while (image.color.a <= 0.97f) {
////					yield return new WaitForEndOfFrame ();
////					image.color = Color.Lerp (image.color, Color.white, Time.deltaTime * fadeSpeed);				
////				}
//				image.color = Color.white;
//
//				yield return new WaitForSeconds (fadeHoldTime);
//
//
//				startTime = Time.time;
//				startColor = image.color;
//				percentage = 0;
//				while(percentage < 1.0f)
//				{
//					yield return new WaitForEndOfFrame ();
//					percentage = Mathf.Clamp01((Time.time - startTime) * fadeSpeed);
//					image.color = Color.Lerp (startColor, clearWhite, percentage);				
//				}
//
////				while (image.color.a >= 0.03f) {
////					yield return new WaitForEndOfFrame ();
////					image.color = Color.Lerp (image.color, clearWhite, Time.deltaTime * fadeSpeed);				
////				}
//				image.color = clearWhite;
//				spriteIdx++;
//			}
//
//			if (OnSceneSplashEndCompleted != null)
//				OnSceneSplashEndCompleted ();
//
//		}

	}
}
