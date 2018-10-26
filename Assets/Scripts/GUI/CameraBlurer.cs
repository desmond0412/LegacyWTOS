using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class CameraBlurer : MonoBehaviour {

	public delegate void CameraBlurerDelegate ();
	
	public event CameraBlurerDelegate OnBlurInStart;
	public event CameraBlurerDelegate OnBlurInEnd;
	public event CameraBlurerDelegate OnBlurOutStart;
	public event CameraBlurerDelegate OnBlurOutEnd;

	public BlurOptimized camBlur;
	public BlurOptimized camTrans;
	public Transform canvasBlur;
	public Transform canvasTrans;
	public Transform canvasClear;
	public ColorFader faderBlur;
	public ColorFader faderTrans;

	public float blurInTime = 0.5f;
	public float blurOutTime = 0.25f;

	bool _isBlur;
	Transform itemInAnim;

	public bool isBlur 
	{
		get { return _isBlur; }
	}
	public void BlurIn(Transform itemBlur = null)
	{
		camBlur.enabled = true;
		camTrans.enabled = true;
		camBlur.blurSize = 0f;
		camTrans.blurSize = 0f;
		itemInAnim = itemBlur;
		if (itemBlur != null) {
			itemBlur.SetParent (canvasTrans);
			CanvasGroup cg = itemBlur.GetComponent<CanvasGroup>();
			if (cg!=null)
				cg.blocksRaycasts = false;
		}
		iTween.ValueTo (gameObject, iTween.Hash ("from",0f,
		                                         "to",2f,
		                                         "time",blurInTime,
		                                         "onstart","AnimEventStartBlurIn",
		                                         "onupdate","AnimEventUpdate",
		                                         "oncomplete","AnimEventEndBlurIn",
		                                         "ignoretimescale",(Time.timeScale==0f),
		                                         "easetype","easeOutSine"));
		
		faderTrans.transform.SetAsLastSibling ();
		faderTrans.SetFadeColor (ColorFaderType.Clear);
		faderBlur.SetFadeColor (ColorFaderType.Clear);
		faderTrans.FadeToHalfColor (blurInTime);
		faderTrans.OnFadeOutCompleted += AfterFadeOut;
	}
	public void BlurOut(Transform itemBlur = null)
	{
		itemInAnim = itemBlur;
		camBlur.blurSize = 0f;
		camTrans.blurSize = 2f;
		if (itemBlur!=null)
			itemBlur.SetParent (canvasTrans);
		iTween.ValueTo (gameObject, iTween.Hash ("from",2f,
		                                         "to",0f,
		                                         "time",blurOutTime,
		                                         "onstart","AnimEventStartBlurOut",
		                                         "onupdate","AnimEventUpdate",
		                                         "oncomplete","AnimEventEndBlurOut",
		                                         "ignoretimescale",(Time.timeScale==0f),
		                                         "easetype","easeOutSine"));

		faderTrans.transform.SetAsLastSibling ();
		faderTrans.SetFadeColor (ColorFaderType.Half);
		faderBlur.SetFadeColor (ColorFaderType.Clear);
		faderTrans.FadeToClear (blurOutTime);
	}
	public void ToBlur(Transform itemBlur)
	{
		camBlur.blurSize = 2f;
		camTrans.blurSize = 0f;
		itemInAnim = itemBlur;
		itemBlur.SetParent (canvasTrans);
		CanvasGroup cg = itemBlur.GetComponent<CanvasGroup>();
		if (cg!=null)
			cg.blocksRaycasts = false;
		iTween.ValueTo (gameObject, iTween.Hash ("from",0f,
		                                         "to",2f,
		                                         "time",blurInTime,
		                                         "onstart","AnimEventStartToBlur",
		                                         "onupdate","AnimEventUpdateBlur",
		                                         "oncomplete","AnimEventEndToBlur",
		                                         "ignoretimescale",(Time.timeScale==0f),
		                                         "easetype","easeOutSine"));
		
		faderTrans.transform.SetAsLastSibling ();
		faderBlur.SetFadeColor (ColorFaderType.Half);
		faderBlur.FadeHalfToClear (blurInTime);
		faderBlur.OnFadeOutCompleted += BlurFadeOut;
		faderTrans.SetFadeColor (ColorFaderType.Clear);
		faderTrans.FadeToHalfColor (blurInTime);
		faderTrans.OnFadeOutCompleted += TransFadeOut;
	}
	public void FromBlur(Transform itemBlur)
	{
		camBlur.blurSize = 2f;
		camTrans.blurSize = 0f;
		itemInAnim = itemBlur;
		itemBlur.SetParent (canvasTrans);
		iTween.ValueTo (gameObject, iTween.Hash ("from",2f,
		                                         "to",0f,
		                                         "time",blurOutTime,
		                                         "onstart","AnimEventStartFromBlur",
		                                         "onupdate","AnimEventUpdateBlur",
		                                         "oncomplete","AnimEventEndFromBlur",
		                                         "ignoretimescale",(Time.timeScale==0f),
		                                         "easetype","easeOutSine"));
		
		faderTrans.transform.SetAsLastSibling ();
		faderBlur.SetFadeColor (ColorFaderType.Clear);
		faderBlur.FadeToHalfColor (blurOutTime);
		faderBlur.OnFadeOutCompleted += BlurFadeOut;
		faderTrans.SetFadeColor (ColorFaderType.Half);
		faderTrans.FadeHalfToClear (blurOutTime);
		faderTrans.OnFadeOutCompleted += TransFadeOut;
	}
	public void SetBlur(bool blurState)
	{
		_isBlur = blurState;
		camBlur.enabled = blurState;
		camTrans.enabled = blurState;
		camTrans.blurSize = 0f;
		camBlur.blurSize = (blurState ? 2f : 0f);
	}

//----------------------ANIMATION EVENT-----------------------------------------------
//------------------------------------------------------------------------------------
	void AfterFadeOut()
	{
		faderTrans.OnFadeOutCompleted -= AfterFadeOut;
		faderBlur.SetFadeColor (ColorFaderType.Half);
		faderTrans.SetFadeColor (ColorFaderType.Clear);
	}
	void BlurFadeOut()
	{
		faderBlur.OnFadeOutCompleted -= BlurFadeOut;
		faderBlur.SetFadeColor (ColorFaderType.Half);
	}
	void TransFadeOut()
	{
		faderTrans.OnFadeOutCompleted -= TransFadeOut;
		faderTrans.SetFadeColor (ColorFaderType.Clear);
	}


	void AnimEventUpdate(float newSize)
	{
		camTrans.blurSize = newSize;
	}

	void AnimEventStartBlurIn()
	{
		_isBlur = true;
		if (OnBlurInStart != null)
			OnBlurInStart ();
	}
	void AnimEventEndBlurIn()
	{
		if (itemInAnim!=null)
			itemInAnim.SetParent (canvasBlur);
		faderBlur.transform.SetAsLastSibling ();
		camBlur.blurSize = 2f;
		camTrans.blurSize = 0f;
		if (OnBlurInEnd != null)
			OnBlurInEnd ();
	}
	void AnimEventStartBlurOut()
	{
		camBlur.blurSize = 0f;
		if (OnBlurOutStart != null)
			OnBlurOutStart ();
	}
	void AnimEventEndBlurOut()
	{
		_isBlur = false;
		camBlur.enabled = false;
		camTrans.enabled = false;
		if (itemInAnim != null) {
			itemInAnim.SetParent (canvasClear);
			CanvasGroup cg = itemInAnim.GetComponent<CanvasGroup>();
			if (cg!=null)
				cg.blocksRaycasts = true;
		}
		if (OnBlurOutEnd != null)
			OnBlurOutEnd ();
	}

	void AnimEventUpdateBlur(float newSize)
	{
		camTrans.blurSize = newSize;
		camBlur.blurSize = 2f - newSize;
	}
	
	void AnimEventStartToBlur()
	{
		if (OnBlurInStart != null)
			OnBlurInStart ();
	}
	void AnimEventEndToBlur()
	{
		itemInAnim.SetParent (canvasBlur);
		faderBlur.transform.SetAsLastSibling ();
		camBlur.blurSize = 2f;
		camTrans.blurSize = 0f;
		if (OnBlurInEnd != null)
			OnBlurInEnd ();
	}
	void AnimEventStartFromBlur()
	{
		camBlur.blurSize = 0f;
		camTrans.blurSize = 2f;
		if (OnBlurOutStart != null)
			OnBlurOutStart ();
	}
	void AnimEventEndFromBlur()
	{
		camBlur.blurSize = 2f;
		camTrans.blurSize = 0f;
		itemInAnim.SetParent (canvasClear);
		faderBlur.transform.SetAsLastSibling ();
		CanvasGroup cg = itemInAnim.GetComponent<CanvasGroup>();
		if (cg!=null)
			cg.blocksRaycasts = true;
		if (OnBlurOutEnd != null)
			OnBlurOutEnd ();
	}
}
