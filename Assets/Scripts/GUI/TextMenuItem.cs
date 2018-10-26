using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum TextMenuItemType {
	Normal,
	Toggle
}

[System.Serializable]
public class TextMenuItemData {
	public TextMenuItemType type;
	public string itemName;
	[Header("For Toggle Only")]
	public string itemNameAlternate;
	public bool alignmentChange = true;
	public bool accessible = true;
}

public class TextMenuItem : MonoBehaviour {
	public delegate void TextMenuItemDelegate ();
	
	public event TextMenuItemDelegate OnItemClicked;
	public event TextMenuItemDelegate OnItemEnter;
	public event TextMenuItemDelegate OnItemExit;
	public event TextMenuItemDelegate OnItemPressed;
	public event TextMenuItemDelegate OnItemUp;

	public TextMenuItemData data;

	public TextMeshProUGUI itemText;
	Button itemButton;
	RectTransform itemRectTransform;
	RectTransform itemTextTransform;
//	Animator itemAnim;

	float radius;
	float radiusMargin;
	float startAngle;
	Vector2 radiusMarginPos;
	Vector2 originalPos;
	Vector2 offset;

	public TextMenuItem InitMenuItem (TextMenuItemData origin) {
		//set data
		data = origin;

		//set components
		itemButton = GetComponent<Button> ();
		itemRectTransform = GetComponent<RectTransform> ();
		itemTextTransform = itemText.GetComponent<RectTransform> ();
//		itemAnim = GetComponent<Animator> ();

		//initialize
		itemText.text = data.itemName;
		itemButton.onClick.RemoveAllListeners ();
		itemButton.onClick.AddListener (() => ButtonClicked ());
		itemButton.interactable = false;

//		print ("Item "+data.itemName+" size: "+itemText.bounds.size);

		return this;
	}

	public void ButtonEntered() {
//		print (data.itemName+" Entered");
		if (itemButton.IsInteractable ()) {
			Vector2 curPos = itemTextTransform.anchoredPosition;
			iTween.ValueTo (gameObject, iTween.Hash ("from",curPos,
			                                         "to",radiusMarginPos,
			                                         "time",0.2f,
			                                         "ignoretimescale",(Time.timeScale==0f),
			                                         "onupdate","UpdatePos",
			                                         "easetype","linear"));
			if (OnItemEnter != null)
				OnItemEnter ();
		}
	}
	public void ButtonExited() {
//		print (data.itemName+" Exited");
		if (itemButton.IsInteractable ()) {
			Vector2 curPos = itemTextTransform.anchoredPosition;
			iTween.ValueTo (gameObject, iTween.Hash ("from",curPos,
			                                         "to",Vector2.zero,
			                                         "time",0.2f,
			                                         "ignoretimescale",(Time.timeScale==0f),
			                                         "onupdate","UpdatePos",
			                                         "easetype","linear"));
			if (OnItemExit != null)
				OnItemExit ();
		}
	}
	void UpdatePos(Vector2 newPos) 
	{
		itemTextTransform.anchoredPosition = newPos;
	}
	public void ButtonPressed() {
		if (itemButton.IsInteractable ()) {
			float curGlow = itemText.fontMaterial.GetFloat("_GlowPower");
			iTween.ValueTo (gameObject, iTween.Hash ("from",curGlow,
			                                         "to",1f,
			                                         "time",0.2f,
			                                         "ignoretimescale",(Time.timeScale==0f),
			                                         "onupdate","UpdateGlow",
			                                         "easetype","linear"));
			if (OnItemPressed != null)
				OnItemPressed ();
		}
	}
	public void ButtonUp() {
		if (itemButton.IsInteractable ()) {
			float curGlow = itemText.fontMaterial.GetFloat("_GlowPower");
			iTween.ValueTo (gameObject, iTween.Hash ("from",curGlow,
			                                         "to",0f,
			                                         "time",0.2f,
			                                         "ignoretimescale",(Time.timeScale==0f),
			                                         "onupdate","UpdateGlow",
			                                         "easetype","linear"));
			if (OnItemUp != null)
				OnItemUp ();
		}
	}
	void UpdateGlow(float newGlow) 
	{
		itemText.fontMaterial.SetFloat ("_GlowPower",newGlow);
	}

	void ButtonClicked()
	{
//		print (data.itemName+" Clicked");
		if (data.type == TextMenuItemType.Toggle) {
			itemText.text = (itemText.text == data.itemName) ? data.itemNameAlternate : data.itemName;
		}
		if (OnItemClicked != null)
			OnItemClicked ();
	}
	public void SetPosition(float tRadius, Vector2 tOffset, float tRadiusMargin, float tStartAngle = 0f)
	{
		radius = tRadius;
		offset = tOffset;
		radiusMargin = tRadiusMargin;
		startAngle = tStartAngle;
		CalculatePosition ();
	}

	void CalculatePosition()
	{
		int childcount = transform.parent.childCount;
		int idx = transform.GetSiblingIndex ();
		float slice = 2 * Mathf.PI/ childcount;
		float perSlice = slice * idx;
		
		perSlice += startAngle * Mathf.Deg2Rad;

		float x = radius * Mathf.Sin(perSlice);
		float y = radius * Mathf.Cos(perSlice);
		originalPos = new Vector2(x+offset.x,y+offset.y);
		x = (0+radiusMargin) * Mathf.Sin(perSlice);
		y = (0+radiusMargin) * Mathf.Cos(perSlice);
		radiusMarginPos = new Vector2(x,y);
//		print ("Item "+data.itemName+" Radius Margin Pos: "+radiusMarginPos);

		float pivotX = (-(Mathf.Sin(perSlice))+1f)/2f;
		float pivotY = (-(Mathf.Cos(perSlice))+1f)/2f;
		Vector2 newPivot = new Vector2(pivotX,pivotY);
		
		itemRectTransform.anchoredPosition = originalPos;
		itemRectTransform.pivot = newPivot;
		if (data.alignmentChange) {
			if (newPivot.x < 0.35f)
				itemText.alignment = TextAlignmentOptions.Left;
			else if (newPivot.x > 0.65f)
				itemText.alignment = TextAlignmentOptions.Right;
			else
				itemText.alignment = TextAlignmentOptions.Center;
		} else {
			itemText.alignment = TextAlignmentOptions.Center;
		}
	}

	public void SetAccessibility(bool access){
		itemButton.interactable = access;
	}
}
