using UnityEngine;
using System.Collections;

public class MoluvushaAnimatorHandler : MonoBehaviour {
    MoluvushaHolder molScript;

    void Start(){
        molScript = GetComponentInChildren<MoluvushaHolder>();
    }
        
    public void EnableGrow(){
        if(molScript){
            molScript.EnableGrow();
        }
    }

    public void DisableGrow(){
        if(molScript){
            molScript.DisableGrow();
        }
    }
}
