using UnityEngine;
using System.Collections;

public class DialogController : MonoBehaviour {

	public DialogSO testSo;

	void OnGUI()
	{
		if(GUILayout.Button("talk"))
		{
			ActorSingleton.shared(testSo.actor.ToString()).Talk(testSo);
//			Talk(testSo);
		}


	}
}
