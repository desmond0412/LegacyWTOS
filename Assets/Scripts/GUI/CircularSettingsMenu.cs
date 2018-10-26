using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CircularSettingsMenu : CircularBaseMenu {

	public CircularBaseMenu lastMenu;
	public VolumeMenu volumeMenu;
	public CircularControlMenu controlMenu;

	public void ShowIntro(CircularBaseMenu lMenu)
	{
		lastMenu = lMenu;
		ShowIntro (true, 270);
	}

	public override void SetItemAction() 
	{
		base.SetItemAction ();
		itemList [0].OnItemClicked += VolumeClicked;
		itemList [1].OnItemClicked += FullScreenClicked;
		itemList [2].OnItemClicked += BackClicked;
		itemList [3].OnItemClicked += ControlClicked;
		OnEscapeAction += BackClicked;

		if (Screen.fullScreen)
			itemList [1].itemText.text = itemData [1].itemName;
		else
			itemList [1].itemText.text = itemData [1].itemNameAlternate;
	}
	void VolumeClicked()
	{
		cameraBlurer.ToBlur (transform);
		volumeMenu.ShowIntro (true,0);
		if (lastMenu.name == "CircularOpeningMenu")
			GUIMenu.shared ().camera.MoveCamFoV (GUICamZoom.Increase,0.5f,0f,null);
	}
	
	void FullScreenClicked()
	{
		Screen.fullScreen = !Screen.fullScreen;
	}
	
	void BackClicked()
	{
		if (lastMenu.name == "CircularOpeningMenu") {
			lastMenu.cameraBlurer.BlurOut (lastMenu.transform);
			GUIMenu.shared ().camera.MoveCamFoV (GUICamZoom.Decrease,0.25f,0f,null);
		} else {
			lastMenu.cameraBlurer.FromBlur (lastMenu.transform);
		}
		ShowOutro ();
	}
	
	void ControlClicked()
	{
		cameraBlurer.ToBlur (transform);
		controlMenu.ShowIntro (true, 270);
		if (lastMenu.name == "CircularOpeningMenu")
			GUIMenu.shared ().camera.MoveCamFoV (GUICamZoom.Increase,0.5f,0f,null);
	}
}
