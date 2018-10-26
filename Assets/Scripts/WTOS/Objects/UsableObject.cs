using UnityEngine;
using System.Collections;

public abstract class UsableObject : MonoBehaviour {

	public delegate void UsableObjectDelegate (GameObject sender);
	public event UsableObjectDelegate OnObjectUsed;

	public bool isAccessible = true;	
	public enum LayerObject {
		Back,
		Mid,
		Front
	}
	public LayerObject objectLayer = LayerObject.Mid;

	public string useAnimation;
	public Transform useHandle;

	virtual public void UseFunction(){
		if(OnObjectUsed !=null)
			OnObjectUsed(this.gameObject);
	}
}
