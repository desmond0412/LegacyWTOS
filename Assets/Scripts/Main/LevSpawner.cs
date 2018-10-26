using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpawnController))]
public class LevSpawner : MonoBehaviour {

	public GameObject LevPrefab;
	public LevControllerSO LevSetting;
	public bool autoSetCameraOnArrived = true;
	private SpawnController sController;
	private LevController lev;
	public void Awake()
	{
		sController = this.GetComponent<SpawnController>();
	}


	void Start()
	{
		//ONLY SPAWN LEV WHEN NO OBJECT WITH TAG PLAYER
		if(MainObjectSingleton.shared(MainObjectType.Player) != null) 
			return;	

		LocationModel levSpawnPosition = GetLevSpawnPosition();
		Transform atTransform = sController.GetSpawnPosition(levSpawnPosition.location);


		// if location == start.. AUTO SET CAMERA WILL BE FORCED.
		if (levSpawnPosition.location == LocationType.Start) {
			autoSetCameraOnArrived = true;
		}

		lev = SpawnLev(atTransform).GetComponent<LevController>();
		ApplySetting();
	}

	void ApplySetting()
	{
		if(LevSetting!=null)
			lev.setLevSetting(LevSetting);
			
	}

	LocationModel GetLevSpawnPosition()
	{
		LocationModel levSpawnPosition = GameDataManager.shared().PlayerLocation;

		if(levSpawnPosition == null)
		{
            levSpawnPosition = new LocationModel(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,LocationType.Start);
		}
//      HACK
//		levSpawnPosition.location = LocationType.Arrived;
		return levSpawnPosition;
	}

	GameObject SpawnLev(Transform atTransform)
	{
		GameObject lev = Instantiate(LevPrefab,atTransform.position,Quaternion.identity) as GameObject;
		lev.name = "LEV";
		lev.GetComponent<LevGauntletController>().cam = Camera.main;
		//SET CAMERA POSITION AND CAMERA TARGET

		if(autoSetCameraOnArrived)
		{
			Camera.main.transform.position = atTransform.position + Camera.main.GetComponent<Artoncode.Core.SmoothFollow>().positionOffset;
			Camera.main.GetComponent<Artoncode.Core.SmoothFollow>().target = lev.transform;
			Camera.main.GetComponent<Artoncode.Core.SmoothFollow>().Reinit();
		}
		return lev;
	}


}
