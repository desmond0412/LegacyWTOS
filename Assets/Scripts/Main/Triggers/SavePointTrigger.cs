using UnityEngine;
using System.Collections;

public class SavePointTrigger : MonoBehaviour {

    public delegate void SavePointTriggerDelegate(GameObject sender);
    public event SavePointTriggerDelegate OnGameSaved;

	public SpawnPoint respawnPoint;


    public virtual void Awake()
	{
		#if UNITY_EDITOR
		if(respawnPoint== null)
			Debug.LogError("SAVE POINT TRIGGER NEEDS RESPAWN POINT TO BE SET UP");
		#endif
	}

    protected void TriggerSave()
    {
        GameDataManager.shared().SaveGameExist = true;
        GameDataManager.shared().PlayerLocation = respawnPoint.locationModel;
        GameDataManager.shared().Save();
        if(OnGameSaved != null )
            OnGameSaved(gameObject);
    }

    public virtual void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
            TriggerSave();
	}
}
