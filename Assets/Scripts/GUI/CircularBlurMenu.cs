using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CircularBlurMenu : CircularBaseMenu {
	public delegate void CircularBlurMenuDelegate ();
	
//	public event CircularBlurMenuDelegate OnToBlurStart;
//	public event CircularBlurMenuDelegate OnToBlurEnd;
//	public event CircularBlurMenuDelegate OnFromBlurStart;
//	public event CircularBlurMenuDelegate OnFromBlurEnd;
//
//
//	public CameraBlurer cameraBlurer;
//
//	public void ToBlur(bool changeBlur, bool moveMainCam=false) 
//	{
//		GetComponent<CanvasGroup> ().blocksRaycasts = false;
//		if ((changeBlur) && (!cameraBlurer.isBlur)) {
//			cameraBlurer.BlurIn (transform);
//		}
//		if (moveMainCam) {
//			GUIMenu.shared().camera.MoveCamFoV(GUICamZoom.Out,0.25f,0f,null);
//		}
//	} 
//
//	public void FromBlur(bool changeBlur,bool moveMainCam=false)
//	{
//		if ((changeBlur) && (cameraBlurer.isBlur)) {
//			cameraBlurer.BlurOut (transform);
//		}
//		if (moveMainCam) {
//			GUIMenu.shared().camera.MoveCamFoV(GUICamZoom.In,0.25f,0f,null);
//		}
//	}
//	void UpdateCamFoV(float newFoV)
//	{
//		Camera.main.fieldOfView = newFoV;
//	}

//----------------------ANIMATION EVENT-----------------------------------------------
//------------------------------------------------------------------------------------
	
//	void AnimEventToBehindStart()
//	{
//		if (OnToBlurStart != null)
//			OnToBlurStart ();
//	}
//	void AnimEventToBehindEnd()
//	{
//		if (OnToBlurEnd != null)
//			OnToBlurEnd ();
//	}
//	void AnimEventFromBehindStart()
//	{
//		if (OnFromBlurStart != null)
//			OnFromBlurStart ();
//	}
//	void AnimEventFromBehindEnd()
//	{
//		GetComponent<CanvasGroup> ().blocksRaycasts = true;
//		transform.SetParent (canvasClear, true);
//		if (OnFromBlurEnd != null)
//			OnFromBlurEnd ();
//	}
}
