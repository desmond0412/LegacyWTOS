using UnityEngine;
using System.Collections;

using Artoncode.WTOS;
using Artoncode.Core.Utility;

public class KopaScript : MonoBehaviour {

	public delegate void KopaScriptDelegate(GameObject sender);
	public event KopaScriptDelegate OnKopaUnplugged;
    public event KopaScriptDelegate OnKopaPlugged;

    [SerializeField]
	GameObject WaterPlug,WaterCopaClue,WaterParticle,WaterContainer;

    [SerializeField]
    float waterHeight = 5.0f, particleStartLifetime = 0.0f, particleStartSpeed = 1.5f;

    public float timeToFillWater = 5.0f;
    public float timeToAbsorbWater = 5.0f;
	public float timeToStopSFX = 5.0f;
    public float particleMinLifetime = 0.05f;
    public float particleMinSpeed = 1.0f;
    public float particleSpeedDeceleration = 1.0f;
    public float particleLifetimeDeceleration = 0.03f;

	public ParticleSystem[] vfxCopa;

//	ParticleSystem  vfxCopaColor;
    [SerializeField]
    private bool _IsPlugged = true;
    public bool IsPlugged{
        get{
            return _IsPlugged;
        }
        set{
            _IsPlugged = value;

			if(!_IsPlugged)
			{
				if(OnKopaUnplugged != null)
					OnKopaUnplugged(this.gameObject);
            }else if(_IsPlugged){
                if(OnKopaPlugged != null)
                    OnKopaPlugged(this.gameObject);
            }
        }
    }

    void Awake(){
        WaterPlug.GetComponent<KopaWaterplug>().enabled = true;
        WaterCopaClue.SetActive(true);
        WaterParticle.SetActive(false);
    }

	void FixedUpdate () {
        particleStartSpeed = GameUtility.changeTowards (particleStartSpeed, particleMinSpeed, particleSpeedDeceleration, Time.fixedDeltaTime);
        particleStartLifetime = GameUtility.changeTowards (particleStartLifetime, particleMinLifetime, particleLifetimeDeceleration, Time.fixedDeltaTime);

		foreach (ParticleSystem item in vfxCopa) {
			item.startLifetime = particleStartLifetime;
			item.startSpeed = particleStartSpeed;
		}
    }

    public void SetPlug(bool plugState){
        if(IsPlugged != plugState){
            IsPlugged = plugState;
            SimulateStartingKopa();
        }
    }

    void SimulateStartingKopa(){
		if(IsPlugged){
			WaterPlug.GetComponent<KopaWaterplug>().enabled = true;
			WaterCopaClue.SetActive(true);
            WaterParticle.SetActive(false);

            if(WaterContainer.GetComponent<WaterContainerHeight>()){
                WaterContainer.GetComponent<WaterContainerHeight>().AbsorbWater(timeToAbsorbWater);
            }
        }else{
			WaterPlug.GetComponent<KopaWaterplug>().enabled = false;
			WaterCopaClue.SetActive(false);
            WaterParticle.SetActive(true);

            if(WaterContainer.GetComponent<WaterContainerHeight>()){
                WaterContainer.GetComponent<WaterContainerHeight>().FillWater(timeToFillWater);
            }

            particleStartSpeed = 7.5f;
            particleStartLifetime = 0.7f;

			AkSoundEngine.PostEvent ("waterfall_loop", gameObject);
			StartCoroutine (stopSFX (timeToStopSFX));
        }
    }

	IEnumerator stopSFX (float delay) {
		yield return new WaitForSeconds (delay);
		AkSoundEngine.PostEvent ("waterfall_end", gameObject);
	}
}
