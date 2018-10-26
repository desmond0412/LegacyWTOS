using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoiseRandomizer : MonoBehaviour {

	public List<Texture> noises;


	public void Start()
	{
		Material mat =  this.GetComponent<MeshRenderer>().material;
		mat.SetTexture("_DissolveMap",noises.Random());
	}

	[ContextMenu("Clean Up")]
	private void CleanUp()
	{
		noises.Remove(null);
	}



}
