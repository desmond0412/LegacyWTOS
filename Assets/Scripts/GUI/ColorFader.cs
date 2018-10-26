using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Artoncode.Core;

public enum ColorFaderType {
	Full,
	Half,
	Clear
}


[RequireComponent(typeof(Image))]
public class ColorFader : MonoBehaviour
{
	public delegate void ColorFaderDelegate ();

	public event ColorFaderDelegate OnFadeInBegin;
	public event ColorFaderDelegate OnFadeInCompleted;
	public event ColorFaderDelegate OnFadeOutBegin;
	public event ColorFaderDelegate OnFadeOutCompleted;

	public Color fadeColor = Color.black;
	public Color fadeHalfColor;
	public bool allowAnimate = true;

	public void FadeToClear (float fadeTime=1f, float delay=0f)
	{
		gameObject.SetActive (true);
		Color c = GetComponent<Image> ().color;
//		print ("FadeToClear IgnoreTimeScale: "+(Time.timeScale==0f));
		iTween.ValueTo (gameObject, iTween.Hash ("from",c,
		                                         "to",Color.clear,
		                                         "time",fadeTime,
		                                         "delay",delay,
		                                         "name","FadeAnim",
		                                         "onstart","OnFadeClearStart",
		                                         "onupdate","UpdateFade",
		                                         "oncomplete","OnFadeClearComplete",
		                                         "ignoretimescale",(Time.timeScale==0f),
		                                         "easetype","easeOutSine"));
	}

	void OnFadeClearStart()
	{
//		if (!allowAnimate) {
//			allowAnimate = true;
//			GetComponent<Image> ().color = Color.clear;
//		}
		if (OnFadeInBegin != null)
			OnFadeInBegin ();
	}
	void OnFadeClearComplete()
	{
//		gameObject.SetActive (false);
		if (OnFadeInCompleted != null)
			OnFadeInCompleted ();
	}

	public void FadeToColor (float fadeTime=1f,float delay=0f)
	{
		gameObject.SetActive (true);
		iTween.ValueTo (gameObject, iTween.Hash ("from",Color.clear,
		                                         "to",fadeColor,
		                                         "time",fadeTime,
		                                         "delay",delay,
		                                         "name","FadeAnim",
		                                         "onstart","OnFadeColorStart",
		                                         "onupdate","UpdateFade",
		                                         "oncomplete","OnFadeColorComplete",
		                                         "ignoretimescale",(Time.timeScale==0f),
		                                         "easetype","easeOutSine"));
	}
	public void FadeToHalfColor (float fadeTime=1f,float delay=0f)
	{
		gameObject.SetActive (true);
		iTween.ValueTo (gameObject, iTween.Hash ("from",Color.clear,
		                                         "to",fadeHalfColor,
		                                         "time",fadeTime,
		                                         "delay",delay,
		                                         "name","FadeAnim",
		                                         "onstart","OnFadeHalfColorStart",
		                                         "onupdate","UpdateFade",
		                                         "oncomplete","OnFadeColorComplete",
		                                         "ignoretimescale",(Time.timeScale==0f),
		                                         "easetype","easeOutSine"));
	}
	public void FadeHalfToClear (float fadeTime=1f,float delay=0f)
	{
		gameObject.SetActive (true);
		iTween.ValueTo (gameObject, iTween.Hash ("from",fadeHalfColor,
		                                         "to",Color.clear,
		                                         "time",fadeTime,
		                                         "delay",delay,
		                                         "name","FadeAnim",
		                                         "onstart","OnFadeHalfColorStart",
		                                         "onupdate","UpdateFade",
		                                         "oncomplete","OnFadeColorComplete",
		                                         "ignoretimescale",(Time.timeScale==0f),
		                                         "easetype","easeInSine"));
	}

	void OnFadeColorStart()
	{
		if (OnFadeOutBegin != null)
			OnFadeOutBegin ();
	}
	void OnFadeHalfColorStart()
	{
		if (OnFadeOutBegin != null)
			OnFadeOutBegin ();
	}
	void OnFadeColorComplete()
	{
		if (OnFadeOutCompleted != null)
			OnFadeOutCompleted ();
	}

	void UpdateFade(Color c) {
		GetComponent<Image> ().color = c;
	}

	public void SetFadeColor(ColorFaderType cft)
	{
		Image iFader = GetComponent<Image>();
		if (iFader != null) {
			if (cft == ColorFaderType.Full) {
				iFader.color = fadeColor;
			} else if (cft == ColorFaderType.Half) {
				iFader.color = fadeHalfColor;
			} else {
				iFader.color = Color.clear;
			}
		}
	}

}





