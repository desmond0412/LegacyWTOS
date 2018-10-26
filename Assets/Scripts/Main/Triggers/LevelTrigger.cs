using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class LevelTrigger : MonoBehaviour {

	public List<string> activeLevel;
	private bool isTriggered = false;

	void OnTriggerEnter(Collider other)
	{
		if(isTriggered) return;
		if(other.CompareTag("Player"))
		{
//			isTriggered = true;
//			LevelController.ActivateLevel(activeLevel);
		}
	}
}
