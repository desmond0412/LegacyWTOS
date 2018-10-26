using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class VolumeMenu : CircularBaseMenu {

	[Range (0,100)]
	public int volume = 100;
	public CircularSettingsMenu settingsMenu;
	public float volumeMultiplier = 0.1f;
	bool onDrag;
	Vector2 oriMousePos;


	public override void SetItemAction ()
	{
		base.SetItemAction ();
		itemList [0].GetComponent<Button> ().enabled = false;
		itemList [0].GetComponent<EventTrigger> ().enabled = false;
		itemList [0].OnItemPressed += StartDrag;
		itemList [0].OnItemUp += EndDrag;
	
		volume = (int)GameDataManager.shared().CONFIG_Volume;
		AkSoundEngine.SetRTPCValue("MasterVolume",volume);
		SetVolume ();
		OnEscapeAction += EndDrag;

	}

	void SetVolume()
	{
		startAngle = 10f + ((float)volume) * (340f / 100f);
		itemList[0].itemText.text = ""+volume+"%";
		itemList [0].GetComponent<RectTransform>().sizeDelta = new Vector2(160,100);
		AkSoundEngine.SetRTPCValue("MasterVolume",volume);

	}

	public void StartDrag()
	{
		onDrag = true;
		itemList [0].GetComponent<Animator> ().SetBool ("Pressed", true);
		itemList [0].GetComponent<Animator> ().SetBool ("Normal", false);
	}

	public void EndDrag()
	{
		onDrag = false;
		ShowOutro ();
		cameraBlurer.FromBlur (settingsMenu.transform);
		if (settingsMenu.lastMenu.name == "CircularOpeningMenu")
			GUIMenu.shared ().camera.MoveCamFoV (GUICamZoom.Decrease,0.25f,0f,null);
		itemList [0].GetComponent<Animator> ().SetBool ("Pressed", false);
		itemList [0].GetComponent<Animator> ().SetBool ("Normal", true);
		GameDataManager.shared().CONFIG_Volume = volume;
		GameDataManager.shared().Save(GameDataManager.GameDataType.ConfigData);
	}

	public void Drag (BaseEventData data){
		PointerEventData e = data as PointerEventData;
		if (onDrag) {

//			float deltaX = e.pressPosition.x - e.position.x;
			Vector2 posFromCenter = new Vector2(e.position.x - ((Screen.width+offset.x)/2), e.position.y - ((Screen.height+offset.y)/2));
			startAngle = Vector2.Angle(offset,posFromCenter);
			if (posFromCenter.x>0)
				startAngle = -startAngle;
			startAngle += 180f;
			if (startAngle<10f)
				startAngle = 10f;
			if (startAngle>350f)
				startAngle = 350f;
			volume = (int)((startAngle-10f)/340f * 100f);
			SetVolume ();
			CircularUpdate ();
		}
	}
}
