using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class BaseMenu : MonoBehaviour {
	public delegate void BaseMenuDelegate ();
	
	public event BaseMenuDelegate OnIntroStart;
	public event BaseMenuDelegate OnIntroEnd;
	public event BaseMenuDelegate OnOutroStart;
	public event BaseMenuDelegate OnOutroEnd;
	public event BaseMenuDelegate OnEscapeAction;

	public Vector3 outScale = Vector3.one;
	public Vector3 firstScaleDelta = new Vector3(0.2f,0.2f,0f);
	public Vector3 stackScaleDelta = new Vector3(0.1f,0.1f,0f);
	public float introTime = 0.5f;
	public float outroTime = 0.25f;
//	protected Animator menuAnim;

	void Start()
	{
		InitMenu ();
	}

//	bool isMenuIdle {
//		get {
//			return menuAnim.GetCurrentAnimatorStateInfo(0).IsName("Normal");
//		}
//	}
	bool isMenuIdle {
		get {
			return ((transform.localScale == Vector3.one) && (gameObject.activeSelf));
		}
	}	

	public void EscapeAction() {
		if (isMenuIdle) {
			if (OnEscapeAction!=null)
				OnEscapeAction();
		}
	}

	public virtual void InitMenu()
	{
		gameObject.SetActive (false);
//		menuAnim = GetComponent<Animator> ();
	}
	public void ShowIntro() 
	{
		GetComponent<CanvasGroup> ().alpha = 0;
		gameObject.SetActive (true);
//		menuAnim.SetTrigger ("Intro");
		transform.localScale = Vector3.one;
		iTween.ScaleFrom (gameObject, iTween.Hash("scale",outScale,
		                                          "time",introTime,
		                                          "onstart","AnimEventIntroStart",
		                                          "oncomplete","AnimEventIntroEnd",
		                                          "ignoretimescale",(Time.timeScale==0f),
		                                          "easetype","easeInOutSmoothBreak"));
		iTween.ValueTo (gameObject, iTween.Hash("from",0f,
		                                        "to",1f,
		                                        "time",introTime,
		                                        "onupdate","AnimUpdate",
		                                        "ignoretimescale",(Time.timeScale==0f),
		                                        "easetype","easeInOutCubic"));
	}
	public void ShowOutro()
	{
//		menuAnim.SetTrigger ("Outro");
		transform.localScale = Vector3.one;
		GetComponent<CanvasGroup> ().alpha = 1;
		iTween.ScaleTo (gameObject, iTween.Hash("scale",outScale,
		                                        "time",outroTime,
		                                        "onstart","AnimEventOutroStart",
		                                        "oncomplete","AnimEventOutroEnd",
		                                        "ignoretimescale",(Time.timeScale==0f),
		                                        "easetype","easeInOutSmoothBreak"));
		iTween.ValueTo (gameObject, iTween.Hash("from",1f,
		                                        "to",0f,
		                                        "time",outroTime,
		                                        "onupdate","AnimUpdate",
		                                        "ignoretimescale",(Time.timeScale==0f),
		                                        "easetype","easeInOutCubic"));
	}

//----------------------ANIMATION EVENT-----------------------------------------------
//------------------------------------------------------------------------------------

	void AnimUpdate(float newV)
	{
		GetComponent<CanvasGroup> ().alpha = newV;
	}

	void AnimEventIntroStart()
	{
		for (int i=GUIMenu.shared ().menuStack.Count-1;i>=0;i--) {
			BaseMenu b = GUIMenu.shared ().menuStack[i];
			Vector3 targetScale = Vector3.one;
			if (i==GUIMenu.shared ().menuStack.Count-1) {
				targetScale = b.transform.localScale - b.firstScaleDelta;
			} else {
				targetScale = b.transform.localScale - b.stackScaleDelta;
			}
			iTween.ScaleTo (b.gameObject, iTween.Hash("scale",targetScale,
			                                          "time",introTime,
			                                          "ignoretimescale",(Time.timeScale==0f),
			                                          "easetype","easeInOutSmoothBreak"));
			float currentAlpha = b.GetComponent<CanvasGroup>().alpha;
			float targetAlpha = currentAlpha / 2;
			iTween.ValueTo (b.gameObject, iTween.Hash("from",currentAlpha,
			                                          "to",targetAlpha,
			                                          "time",introTime,
			                                          "onupdate","AnimUpdate",
			                                          "onupdatetarget",b.gameObject,
			                                          "ignoretimescale",(Time.timeScale==0f),
			                                          "easetype","easeInOutSine"));

		}
		if (OnIntroStart != null)
			OnIntroStart ();
	}


	void AnimEventIntroEnd()
	{
		GUIMenu.shared ().menuStack.Add (this);
		if (OnIntroEnd != null)
			OnIntroEnd ();
	}
	void AnimEventOutroStart()
	{
		for (int i=GUIMenu.shared ().menuStack.Count-2;i>=0;i--) {
			BaseMenu b = GUIMenu.shared ().menuStack[i];
			Vector3 targetScale = Vector3.one;
			if (i==GUIMenu.shared ().menuStack.Count-2) {
				targetScale = b.transform.localScale + b.firstScaleDelta;
			} else {
				targetScale = b.transform.localScale + b.stackScaleDelta;
			}
			iTween.ScaleTo (b.gameObject, iTween.Hash("scale",targetScale,
			                                          "time",outroTime,
			                                          "ignoretimescale",(Time.timeScale==0f),
			                                          "easetype","easeInOutSmoothBreak"));
			float currentAlpha = b.GetComponent<CanvasGroup>().alpha;
			float targetAlpha = currentAlpha * 2;
			iTween.ValueTo (b.gameObject, iTween.Hash("from",currentAlpha,
			                                          "to",targetAlpha,
			                                          "time",outroTime,
			                                          "onupdate","AnimUpdate",
			                                          "onupdatetarget",b.gameObject,
			                                          "ignoretimescale",(Time.timeScale==0f),
			                                          "easetype","easeInOutSine"));
			

		}
		if (OnOutroStart != null)
			OnOutroStart ();
	}
	void AnimEventOutroEnd()
	{
		GUIMenu.shared ().menuStack.Remove (this);
		gameObject.SetActive (false);
		if (OnOutroEnd != null)
			OnOutroEnd ();
	}
}
