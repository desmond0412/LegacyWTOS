using UnityEngine;
using System.Collections;

public class MyuuModelBase : MonoBehaviour {
	public delegate void MyuuModelBaseDelegate ();
	public event MyuuModelBaseDelegate OnFinishExit;
	public event MyuuModelBaseDelegate OnFinishIntro;
	public Animator myuuAnim;

	void EventAnimFinishExit()
	{
		if (OnFinishExit != null) 
			OnFinishExit ();
	}
	void EventAnimFinishIntro()
	{
		if (OnFinishIntro != null) 
			OnFinishIntro ();
	}
}
