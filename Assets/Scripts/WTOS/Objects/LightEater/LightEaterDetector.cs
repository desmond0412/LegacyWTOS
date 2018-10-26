using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Linq;

public class LightEaterDetector : MonoBehaviour {
    Moluvusha closestMol;

    [SerializeField]
    LightEaterMovementBezier moverScript;

    public bool isEnableChasing = true;

    private List<GameObject> molList;
    bool calledFixedUpdate = false;

    void Awake(){
        molList = new List<GameObject>();
        molList.Clear();
    }

    void Update(){
//        Debug.Log("Update : "+molList.Count);

        for(int i=molList.Count - 1;i>=0;i--){
            GameObject go = molList[i];

            if(go == null || !go.activeInHierarchy){
                molList.Remove(go);
            }
        }

        if(calledFixedUpdate && moverScript && molList.Count <= 0){
//            Debug.Log("Update 2 : "+molList.Count);
            moverScript.SetLightEaterMoluvusha(null);
        }
    }

    void FixedUpdate(){
        calledFixedUpdate = true;
    }

    public GameObject GetClosestMoluvusha(){
        float currentClosest = 999.0f;
        GameObject currMoluvusha = null;

        molList = molList.Where(item => item!=null).ToList();

        int index = 0;
        foreach(GameObject mol in molList){
            if(mol && !mol.GetComponent<Moluvusha>().isBeingEaten){
                float curr = (transform.position - mol.transform.position).magnitude;

                if(curr < currentClosest){
                    currentClosest = curr;
                    currMoluvusha = mol;
                }
            }
            index++;
        }

        return currMoluvusha;
    }

    void LateUpdate(){
//        Debug.Log("LateUpdate : "+molList.Count);
        molList.Clear();
        calledFixedUpdate = false;
    }

    void OnTriggerStay(Collider col){
        MoluvushaEatable molScript = col.gameObject.GetComponent<MoluvushaEatable>();

        if(molScript){
            molList.Add(molScript.rootMoluvushaGO);
//            Debug.Log("OnTriggerStay : "+molList.Count);
            GameObject currM = GetClosestMoluvusha();
            if(currM && isEnableChasing){
                moverScript.SetLightEaterMoluvusha(currM);
            }
        }

    }
}
