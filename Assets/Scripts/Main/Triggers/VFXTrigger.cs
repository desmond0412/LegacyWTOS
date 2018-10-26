using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum VFXTriggerType
{
	//LEV ANIMATION VFX 0 - 100
	Lev_Walk = 0,
	Lev_Landing = 1,

	//SET OTHER VFX ABOVE 100


}


[System.Serializable]
public struct VFXSet
{
	public VFXTriggerType key;
	public GameObject vfxObject;
}
[RequireComponent(typeof(Collider))]
public class VFXTrigger : MonoBehaviour {

	public VFXSet[] Vfxs;
	private Dictionary<VFXTriggerType,GameObject> VFXObjects;

	void Awake()
	{
		//reassign to dictionary
		VFXObjects = new Dictionary<VFXTriggerType, GameObject>();
		foreach (VFXSet item in Vfxs) {
			VFXObjects.Add(item.key,item.vfxObject);
		}
//		VFXObjects.Keys.ForEach((x)=>{print(x);});
	}


	void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<VFXTriggerHandler>())
		{
			foreach (var item in VFXObjects) {
				other.GetComponent<VFXTriggerHandler>().AssignVFX(item.Key,item.Value);
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.GetComponent<VFXTriggerHandler>())
		{
			foreach (var item in VFXObjects) {
				other.GetComponent<VFXTriggerHandler>().RevokeVFX(item.Key);
			}
		}
	}










}
