using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CircularBaseMenu : BaseMenu {
	[Range (0,360)]
	public float startAngle;
	[Range (0,1000)]
	public float radius = 250f;
	[Range (0,500)]
	public float radiusMargin = 10f;
	public Vector2 offset;
	public float animationDelay = 0.1f;
	[Range (0,300)]
	public float circularMargin = 10f;

	public string titleText;
	public GameObject textMenuItemPrefab;
	public List<TextMenuItemData> itemData;
	public List<TextMenuItem> itemList;
	public GameObject circularImage;
	public TextMeshProUGUI titleTextUI;
	public Transform itemGroup;
	public CameraBlurer cameraBlurer;

	public override void InitMenu ()
	{
		base.InitMenu ();
		circularImage.gameObject.SetActive (false);
		titleTextUI.gameObject.SetActive (false);
		itemList = new List<TextMenuItem> ();
		for (int i=0;i<itemData.Count;i++) {
			GameObject g = Instantiate (textMenuItemPrefab);
			g.transform.SetParent(itemGroup);
			g.transform.localPosition = Vector3.zero;
			g.transform.localScale = Vector3.one;
			TextMenuItem t = g.GetComponent<TextMenuItem>().InitMenuItem(itemData[i]);
			itemList.Add(t);
		}
		SetItemAction ();
	}
	public virtual void SetItemAction() 
	{
	}

	public void ShowIntro(bool circularShow, float circularRotation) 
	{
		ShowIntro ();
		SetCircularImage (circularShow, circularRotation);
		CircularUpdate ();
		OnIntroEnd += ItemIntro;
		OnOutroEnd += ItemOutro;
	}
	void ItemIntro() {
		OnIntroEnd -= ItemIntro;
		StartCoroutine (DelayedItemIntro (0.1f));
	}
	IEnumerator DelayedItemIntro(float delay) {
		foreach (TextMenuItem item in itemList) {
			item.SetAccessibility(item.data.accessible);
			yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(delay));
		}
	}

	void ItemOutro()
	{
		foreach (TextMenuItem item in itemList) {
			item.SetAccessibility(false);
		}
	}

	void SetCircularImage(bool circularShow, float circularRotation)
	{
		if (circularShow) {
			circularImage.SetActive(true);
			titleTextUI.gameObject.SetActive(true);
			titleTextUI.text = titleText;
			RectTransform crt = circularImage.GetComponent<RectTransform>();
			crt.anchoredPosition = offset;
			titleTextUI.GetComponent<RectTransform>().anchoredPosition = offset;
			float size = (radius*2) - circularMargin;
			crt.sizeDelta = new Vector2(size,size);
			crt.localRotation = Quaternion.Euler(new Vector3(0f,0f,-circularRotation));
		} else {
			circularImage.SetActive(false);
			titleTextUI.gameObject.SetActive(false);
		}
	}

	public void CircularUpdate()
	{
		for(int i=0;i<itemList.Count;i++) {
			itemList[i].SetPosition(radius,offset,radiusMargin,startAngle);
		}		
	}
}
