using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GroupObjectSingleton<T> : MonoBehaviour where T : MonoBehaviour {

	protected static Dictionary<string,T> _instance = new Dictionary<string,T>();
	public static T shared(string key)
	{
		if(_instance.ContainsKey(key))
			return _instance[key];
		return null;
	}

	protected virtual void OnConstruct(string key)
	{
		if(!_instance.ContainsKey(key))
		{
			OnInit(key);
		}
		else
		{
			OnInstanceExist(key);
		}
	}

	protected virtual void OnInit(string key)
	{
		_instance[key] = this.GetComponent<T>();
	}

	protected virtual void OnInstanceExist(string key)
	{
		Destroy(gameObject);
	}

	public void OnDestruct(string key)
	{
		if(_instance.ContainsKey(key))
			_instance.Remove(key);
	}

	public static void DestoryAllInstance()
	{
		foreach (KeyValuePair<string,T> item in _instance) {
			Destroy(item.Value.gameObject);
		}
		_instance.Clear();
	}



}
