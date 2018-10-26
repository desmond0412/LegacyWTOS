using UnityEngine;
using System.Collections;

namespace Electricity 
{
	public class Switch : Conductor {

		public delegate void SwitchDelegate(GameObject sender, bool status);
		public event SwitchDelegate OnSwitchValueChanged;

		[SerializeField]
		private bool isOn;
		public bool IsOn{
			get{
				return isOn;
			}
			set{
				isOn = value;
				if(OnSwitchValueChanged !=null)
					OnSwitchValueChanged(this.gameObject,isOn);
			}
		}

		protected override void Update ()
		{

			DelegateEvents();

			CleanPowerSources();

			if (IsOn) 
				TransmittingPower ();
			
			ResetPower();

		}
	}
}

