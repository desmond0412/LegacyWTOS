using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Artoncode.Core;


namespace Artoncode.Core.UI
{
	[RequireComponent(typeof(Image))]
	public class SceneFader : MonoBehaviour
	{
		public delegate void SceneFaderDelegate ();

		public event SceneFaderDelegate OnFadeInBegin;
		public event SceneFaderDelegate OnFadeInCompleted;
		public event SceneFaderDelegate OnFadeOutBegin;
		public event SceneFaderDelegate OnFadeOutCompleted;

		public Color fadeColor = Color.black;
		public float fadeSpeed = 2;

		public bool awakeFadeIn = true; 

		private float fadeStartTime;
		private Color fadeStartColor;

		private Image myImage;
		private bool isFadingIn;
		private bool isFadingOut;

		public bool isFading{
			get{
				return isFadingOut || isFadingIn;
			}
		}


		private void SetupCanvasGroup()
		{
			if(gameObject.GetComponent<CanvasGroup>() == null)
				gameObject.AddComponent<CanvasGroup>();
			
			gameObject.GetComponent<CanvasGroup>().interactable = false;
			gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;	

		}
//		public override void Awake ()

		public static SceneFader shared()
		{
			if(MainObjectSingleton.shared(MainObjectType.SceneFader) !=null)
				return MainObjectSingleton.shared(MainObjectType.SceneFader).GetComponent<SceneFader>();
			return null;
		}

		public void Awake ()
		{
			gameObject.transform.SetAsLastSibling();
			gameObject.GetComponent<RectTransform>().anchorMin = Vector2.zero;
			gameObject.GetComponent<RectTransform>().anchorMax = Vector2.one;
			gameObject.GetComponent<RectTransform>().pivot= Vector2.one / 2;
			gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
			gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
			gameObject.GetComponent<RectTransform>().localScale = Vector3.one * 2;

			myImage = gameObject.GetComponent<Image>();
			myImage.color = Color.clear;
			myImage.enabled = false;
			isFadingIn = false;
			isFadingOut = false;
			SetupCanvasGroup();
		}

		void Start()
		{
			if(awakeFadeIn)
				FadeInStart();
		}

		public void FadeInStart()
		{
			if (OnFadeInBegin != null)
				OnFadeInBegin ();
			myImage.color = fadeColor;
			fadeStartTime= Time.time;
			fadeStartColor = myImage.color;
			isFadingIn = false;
			isFadingOut = false;
			FadeIn();

		}

		public void FadeOutStart()
		{
			if (OnFadeOutBegin != null)
				OnFadeOutBegin ();

			myImage.color = Color.clear;
			fadeStartTime= Time.time;
			fadeStartColor = myImage.color;
			isFadingIn = false;
			isFadingOut = false;
			FadeOut();
		}


		void FadeToClear ()
		{
			float percentage = Mathf.Clamp01((Time.time - fadeStartTime) / fadeSpeed);
			myImage.color = Color.Lerp(fadeStartColor, Color.clear, percentage);

//			myImage.color = Color.Lerp(myImage.color, Color.clear, fadeSpeed * Time.deltaTime);
		}

		void FadeToBlack ()
		{
			float percentage = Mathf.Clamp01((Time.time - fadeStartTime) / fadeSpeed);
			myImage.color = Color.Lerp(fadeStartColor, fadeColor, percentage);


//			myImage.color = Color.Lerp(myImage.color, fadeColor, fadeSpeed * Time.deltaTime);
		}

		void Update ()
		{
			if (isFadingIn) {
				FadeIn ();
			}


			if (isFadingOut) {
				FadeOut ();
			}
		}

		void FadeIn ()
		{
			myImage.enabled = true;
			isFadingIn = true;
			FadeToClear ();
			if (myImage.color.a <= .002f) {

				myImage.color = Color.clear;
				isFadingIn = false;
				if (OnFadeInCompleted != null)
					OnFadeInCompleted ();
			}

		}

		void FadeOut ()
		{
			myImage.enabled = true;
			isFadingOut = true;
			FadeToBlack ();

			if (myImage.color.a >= 0.998f) {
				myImage.color = fadeColor;
				isFadingOut = false;
				if (OnFadeOutCompleted != null)
					OnFadeOutCompleted ();
			}


		}
	}




}
