using UnityEngine;
using System.Collections;

public class WaterRippleVFXHandler : MonoBehaviour {

	public GameObject rippleVFXprefab;
	protected GameObject _rippleVFXobject;
	public GameObject RippleVFX{
		get{
			if(_rippleVFXobject == null)
			{
				_rippleVFXobject = Instantiate (rippleVFXprefab) as GameObject;
				_rippleVFXobject.name = this.name + _rippleVFXobject.name;
			}
			return _rippleVFXobject;
		}
	}

	public void Play()
	{
		RippleVFX.transform.position = this.transform.position + (Vector3.down * 0.1f);
		RippleVFX.GetComponent<ParticleSystem>().Play();
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("WaterPool"))
		{
			Play();
		}
	}
}
