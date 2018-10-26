using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VFXTriggerHandler : MonoBehaviour {

	private Dictionary<VFXTriggerType,GameObject> VFXObjects;
	void Awake()
	{
		VFXObjects = new Dictionary<VFXTriggerType, GameObject>();
	}

	public void AssignVFX(VFXTriggerType key, GameObject vfx)
	{
		if(VFXObjects.ContainsKey(key))return;
		VFXObjects.Add(key,vfx);
	}

	public void RevokeVFX(VFXTriggerType key)
	{
		if(!VFXObjects.ContainsKey(key)) return;
		VFXObjects.Remove(key);
	}

	public GameObject GetVFX(VFXTriggerType key)
	{
		if(!VFXObjects.ContainsKey(key)) return null;
		return VFXObjects[key];
	}

	public void PlayVFX(VFXTriggerType key)
	{
		PlayVFX(key,transform);
	}

	public void PlayVFX(VFXTriggerType key, Transform transform)
	{
		if(!VFXObjects.ContainsKey(key)) return;


		GameObject vfx = Instantiate(VFXObjects[key],transform.position,transform.rotation) as GameObject;
		Destroy(vfx,vfx.GetComponent<ParticleSystem>().duration);
	}
}
