using UnityEngine;
using System.Collections;

public class SavePointVFXHandler : MonoBehaviour {
       
    public Animator particleAnimator;
    public SavePointTrigger savePoint;

    void Awake()
    {
        #if UNITY_EDITOR
        if(savePoint== null)
            Debug.LogError("SavePointVFXHandler NEEDS SAVE POINT TO BE SET UP");
        #endif
    }

    void Start()
    {
        savePoint.OnGameSaved += SavePoint_OnGameSaved;
    }

    void SavePoint_OnGameSaved (GameObject sender)
    {
        particleAnimator.SetTrigger("Save");
    }

    void OnDestroy()
    {
        savePoint.OnGameSaved -= SavePoint_OnGameSaved;
    }


}
