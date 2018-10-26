using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CircularControlMenu : CircularBaseMenu {

	public CircularSettingsMenu settingsMenu;
	public BaseMenu keyboardMenu;
	
	public override void SetItemAction() 
	{
		base.SetItemAction ();
		itemList [0].OnItemClicked += KeyboardClicked;
		itemList [1].OnItemClicked += ControllerClicked;
		itemList [2].OnItemClicked += BackClicked;
		itemList [3].OnItemClicked += SteamClicked;
		OnEscapeAction += BackClicked;

		keyboardMenu.OnEscapeAction += keyboardMenu.ShowOutro;
	}

	void KeyboardClicked()
	{
		cameraBlurer.ToBlur(transform);
		keyboardMenu.ShowIntro ();
		keyboardMenu.OnOutroStart += MenuExited;
		if (settingsMenu.lastMenu.name == "CircularOpeningMenu")
			GUIMenu.shared ().camera.MoveCamFoV (GUICamZoom.Increase,0.5f,0f,null);
	}
	
	void ControllerClicked()
	{
//		ToBlur (false);
	}
	
	void BackClicked()
	{
		cameraBlurer.FromBlur (settingsMenu.transform);
		if (settingsMenu.lastMenu.name == "CircularOpeningMenu")
			GUIMenu.shared ().camera.MoveCamFoV (GUICamZoom.Decrease,0.25f,0f,null);
		ShowOutro ();
	}
	
	void SteamClicked()
	{
//		ToBlur (false);
	}

	void MenuExited()
	{
		keyboardMenu.OnOutroStart -= MenuExited;
		if (settingsMenu.lastMenu.name == "CircularOpeningMenu")
			GUIMenu.shared ().camera.MoveCamFoV (GUICamZoom.Decrease,0.25f,0f,null);
		cameraBlurer.FromBlur (transform);
	}
}
