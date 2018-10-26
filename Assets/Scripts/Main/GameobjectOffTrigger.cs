using UnityEngine;
using System.Collections;

public class GameobjectOffTrigger : MonoBehaviour {


	public GameObject[] objects;

	private bool isTriggered = false;

	void OnTriggerEnter(Collider other)
	{
		if(isTriggered) return;
		if(other.CompareTag("Player"))
		{
			isTriggered = true;	
			foreach (var item in objects) {
				item.SetActive(false);
			}

		}
	}
}
