using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PocaPetal : MonoBehaviour {

	public delegate void PocaDelegate ();
	public event PocaDelegate OnPocaUsedWhileTurnedOn;

	public NewLightVisualVFX LightHolder;
	public GameObject SparkyGO;

	[Header ("Material Changes When Turned On")]
	public bool isChangeMaterial;
	public float emissiveLevel = 1f;
	ShaderAnimationFloatHandler shaderHandler;

	ParticleSystem Sparky;
	Naivis naivisSource;
	public int index = 1;
	bool isIncreasing = true;

	[Header ("Light Data List")]
	public List<NewVisualData> lightData;
	Color lightSourceColor;
	void Awake(){
		shaderHandler = GetComponent<ShaderAnimationFloatHandler> ();
		if (lightData.Count > 0) {
			LightHolder.lightData = lightData [index];
			transform.localEulerAngles = lightData [index].RealPocaAngle;
		}
	}

	//trigger this light by flag(t/f)
	public void HasLight(bool flag, Color lightSourceColor, Naivis source = null){
		naivisSource = source;
		this.lightSourceColor = lightSourceColor;
		if (flag) {
			LightHolder.EnableLight(naivisSource, lightSourceColor);

			GameObject x = Instantiate (SparkyGO) as GameObject;
			x.transform.SetParent (this.transform.parent);
			x.transform.position = transform.position;
			Sparky = x.GetComponent<ParticleSystem> ();
			Sparky.Play ();
			if (isChangeMaterial) {
				shaderHandler.originValue = emissiveLevel;
				shaderHandler.TriggerActivation (true);
			}
		}else {
			LightHolder.DisableLight ();
			if (isChangeMaterial)
				shaderHandler.TriggerActivation (false);
			if (Sparky != null) {
				Sparky.Stop ();
				Destroy (Sparky.gameObject, 5f);
			}
		}
	}

	public void Use(){
		bool temp = LightHolder.Triggered;
		LightHolder.DisableLight ();
		if (lightData.Count > 0) {
			if (index == 0)
				isIncreasing = true;
			else if (index == lightData.Count - 1)
				isIncreasing = false;
			index = (isIncreasing) ? index + 1 : index - 1;
			LightHolder.lightData = lightData [index];
			StartCoroutine (RotatePoca ());
		}
		if (temp) {
			if (OnPocaUsedWhileTurnedOn != null)
				OnPocaUsedWhileTurnedOn ();
			LightHolder.EnableLight(naivisSource,lightSourceColor);
		}
	}

	IEnumerator RotatePoca(){
		float waktu = 1f;
		Vector3 temp = transform.localEulerAngles;
		while (waktu > 0f) {
			waktu -= Time.deltaTime;
			transform.localRotation = Quaternion.Lerp(transform.localRotation,Quaternion.Euler(lightData[index].RealPocaAngle),Time.deltaTime);
			yield return null;
		}
	}
}
