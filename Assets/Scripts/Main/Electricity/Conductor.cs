using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Electricity
{
	public class Conductor : MonoBehaviour
	{

		public delegate void ConductorDelegate(GameObject sender,float power);

		public event ConductorDelegate OnObjectStartElectrified;
		public event ConductorDelegate OnObjectUpdateElectrified;
		public event ConductorDelegate OnObjectStopElectrified;



		/// <summary>
		/// is Closed Circuit / Open Circuit
		/// </summary>
		[Tooltip("Is this object able to received power from outside?")]
		public bool isAccessible = false;

		public List<Conductor> positiveNodes;

		[HideInInspector]
		public List<Conductor> sources;

		/// <summary>
		/// The electic current.
		/// </summary>
		public float voltage;

		protected float lastVoltage;

		public virtual void Awake()
		{
			sources = new List<Conductor>();	
		}

		protected virtual void ApplyVoltage(float newVoltage)
		{
			if (isAccessible)
				voltage = newVoltage;
		}

		//HELPER
		public void InputUpdate()
		{
			if (Input.GetKey(KeyCode.A))
			{
				ApplyVoltage(10);
			}
		}

		protected virtual void Update()
		{
			
			//calling events
			DelegateEvents();

			//Clean power Source if there is no power
			CleanPowerSources();

			//send power to positives end.
			TransmittingPower();

			//reset power
			ResetPower();

			//HACK
			InputUpdate();
		}

		virtual protected void DelegateEvents()
		{
			if (voltage > 0.0f)
			{
				if (lastVoltage != voltage)
				{
//					print("start");
					if (OnObjectStartElectrified != null)
						OnObjectStartElectrified(this.gameObject, voltage);
				}

//				print("update");
				if (OnObjectUpdateElectrified != null)
					OnObjectUpdateElectrified(this.gameObject, voltage);
			}
			else if (voltage == 0.0f)
			{
				//called only once
				if (lastVoltage != voltage)
				{
//					print("stop");
					if (OnObjectStopElectrified != null)
						OnObjectStopElectrified(this.gameObject, voltage);
				}
			}
		}

		protected void ResetPower()
		{
			lastVoltage = voltage;
			voltage = 0.0f;
		}

		//		protected virtual void ReceivingPower()
		//		{
		//			foreach (Conductor node in negativeNodes) {
		//				if(node == null) continue;
		//				powerCurrent += node.powerCurrent;
		//			}
		//		}

		protected virtual void CleanPowerSources()
		{
			if (voltage == 0 && sources.Count > 0)
				sources.Clear();
		}

		protected virtual void TransmittingPower()
		{
			foreach (Conductor node in positiveNodes)
			{
				if (node == null)
					continue;

				if (!node.sources.Contains(this))
					node.sources.Add(this);

				//power doesnt spliting, but absorb more instead 
//				node.powerCurrent += (powerCurrent/positiveNodes.Count);

				node.voltage += voltage;
			}
		}

	}
}