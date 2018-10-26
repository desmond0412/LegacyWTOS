using UnityEngine;
using System.Collections;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class DialogHandler : MonoBehaviour {

	public delegate void DialogHandlerDelegate(GameObject sender);
	public event DialogHandlerDelegate OnLifetimeEnded;
	public event DialogHandlerDelegate OnObjectDestroyed;

	public DialogSO dialogSO;

	private float textLifetime = -1;
	private float followSpeed = 5;
	private int line = 0;
	private Transform parent;
	private Vector3 offset = Vector3.zero + (Vector3.forward * -2);
	private Vector3 offsetLine = Vector3.zero;
	private Vector3 extraOffset = Vector3.zero;
	private TextMeshPro tmpro;
	private bool isTriggerLifetimeEnded = false;


	public TextMeshPro TextMeshPro
	{
		get {
			if(tmpro == null)
				tmpro = this.GetComponent<TextMeshPro>();
			return tmpro;
		}	
	}


	void Update()
	{
		if(parent == null) return;

		if(this.transform.position != (parent.position + offset + offsetLine + extraOffset))
			this.transform.position = Vector3.Slerp(this.transform.position,parent.position + offset + offsetLine + extraOffset,Time.deltaTime * followSpeed);


		if(!isTriggerLifetimeEnded)
		{
			textLifetime -= Time.deltaTime;
			if(textLifetime <= 0 )
			{
				isTriggerLifetimeEnded = true;	
				if(OnLifetimeEnded!=null)
					OnLifetimeEnded(this.gameObject);
			}
		}


	}

	public void AddLine()
	{
		this.line++;
		offsetLine = line * new Vector3(0,0.5f,0);
	}

	public void SetText(DialogSO dialogSO,Transform parent)
	{
		this.dialogSO 				= dialogSO;
		this.textLifetime 			= dialogSO.lifetime;
		this.TextMeshPro.text 		= dialogSO.GetText();
		this.TextMeshPro.fontSize  *= this.GetFontSize(dialogSO.fontSize);
		this.TextMeshPro.alignment 	= GetTextAlignment(dialogSO.alignment);
		this.extraOffset 			= dialogSO.extraOffset;
		this.offset += this.GetOffset(dialogSO.alignment);
		this.parent = parent;

		Vector3 playerHeight = new Vector3(0,1.0f,0);
		this.transform.position = parent.position + playerHeight;
		this.GetComponent<RectTransform>().pivot = GetPivot(dialogSO.alignment);

		FadeInTransition();
	}

	public void DestroyMe()
	{
		if(OnObjectDestroyed!=null)
			OnObjectDestroyed(this.gameObject);
		Destroy(gameObject);
	}


	public void FadeOutTransition(float duration = -1)
	{
		if(duration == -1)
			duration = dialogSO.fadeOutTime;

		parent = null;

		Vector3 vec = new Vector3(0,0.5f,0);
		iTween.MoveTo(gameObject,
			iTween.Hash("position", transform.position + vec,
				"time",duration,
				"easeType", iTween.EaseType.easeInOutQuad,
				"oncomplete","DestroyMe",
				"oncompletetarget",gameObject
			));
		
	}

	public void FadeInTransition(float duration =-1)
	{
		if(duration == -1)
			duration = dialogSO.fadeInTime;
		
		iTween.ScaleFrom(gameObject,
			iTween.Hash("scale", Vector3.one * 0.01f,
				"time",duration,
				"easeType", iTween.EaseType.easeInOutQuad
			));
		
		
	}



	private float GetFontSize(DialogFontSizeType fontsize)
	{
		float retVal = 1.0f;
		switch (fontsize) {
		case DialogFontSizeType.Smallest  	: retVal = 0.50f; break;
		case DialogFontSizeType.Smaller  	: retVal = 0.75f; break;
		case DialogFontSizeType.Normal 		: retVal = 1.00f; break;
		case DialogFontSizeType.Larger  	: retVal = 1.25f; break;
		case DialogFontSizeType.Largest  	: retVal = 1.50f; break;
		}
		return retVal;
	}

	private Vector2 GetPivot(DialogAlignmentType alignment)
	{
		Vector2 retVal = Vector2.zero;
		switch (alignment) {

		case DialogAlignmentType.BottomLeft 	: retVal = new Vector2(1.0f,1.0f); break;
		case DialogAlignmentType.BottomRight 	: retVal = new Vector2(0.0f,1.0f); break;

		case DialogAlignmentType.TopLeft 		: retVal = new Vector2(1.0f,0.0f); break;
		case DialogAlignmentType.TopCenter 		: retVal = new Vector2(0.5f,0.0f); break;
		case DialogAlignmentType.TopRight 		: retVal = new Vector2(0.0f,0.0f); break;

		default:
			retVal = Vector2.zero;
			break;
		}
		return retVal;
	}



	private TextAlignmentOptions GetTextAlignment(DialogAlignmentType alignment)
	{
		TextAlignmentOptions retVal = TextAlignmentOptions.Center;
		switch (alignment) {

		case DialogAlignmentType.TopLeft 		: 
		case DialogAlignmentType.BottomLeft 	: retVal = TextAlignmentOptions.Right; break;
			
		case DialogAlignmentType.TopRight 		: 
		case DialogAlignmentType.BottomRight 	: retVal = TextAlignmentOptions.Left; break;

		case DialogAlignmentType.TopCenter 		: retVal = TextAlignmentOptions.Center; break;

		default:
			retVal = TextAlignmentOptions.Center;
			break;
		}
		return retVal;

	}


	private Vector3 GetOffset(DialogAlignmentType alignment)
	{
		Vector3 retVal = Vector3.zero;
		switch (alignment) {

		case DialogAlignmentType.BottomLeft 	: retVal = new Vector2(-1.0f,1.0f); break;
		case DialogAlignmentType.BottomRight 	: retVal = new Vector2(1.0f,1.0f); break;
		
		case DialogAlignmentType.TopLeft 		: retVal = new Vector2(-1.0f,1.5f); break;
		case DialogAlignmentType.TopCenter 		: retVal = new Vector2(0.0f,1.5f); break;
		case DialogAlignmentType.TopRight 		: retVal = new Vector2(1.0f,1.5f); break;

		default:
			retVal = Vector2.zero;
			break;
		}
		return retVal;
	}




}
