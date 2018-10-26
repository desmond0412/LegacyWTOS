using UnityEngine;
using System.Collections;
using Artoncode.Core;	

public class GUICamDemo : GUICam {

	void Start()
	{
		deltaCamPos 		= new Vector3(0,13.0f,0);
		deltaCamPosFromLev 	= new Vector3(0,1.15f,-12.11f);
		zoomInFoV 			= 20;
		SaveCamPos();
		SaveCamFoV();
	}

	void SetCameraFollow(bool follow)
	{
		MainObjectSingleton.shared(MainObjectType.Camera).GetComponent<SmoothFollow>().enabled = follow;
	}

	void OnGUI()
	{
		if(GUILayout.Button("PLAY TOP TO MID"))
		{
			SetCameraFollow(false);
			SetCamPos(GUICamPos.Up);
			SetCamFoV(GUICamZoom.In);
			MoveCamPos(GUICamPos.Center,1,1,null);

		}

		if(GUILayout.Button("PLAY MID TO TOP"))
		{
			SetCameraFollow(false);
			SetCamPos(GUICamPos.Center);
			SetCamFoV(GUICamZoom.In);
			MoveCamPos(GUICamPos.Up,1,1,null);

		}

		if(GUILayout.Button("PLAY MID TO BOT"))
		{
			SetCameraFollow(false);
			SetCamPos(GUICamPos.Center);
			SetCamFoV(GUICamZoom.In);
			MoveCamPos(GUICamPos.Down,1,1,null);
		}

		if(GUILayout.Button("RESET"))
		{
			SetCameraFollow(true);
			SetCamPos(GUICamPos.Original);
			SetCamFoV(GUICamZoom.Original);

		}
	}


}
