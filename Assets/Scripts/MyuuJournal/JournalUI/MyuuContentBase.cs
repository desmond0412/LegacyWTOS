using UnityEngine;
using System.Collections;

public class MyuuContentBase : MonoBehaviour {
	public delegate void ContentDelegate ();
	public event ContentDelegate OnSceneStart;
	public event ContentDelegate OnSceneEnd;

	// Use this for initialization
	void Start () {
		if (OnSceneStart != null)
			OnSceneStart ();
		if(GUIMenu.shared () !=null)
		{
			GUIMenu.shared ().isIdle = true;
			GUIMenu.shared ().myuuMenu = true;
		}
	}	

	void OnDestroy()
	{
		if(GUIMenu.shared () !=null)
		{
			GUIMenu.shared ().isIdle = false;	
			GUIMenu.shared ().myuuMenu = false;
		}
		if (OnSceneEnd != null)
			OnSceneEnd ();
	}

}
