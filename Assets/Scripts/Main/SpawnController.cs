using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Artoncode.Core;

public class SpawnController : MonoBehaviour {

	List<SpawnPoint> spawnPoints;

	void Awake ()
	{
		spawnPoints = new List<SpawnPoint>();

		foreach (Transform item in transform) {
			SpawnPoint spawnPoint = null;
			if(item.gameObject.activeSelf && (spawnPoint = item.gameObject.GetComponent<SpawnPoint>() )) 
			{
				spawnPoints.Add(spawnPoint);
			}
		}
	}

	public Transform GetSpawnPosition(LocationType location)
	{
		foreach (SpawnPoint point in spawnPoints) {
			if(point.locationModel.location == location)
			{
				return point.gameObject.transform;
			}
		}
		return null;
	}


}
