using UnityEngine;
using System.Collections;
using ScionEngine;

public enum GUICamZoom {
	Original,
	In,
	Decrease,
	Increase
}

public enum GUICamPos {
	Original,
	Center,
	Up,
	Down
}

public class GUICam : MonoBehaviour {

	public Vector3 deltaCamPos;
	public Vector3 deltaCamPosFromLev;
	public float zoomInFoV; 
	public float deltaFoV = 5f; 

	float oriFoV = 60f;
	float oriFocal = 85f;
	Vector3 oriCamPos = Vector3.zero;
	Quaternion oriCamRotate = Quaternion.identity;
	System.Action onCamMoveComplete;
	System.Action onCamFoVComplete;


	Camera mainCam {
		get {
			return Camera.main;
		}
	}

	public void SaveCamPos()
	{
		oriCamPos = mainCam.transform.position;
		oriCamRotate = mainCam.transform.rotation;
	}
	public void SetCamPos(GUICamPos pos) {
		Vector3 levPos = MainObjectSingleton.shared (MainObjectType.Player).transform.position;
		mainCam.transform.rotation = Quaternion.identity;
		if (pos == GUICamPos.Original) {
			mainCam.transform.position = oriCamPos;
			mainCam.transform.rotation = oriCamRotate;
		} else if (pos == GUICamPos.Up)
			mainCam.transform.position = levPos + deltaCamPosFromLev + deltaCamPos;
		else if (pos == GUICamPos.Down)
			mainCam.transform.position = levPos + deltaCamPosFromLev - deltaCamPos;
		else 
			mainCam.transform.position =  levPos + deltaCamPosFromLev;
	}

	public void MoveCamPos(GUICamPos pos,float camTime, float camDelay, System.Action camCompleteAction) {
		Vector3 endCamPos = Vector3.zero;
		Vector3 levPos = MainObjectSingleton.shared (MainObjectType.Player).transform.position;
		Quaternion endCamRotate = Quaternion.identity;
		onCamMoveComplete = camCompleteAction;
		if (pos == GUICamPos.Original) {
			endCamPos = oriCamPos;
			endCamRotate = oriCamRotate;
		} else if (pos == GUICamPos.Up)
			endCamPos = levPos + deltaCamPosFromLev + deltaCamPos;
		else if (pos == GUICamPos.Down)
			endCamPos = levPos + deltaCamPosFromLev - deltaCamPos;
		else 
			endCamPos = levPos + deltaCamPosFromLev;
		iTween.MoveTo (mainCam.gameObject, iTween.Hash ("position",endCamPos,
					                                     "time",camTime,
					                                     "delay",camDelay,
					                                     "oncomplete","OnCamMoveComplete",
					                                     "oncompletetarget",gameObject,
		                                                "ignoretimescale",(Time.timeScale==0f),
		                                                "easetype","easeInOutSmoothBreak"));	
		iTween.RotateTo (mainCam.gameObject, iTween.Hash ("rotation",endCamRotate.eulerAngles,
		                                                "time",camTime,
		                                                "delay",camDelay,
		                                                "ignoretimescale",(Time.timeScale==0f),
		                                                  "easetype","easeInOutSmoothBreak"));	
	}
	void OnCamMoveComplete()
	{
		if (onCamMoveComplete != null) 
			onCamMoveComplete ();
		onCamMoveComplete = null;
	}

	public void SaveCamFoV()
	{
		oriFoV = mainCam.fieldOfView;
		ScionPostProcessNoTonemap spp = mainCam.GetComponent<ScionPostProcessNoTonemap> ();
		if (spp!=null)
			oriFocal = spp.focalLength;
	}
	public void SetCamFoV(GUICamZoom zoom) {
		ScionPostProcessNoTonemap spp = mainCam.GetComponent<ScionPostProcessNoTonemap> ();
		if (zoom == GUICamZoom.Original) {
			mainCam.fieldOfView = oriFoV;
			if (spp!=null)
				spp.focalLength = oriFocal;
		} else if (zoom == GUICamZoom.In) {
			mainCam.fieldOfView = zoomInFoV;
			if (spp!=null)
				mainCam.GetComponent<ScionPostProcessNoTonemap> ().focalLength = 0;
		} 
	}
	public void MoveCamFoV(GUICamZoom pos,float camTime, float camDelay, System.Action camCompleteAction) {
		ScionPostProcessNoTonemap spp = mainCam.GetComponent<ScionPostProcessNoTonemap> ();
		float endCamFoV = 0f;
		float oriCamFoV = mainCam.fieldOfView;
		float oriCamFocal = 85f;
		float endCamFocal = 0f;

		if (spp != null)
			oriCamFocal = spp.focalLength;

		onCamFoVComplete = camCompleteAction;
		if (pos == GUICamZoom.Original) {
			endCamFoV = oriFoV;
			endCamFocal = oriFocal;
		} else if (pos == GUICamZoom.In) {
			endCamFoV = zoomInFoV;
			endCamFocal = 0f;
		} else if (pos == GUICamZoom.Increase) {
			endCamFoV = mainCam.fieldOfView + deltaFoV;
			endCamFocal = 0f;
		}  else if (pos == GUICamZoom.Decrease) {
			endCamFoV = mainCam.fieldOfView - deltaFoV;
			endCamFocal = 0f;
		}
		iTween.ValueTo (gameObject, iTween.Hash ("from",oriCamFoV,
                                                 "to",endCamFoV,
                                                "time",camTime,
                                                 "delay",camDelay,
                                                 "onupdate","OnCamFoVUpdate",
                                                 "oncomplete","OnCamFoVComplete",
                                                "oncompletetarget",gameObject,
		                                         "ignoretimescale",(Time.timeScale==0f),
		                                         "easetype","easeInOutSmoothBreak"));	
		if (spp != null) {
			iTween.ValueTo (gameObject, iTween.Hash ("from", oriCamFocal,
			                                         "to", endCamFocal,
			                                         "time", camTime,
			                                         "delay", camDelay,
			                                         "onupdate", "OnCamFocalUpdate",
			                                         "ignoretimescale", (Time.timeScale == 0f),
			                                         "easetype", "easeInOutSmoothBreak"));	
		}
	}
	void OnCamFoVComplete()
	{
		if (onCamFoVComplete != null) 
			onCamFoVComplete ();
		onCamFoVComplete = null;
	}
	void OnCamFoVUpdate(float newFoV)
	{
		mainCam.fieldOfView = newFoV;
	}
	void OnCamFocalUpdate(float newFocal)
	{
		ScionPostProcessNoTonemap spp = mainCam.GetComponent<ScionPostProcessNoTonemap> ();
		if (spp != null)
			spp.focalLength = newFocal;
	}

}
