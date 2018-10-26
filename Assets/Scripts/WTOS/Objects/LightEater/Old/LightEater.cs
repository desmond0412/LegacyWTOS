using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Linq;

public class LightEater : MonoBehaviour {
    public List<GameObject> molList;
    Moluvusha closestMol;
    LightEaterAnimator moverScript;

    public bool isEnableChasing = true;

    void Awake(){
        molList = new List<GameObject>();
        molList.Clear();
    }

    void Start(){
        moverScript = GetComponent<LightEaterAnimator>();
    }

    void FixedUpdate(){
        if(moverScript){
            GameObject currM = GetClosestMoluvusha();
            if(currM && isEnableChasing){
                moverScript.SetLightEaterMoluvusha(currM.transform);
            }else{
                moverScript.SetLightEaterMoluvusha(null);
            }
        }
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

    void OnTriggerEnter(Collider col){
        MoluvushaEatable molScript = col.gameObject.GetComponent<MoluvushaEatable>();

        if(molScript){
            molList.Add(molScript.rootMoluvushaGO);
        }
    }

    void OnTriggerExit(Collider col){
        MoluvushaEatable molScript = col.gameObject.GetComponent<MoluvushaEatable>();

        if(molScript){
            molList.Remove(molScript.rootMoluvushaGO);
        }
    }
}
