using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Electricity 
{
	public class TeslaCore : Conductor {

		public override void Awake ()
		{
			base.Awake ();
			positiveNodes = new List<Conductor>();
		}

		protected override void Update ()
		{
			DelegateEvents();

			CleanPowerSources ();

			TransmittingPower();

			ResetPower();
		}

		void OnTriggerStay(Collider other)
		{
			if (voltage <= 0.0f) return;

			if (other.GetComponent<Conductor> () != null) {

				Conductor condObject = other.GetComponent<Conductor> ();

				if (!condObject.isAccessible) 				return;
				if (sources.Contains (condObject))		
				{
					RemoveObject(condObject);
					return;
				}

				AddObject(condObject);
			}
		}

		void OnTriggerExit (Collider other)
		{
			if (other.GetComponent<Conductor> () != null) {

				Conductor condObject = other.GetComponent<Conductor> ();

				if (!condObject.isAccessible) 			return;
				if (sources.Contains (condObject)) return;

				RemoveObject(condObject);
			}
		}

		protected override void CleanPowerSources ()
		{
			if(voltage == 0 && sources.Count > 0)
			{
				sources.Clear();
				positiveNodes.Clear();
			}
		}


		private void AddObject(Conductor condObject)
		{
			if (!positiveNodes.Contains (condObject)) {
				positiveNodes.Add (condObject);
			}
		}

		private void RemoveObject(Conductor condObject)
		{
			if (positiveNodes.Contains (condObject)) {
				positiveNodes.Remove (condObject);
			}
		}



	}
}

