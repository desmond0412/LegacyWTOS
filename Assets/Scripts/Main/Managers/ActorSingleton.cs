using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum ActorType
{
	Lev = 0,
	Raina = 1,
}	

public class ActorSingleton : GroupObjectSingleton<ActorSingleton>
{
	public ActorType actor;
	public GameObject dialogPrefab;
	private List<GameObject> dialogContainer;

	public static ActorSingleton shared(ActorType type)
	{
		return shared(type.ToString());
	}

	void Awake()
	{
		OnConstruct(actor.ToString());
		dialogContainer = new List<GameObject>();
	}
	void OnDestroy()
	{
		OnDestruct(actor.ToString());
	}

	public void Talk(DialogSO dialog)
	{
		AddLineToPreviousText();
		dialogContainer.Add(CreateDialogText(dialog));
	}


	void AddLineToPreviousText()
	{
		foreach (var item in dialogContainer) {
			item.GetComponent<DialogHandler>().AddLine();
		}
	}

	GameObject CreateDialogText(DialogSO dialogSO)
	{
		GameObject obj = Instantiate(dialogPrefab) as GameObject;
		obj.GetComponent<DialogHandler>().SetText(dialogSO,transform);
		obj.GetComponent<DialogHandler>().OnLifetimeEnded += OnDialogHandlerLifetimeEnded;

		return obj;
	}


	void OnDialogHandlerLifetimeEnded (GameObject sender)
	{
		DialogHandler handler = sender.GetComponent<DialogHandler>();
		DialogSO nextDialog = handler.dialogSO.nextDialog;


		if(!handler.dialogSO.isWithNext) // not in the same line
		{
			foreach (var item in dialogContainer) {
				item.GetComponent<DialogHandler>().FadeOutTransition();
			}
			dialogContainer.Clear();				
		}

		if(nextDialog != null)
		{
			//still on the same character talk
			if(nextDialog.actor == actor)
				Talk(nextDialog);
			else
				handler.OnObjectDestroyed += OnDialogHandlerObjectDestroyed;
		}
	}

	void OnDialogHandlerObjectDestroyed (GameObject sender)
	{
		DialogHandler handler = sender.GetComponent<DialogHandler>();
		DialogSO nextDialog = handler.dialogSO.nextDialog;

		if(nextDialog != null)
		{
			ActorSingleton.shared(nextDialog.actor.ToString()).Talk(nextDialog);
		}
	}





}
