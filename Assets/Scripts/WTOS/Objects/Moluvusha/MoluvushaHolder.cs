using UnityEngine;
using System.Collections;

public class MoluvushaHolder : MonoBehaviour {
	[SerializeField] float moluvushaRadius = .3f;
	[SerializeField] float moluvushaBirthTime = 10.0f;
	[SerializeField] float moluvushaLifeTime = 30.0f;

	[SerializeField]
    Transform moluvushaGrowPoint;

    [SerializeField]
    Animator molHolderAnim;

    public int moluvushaCount = 1;
    public GameObject moluvushaPrefab;
    public float timeToGrow = 0.0f;

    public bool isEnableGrow = true;

    int plugTrigger,openTrigger,eatTrigger;

	//true = first time triggered faster ; false = first time, triggered as usual
	public bool isFirst = true;

    void Awake(){
        plugTrigger = Animator.StringToHash("Plugged");
        openTrigger = Animator.StringToHash("Open");
        eatTrigger = Animator.StringToHash("Eat");

        molHolderAnim = GetComponentInParent<Animator>();
    }
	
	void FixedUpdate () {
        CheckMoluvusha();

        if(moluvushaCount < 1 && isEnableGrow){
            moluvushaCount ++;
            StartCoroutine(GrowMoluvusha());
        }
	}

    void CheckMoluvusha(){
        Moluvusha[] mol = GetComponentsInChildren<Moluvusha>();

        moluvushaCount = mol.Length;
    }

    public void RemoveMoluvusha(){
        moluvushaCount--;
        isEnableGrow = false;

        if(molHolderAnim){
            molHolderAnim.SetTrigger(plugTrigger);
        }
    }

    IEnumerator GrowMoluvusha(){
        yield return new WaitForFixedUpdate();
        GameObject go = Instantiate(moluvushaPrefab,
                        moluvushaGrowPoint.position,
                        Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        go.transform.localScale = Vector3.zero;
		Moluvusha mov = go.GetComponent<Moluvusha> ();
		mov.moluvushaLifeTime = moluvushaLifeTime;
		mov.moluvushaRadius = moluvushaRadius;
		mov.moluvushaBirthTime = (isFirst) ? 1f :  moluvushaBirthTime;
		mov.StartMoluvusha ();
		isFirst = false;
    }

    public void DisableGrow(){
        isEnableGrow = false;
    }

    public void EnableGrow(){
        isEnableGrow = true;
    }

    public void Eat(){
        molHolderAnim.SetTrigger(eatTrigger);
    }

    public void OpenMoluvusha(){
        molHolderAnim.SetTrigger(openTrigger);
    }
}
