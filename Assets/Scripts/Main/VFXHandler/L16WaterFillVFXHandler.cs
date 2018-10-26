using UnityEngine;
using System.Collections;

public class L16WaterFillVFXHandler : MonoBehaviour {

	public WaterContainerHeight waterContainer;
	public GameObject waterSource;
	public ParticleSystem[] interactableParticleGO;
	public bool isOnToOff;
	public bool isOffToOn;

	bool isTriggered = false;

	void Awake()
	{
		waterContainer.OnFillWaterUpdate += WaterContainer_OnFillWaterUpdate;
		waterContainer.OnFillWaterEnd += WaterContainer_OnFillWaterEnd;

		foreach (var item in interactableParticleGO) {
			if(isOnToOff)	
				item.gameObject.SetActive(true);
			else if(isOffToOn)	
				item.gameObject.SetActive(false);
		}



	}

	void WaterContainer_OnFillWaterEnd (GameObject sender)
	{
		this.transform.position = sender.transform.position;	
	}

	void WaterContainer_OnFillWaterUpdate (GameObject sender)
	{
		this.transform.position = sender.transform.position;
	}

	void OnTriggerEnter(Collider other)
	{
		if(isTriggered) return;
		if(interactableParticleGO == null) return;

		if(other.gameObject == waterSource)
		{
//			float dist = Mathf.Abs(other.transform.position.y - this.transform.position.y);
//			if(dist <=0.1f)
			{
				isTriggered = true;
				foreach (var item in interactableParticleGO) {
					if(isOnToOff)	
					{
//						item.gameObject.SetActive(false);
//						item.
//						item.emission.rate =  0;
						ParticleSystem.EmissionModule emission = item.emission;
						emission.rate = new ParticleSystem.MinMaxCurve(0);
					}
					else if(isOffToOn)	
					{
						item.gameObject.SetActive(true);
					}
						
				}
			}
		}
		
	}

}
