using UnityEngine;
using System.Collections;



public class ShaderAnimationHandler : MonoBehaviour {

	[SerializeField]
	protected string propertiesName;
	protected Material meshMat;
	protected float fadingSpeed = 0.1f;
	protected bool isFading = false;
	protected float startTime;
	protected float defaultFadingSpeed;

	protected virtual void Awake()
	{
		defaultFadingSpeed = fadingSpeed;
		if(this.GetComponent<MeshRenderer>() != null)
		{
			meshMat = this.GetComponent<MeshRenderer>().material;	
		}
		else if (this.GetComponent<ParticleSystemRenderer>() !=null)
		{
			meshMat = this.GetComponent<ParticleSystemRenderer>().material;
		}
        else if (this.GetComponent<SkinnedMeshRenderer>() != null){
            meshMat = this.GetComponent<SkinnedMeshRenderer>().material;
        }
	}

	public virtual void TriggerActivation(bool isOn, float speed = -1)
	{
		isFading = true;
		startTime = Time.time;
		if(speed != -1)
			fadingSpeed = speed;
		
	}
}
