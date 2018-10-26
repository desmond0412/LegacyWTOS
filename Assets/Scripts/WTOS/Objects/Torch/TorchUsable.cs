using UnityEngine;
using System.Collections;

public class TorchUsable : UsableObject {

	Coroutine useCoroutine;

	SphereCollider col;
	void Awake(){
		col = GetComponent<SphereCollider> ();
	}

	public override void UseFunction ()
	{
		base.UseFunction ();
		if (useCoroutine == null) {
			useCoroutine = StartCoroutine (UseTorch ());
		}
	}
	IEnumerator UseTorch(){
		col.enabled = true;
		yield return new WaitForSeconds (.4f);
		col.enabled = false;
		useCoroutine = null;
	}
}
