using UnityEngine;
using System.Collections;

public enum DialogAlignmentType{

	Floating,
	TopLeft,
	TopCenter,
	TopRight,
	BottomLeft,
	BottomRight,
}


public enum DialogFontSizeType
{
	Smallest,
	Smaller,
	Normal,
	Larger,
	Largest,
}

public class DialogSO : ScriptableObject {

	public string objectID;
	public ActorType actor;
	public float lifetime = 1;
	public float fadeInTime = 1;
	public float fadeOutTime = 1;
	public Vector2 extraOffset = Vector2.zero;

	public DialogAlignmentType alignment;
	public DialogFontSizeType fontSize = DialogFontSizeType.Normal;

	public bool isWithNext;
	public bool isTriggerAnimation;
	public string animationName;

	public DialogSO nextDialog;


	public string GetText()
	{
		return SmartLocalization.LanguageManager.Instance.GetTextValue(objectID);
	}

	public void SetActor(string actorName)
	{
		string actorLower = actorName.ToLower();

		switch (actorLower) {
		case "lev" 		: actor = ActorType.Lev; break;
		case "raina"	: actor = ActorType.Raina; break;
		}
	}

}
