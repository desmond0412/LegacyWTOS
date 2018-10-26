using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum MainObjectType
{
	Player 		= 	0,
	Camera 		= 	1,
	Manager 	= 	2,
	EventSystem = 	3,
	SceneFader	= 	4,
	WwiseGlobal = 	5,
}

public class MainObjectSingleton : GroupObjectSingleton<MainObjectSingleton>
{
	public MainObjectType type;

	public static MainObjectSingleton shared(MainObjectType type)
	{
		return shared(type.ToString());
	}

	void Awake()
	{
		OnConstruct(type.ToString());
	}
	void OnDestroy()
	{
		OnDestruct(type.ToString());
	}

	protected override void OnInit (string key)
	{
		base.OnInit (key);
		if(key != MainObjectType.EventSystem.ToString())
			DontDestroyOnLoad(gameObject);
	}

	protected override void OnInstanceExist (string key)
	{	
		if(key == MainObjectType.EventSystem.ToString())
		{
			OnDestruct(key);
			Destroy(gameObject);
		}
		else
			this.gameObject.SetActive(false);
	}



}
