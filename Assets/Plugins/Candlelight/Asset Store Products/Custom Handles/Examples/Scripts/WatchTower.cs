// 
// WatchTower.cs
// 
// Copyright (c) 2011-2015, Candlelight Interactive, LLC
// All rights reserved.
// 
// This file is licensed according to the terms of the Unity Asset Store EULA:
// http://download.unity3d.com/assetstore/customer-eula.pdf

using UnityEngine;
using System.Collections.Generic;

namespace Candlelight.Examples.CustomHandles
{
	/// <summary>
	/// An example awareness tester.
	/// </summary>
	public class WatchTower : MonoBehaviour
	{
		#region Backing Fields
		[SerializeField, UnityEngine.Serialization.FormerlySerializedAs("emitters")]
		private List<HelixParticleEmitter> m_Emitters = new List<HelixParticleEmitter>();
		#endregion

		/// <summary>
		/// Gets the emitters.
		/// </summary>
		/// <returns>The number of emitters.</returns>
		/// <param name="emitters">A list of <see cref="HelixParticleEmitter"/> to populate.</param>
		public int GetEmitters(ref List<HelixParticleEmitter> emitters)
		{
			emitters = emitters ?? new List<HelixParticleEmitter>();
			emitters.Clear();
			emitters.AddRange(m_Emitters);
			return emitters.Count;
		}
		
		/// <summary>
		/// Stop emitting
		/// </summary>
		private void OnBecameUnaware()
		{
			for (int i = 0; i < m_Emitters.Count; ++i)
			{
				if (m_Emitters[i] == null)
				{
					continue;
				}
				m_Emitters[i].ShouldEmit = false;
			}
		}
		
		/// <summary>
		/// Create a suspicious red emission, with more alpha as the sound is closer
		/// </summary>
		/// <param name="evt">A <see cref="HearingEvent"/></param>
		private void OnHeardSomething(HearingEvent evt)
		{
			for (int i = 0; i < m_Emitters.Count; ++i)
			{
				if (m_Emitters[i] == null)
				{
					continue;
				}
				if (m_Emitters[i].ShouldEmit)
				{
					return;
				}
				m_Emitters[i].Emit(2f, 0.2f, new Color(0.9f, 0.2f, 0.2f, evt.FalloffValue));
			}
		}
		
		/// <summary>
		/// Go berserk with a white emission
		/// </summary>
		/// <param name="evt">A <see cref="SightEvent"/></param>
		private void OnSawSomething(SightEvent evt)
		{
			for (int i = 0; i < m_Emitters.Count; ++i)
			{
				if (m_Emitters[i] == null)
				{
					continue;
				}
				m_Emitters[i].ShouldEmit = true;
			}
		}

		/// <summary>
		/// Sets the emitters.
		/// </summary>
		/// <param name="value">Value.</param>
		public void SetEmitters(IEnumerable<HelixParticleEmitter> value)
		{
			value = value ?? new HelixParticleEmitter[0];
			m_Emitters.Clear();
			m_Emitters.AddRange(value);
		}
		
		/// <summary>
		/// Initialize this instance.
		/// </summary>
		private void Start()
		{
			if (m_Emitters.Count < 1)
			{
				m_Emitters.AddRange(GetComponentsInChildren<HelixParticleEmitter>());
			}
		}
	}
}