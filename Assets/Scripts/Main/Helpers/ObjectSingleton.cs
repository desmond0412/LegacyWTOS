using UnityEngine;
using System.Collections;


public class ObjectSingleton<T> : MonoBehaviour where T : MonoBehaviour {

	private static T _instance;
	public static T shared()
	{
		return _instance;
	}

	void Awake()
	{
		if (_instance == null) {
			OnInit ();
		}
		else
		{
			OnInstanceExist();
		}
	}
	protected virtual void OnInit()
	{
		_instance = this.GetComponent<T>();
	}

	protected virtual void OnInstanceExist()
	{
		Destroy(gameObject);
	}

}
