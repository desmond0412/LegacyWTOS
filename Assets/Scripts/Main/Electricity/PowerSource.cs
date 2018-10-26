using UnityEngine;
using System.Collections;

namespace Electricity
{
	/// <summary>
	/// Power source a.k.a. Battery.
	/// </summary>
	public class PowerSource : Conductor
	{

		public delegate void PowerSourceDelegate (GameObject sender, float capacity);

		public event PowerSourceDelegate OnPowerCharge;
		public event PowerSourceDelegate OnPowerFull;

		private const float MAXIMUM_CAPACITY = 100.0f;
		private const float MINIMUM_CAPACITY = 0.0f;
		private const float CHARGE_SPEED = 1.0f;
		private const float DISCHARGE_SPEED = 0.1f;

		public float powerCapacity;


		protected override void Update ()
		{
//			ReceivingPower();
			DelegateEvents();

			CleanPowerSources ();

			//CHARGED OVER TIME
			if (voltage > 0.0f)
				powerCapacity += voltage * CHARGE_SPEED * Time.deltaTime;
			else { // FIXME // either need to put this line of code under "else" or make it called every frame

				if (powerCapacity > 0.0f) {

					// power current get power from power capacity
					voltage = 10.0f;

					if (positiveNodes.Count > 0) {
						//only if there is any positive nodes connected
						powerCapacity -= DISCHARGE_SPEED * voltage * positiveNodes.Count * Time.deltaTime;
					} else {
						//discharged by the time
						powerCapacity -= DISCHARGE_SPEED * Time.deltaTime;
					}
				}

			}
				

			powerCapacity = Mathf.Clamp (powerCapacity, MINIMUM_CAPACITY, MAXIMUM_CAPACITY);	

			if (powerCapacity < MAXIMUM_CAPACITY) {
				if (OnPowerCharge != null)
					OnPowerCharge (gameObject, powerCapacity);
			} else if (powerCapacity == MAXIMUM_CAPACITY) {
				if (OnPowerFull != null)
					OnPowerFull (gameObject, powerCapacity);
			}
			TransmittingPower ();

			ResetPower();
		
		}
	}
}

