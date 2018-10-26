using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Electricity {
	public class Umba : Conductor {

		[SerializeField] GameObject TriggerUmba;
		[SerializeField] GameObject UselessUmba;

		public float timeFinishOneRotation = 10f;
		public int maxUmbaAtOneTime = 5;

		iTweenPath path;
		Transform umbaParent;
		List<GameObject> umbaList = new List<GameObject>();
		float timer = 0f;
		bool isStarted = false;
		int index = -1;

		public override void Awake ()
		{
			base.Awake ();
			path = GetComponent<iTweenPath> ();
			umbaParent = new GameObject ("UmbaParent").transform;
			umbaParent.SetParent (this.transform);

			OnObjectStartElectrified += TestStart;
			OnObjectUpdateElectrified += TestUpdate;
			OnObjectStopElectrified += TestStop;

			CreateUmba ();
		}

		protected override void Update ()
		{
			DelegateEvents();

			CleanPowerSources ();

			ResetPower();
		}

		void CreateUmba(){
			index = (index + 1) % (maxUmbaAtOneTime+1);
			GameObject create = Instantiate ((index == 0) ? TriggerUmba : UselessUmba) as GameObject;
			create.transform.SetParent (umbaParent);
			umbaList.Add (create);

			iTween.MoveTo (create,iTween.Hash("path",path.nodes.ToArray(),
				"time",timeFinishOneRotation,
				"easetype",iTween.EaseType.linear,
				"movetopath",false,
				"oncomplete","DestroyFinishedUmba",
				"oncompleteparams",create,
				"oncompletetarget",this.gameObject));
			if (!isStarted)
				iTween.Pause(create);
		}

		void DestroyFinishedUmba(GameObject x){
			umbaList.Remove (x);
			Destroy (x);
		}

		void TestStart(GameObject sender, float power){
			isStarted = true;
			iTween.Resume(umbaParent.gameObject,true);
		}

		void TestUpdate(GameObject sender, float power){
			timer += Time.deltaTime;
			if (timer > timeFinishOneRotation / maxUmbaAtOneTime) {
				timer -= timeFinishOneRotation / maxUmbaAtOneTime;
				CreateUmba ();
			}
		}

		void TestStop(GameObject sender, float power){
			isStarted = false;
			iTween.Pause(umbaParent.gameObject,true);
		}
	}
}