using UnityEngine;
using System.Collections;

public class LightTrigger : MonoBehaviour {

	public GameObject[] lights;

	void Awake()
	{
		#if UNITY_EDITOR
		if(lights.Length == 0)
			Debug.LogError("LIGHT TRIGGER NEEDS LIGHT TO BE SET UP");
		#endif
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
//				print("light trigger activated");
			foreach (var item in lights) {
				if(item != null)
					item.GetComponent<DirectionalLightSingleton>().ActivateLight();
			}
		}
	}
}
