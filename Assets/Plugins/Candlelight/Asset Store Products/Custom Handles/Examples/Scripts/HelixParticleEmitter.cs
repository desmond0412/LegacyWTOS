// 
// HelixParticleEmitter.cs
// 
// Copyright (c) 2011-2015, Candlelight Interactive, LLC
// All rights reserved.
// 
// This file is licensed according to the terms of the Unity Asset Store EULA:
// http://download.unity3d.com/assetstore/customer-eula.pdf

using UnityEngine;
using System.Collections;

namespace Candlelight.Examples.CustomHandles
{
	/// <summary>
	/// A component for emitting particles down a helical path. It should be on a <see cref="UnityEngine.GameObject"/>
	/// with a <see cref="UnityEngine.ParticleEmitter"/>. If one does not exist, it will be added.
	/// </summary>
	/// <remarks>
	/// Since there is no API for oneShot on Unity's particle emitters, setting oneShot = true on this component will
	/// not affect the particle emitter that is added. As such, if you want oneShot to work in conjunction with
	/// autoDestruct on a ParticleAnimator, you must add an ellipsoid particle emitter manually and mark it as oneShot
	/// in the inspector.
	/// </remarks>
	[AddComponentMenu("Particles/Helix Particle Emitter")]
	public class HelixParticleEmitter : MonoBehaviour
	{
		/// <summary>
		/// The transform component.
		/// </summary>
		private Transform m_Transform;
		/// <summary>
		/// The emitter component.
		/// </summary>
		private ParticleEmitter m_Emitter;

		#region Backing Fields
		[SerializeField]
		private bool m_ShouldEmit = true;
		[SerializeField]
		private float m_MinSize = 0.1f;
		[SerializeField]
		private float m_MaxSize = 0.1f;
		[SerializeField]
		private float m_MinEnergy = 3f;
		[SerializeField]
		private float m_MaxEnergy = 3f;
		[SerializeField]
		public int m_MinEmission = 50;
		[SerializeField]
		private int m_MaxEmission = 50;
		[SerializeField]
		private Vector3 m_WorldVelocity = Vector3.zero;
		[SerializeField]
		private Vector3 m_LocalVelocity = Vector3.zero;
		[SerializeField]
		private Vector3 m_RndVelocity = Vector3.zero;
		[SerializeField]
		private float m_EmitterVelocityScale = 0.05f;
		[SerializeField]
		private Vector3 m_TangentVelocity = Vector3.zero;
		[SerializeField, PropertyBackingField]
		private bool m_SimulateInWorldSpace = true;
		[SerializeField]
		private bool m_OneShot = false;
		[SerializeField]
		private float m_TimeScale = 1f;
		[SerializeField]
		private Helix m_Helix = new Helix();
		private Rigidbody m_Rigidbody;
		#endregion

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Candlelight.HelixParticleEmitter"/> should emit.
		/// </summary>
		/// <value><see langword="true"/> if this instance should emit; otherwise, <see langword="false"/>.</value>
		public bool ShouldEmit
		{
			get { return m_ShouldEmit; }
			set { m_ShouldEmit = value; }
		}
		/// <summary>
		/// Gets or sets the minimum size.
		/// </summary>
		/// <value>The minimum size.</value>
		public float MinSize
		{
			get { return m_MinSize; }
			set { m_MinSize = value; }
		}
		/// <summary>
		/// Gets or sets the maximum size.
		/// </summary>
		/// <value>The maximum size.</value>
		public float MaxSize
		{
			get { return m_MaxSize; }
			set { m_MaxSize = value; }
		}
		/// <summary>
		/// Gets or sets the minimum energy.
		/// </summary>
		/// <value>The minimum energy.</value>
		public float MinEnergy
		{
			get { return m_MinEnergy; }
			set { m_MinEnergy = value; }
		}
		/// <summary>
		/// Gets or sets the maximum energy.
		/// </summary>
		/// <value>The maximum energy.</value>
		public float MaxEnergy
		{
			get { return m_MaxEnergy; }
			set { m_MaxEnergy = value; }
		}
		/// <summary>
		/// Gets or sets the minimum emission.
		/// </summary>
		/// <value>The minimum emission.</value>
		public int MinEmission
		{
			get { return m_MinEmission; }
			set { m_MinEmission = value; }
		}
		/// <summary>
		/// Gets or sets the max emission.
		/// </summary>
		/// <value>The max emission.</value>
		public int MaxEmission
		{
			get { return m_MaxEmission; }
			set { m_MaxEmission = value; }
		}
		/// <summary>
		/// Gets or sets the world velocity.
		/// </summary>
		/// <value>The world velocity.</value>
		public Vector3 WorldVelocity
		{
			get { return m_WorldVelocity; }
			set { m_WorldVelocity = value; }
		}
		/// <summary>
		/// Gets or sets the local velocity.
		/// </summary>
		/// <value>The local velocity.</value>
		public Vector3 LocalVelocity
		{
			get { return m_LocalVelocity; }
			set { m_LocalVelocity = value; }
		}
		/// <summary>
		/// Gets or sets the random velocity.
		/// </summary>
		/// <value>The random velocity.</value>
		public Vector3 RndVelocity
		{
			get { return m_RndVelocity; }
			set { m_RndVelocity = value; }
		}
		/// <summary>
		/// Gets or sets the emitter velocity scale.
		/// </summary>
		/// <value>The emitter velocity scale.</value>
		public float EmitterVelocityScale
		{
			get { return m_EmitterVelocityScale; }
			set { m_EmitterVelocityScale = value; }
		}
		/// <summary>
		/// Gets or sets the tangent velocity.
		/// </summary>
		/// <value>The tangent velocity.</value>
		public Vector3 TangentVelocity
		{
			get { return m_TangentVelocity; }
			set { m_TangentVelocity = value; }
		}
		/// <summary>
		/// Gets or sets a value indicating whether this
		/// <see cref="HelixParticleEmitter"/> simulates in world space.
		/// </summary>
		/// <value><see langword="true"/> if simulates in world space; otherwise, <see langword="false"/>.</value>
		public bool SimulateInWorldSpace
		{
			get { return m_SimulateInWorldSpace; }
			set
			{
				if (Application.isPlaying)
				{
					m_Emitter.useWorldSpace = value;
				}
				m_SimulateInWorldSpace = value;
			}
		}
		/// <summary>
		/// Gets or sets a value specifying whether the particles all shoot at once or continuously.
		/// </summary>
		/// <value>A value specifying whether the particles all shoot at once or continuously.</value>
		public bool OneShot
		{
			get { return m_OneShot; }
			set { m_OneShot = value; }
		}
		/// <summary>
		/// Gets or sets the time scale.
		/// </summary>
		/// <value>The time scale.</value>
		public float TimeScale
		{
			get { return m_TimeScale; }
			set { m_TimeScale = value; }
		}
		/// <summary>
		/// Gets or sets the helix.
		/// </summary>
		/// <value>The helix.</value>
		public Helix Helix
		{
			get { return m_Helix; }
			set { m_Helix = value; }
		}
		/// <summary>
		/// Gets the rigidbody.
		/// </summary>
		/// <value>The rigidbody.</value>
		public Rigidbody Rigidbody
		{
			get
			{
				if (m_Rigidbody == null)
				{
					m_Rigidbody = GetComponent<Rigidbody>();
				}
				return m_Rigidbody;
			}
		}
		
		/// <summary>
		/// Composite all velocity values at normalized parameter t.
		/// </summary>
		/// <returns>The velocities.</returns>
		/// <param name="t">A homogeneous parameter in the range [0f, 1f].</param>
		private Vector3 CompositeVelocities(float t)
		{
			Vector3 v = m_WorldVelocity + 
				m_Transform.rotation * m_LocalVelocity + 
					new Vector3(
						Random.Range(0f, m_RndVelocity.x),
						Random.Range(0f, m_RndVelocity.y),
						Random.Range(0f, m_RndVelocity.z)
					) + TransformTangentVelocity(t);
			if (this.Rigidbody != null)
			{
				v += m_EmitterVelocityScale * Rigidbody.velocity;
			}
			return v;
		}
		
		/// <summary>
		/// Do an emission for t seconds with supplied timeScale.
		/// </summary>
		/// <param name="t">Time in seconds.</param>
		/// <param name="timeScale">Time scale.</param>
		public void Emit(float t, float timeScale)
		{
			StartCoroutine(EmitCoroutine(t, timeScale));
		}
		
		/// <summary>
		/// Do a colored emission for t seconds with supplied timeScale.
		/// </summary>
		/// <param name="t">Time in seconds.</param>
		/// <param name="timeScale">Time scale.</param>
		/// <param name="color">Color.</param>
		public void Emit(float t, float timeScale, Color color)
		{
			StartCoroutine(EmitCoroutine(t, timeScale, color));
		}
		
		/// <summary>
		/// Emit particles as specified in emitter at parameter t on the helix.
		/// </summary>
		/// <param name="t">A parameter in the range [0f, helix.length].</param>
		public void Emit(float t)
		{
			EmitNormalized(t / m_Helix.Length);
		}
		
		/// <summary>
		/// Emit particles as specified in emitter at parameter t on the helix.
		/// </summary>
		/// <param name="t">A parameter in the range [0f, helix.length].</param>
		/// <param name="velocity">Velocity.</param>
		/// <param name="size">Size.</param>
		/// <param name="energy">Energy.</param>
		/// <param name="color">Color.</param>
		public void Emit(float t, Vector3 velocity, float size, float energy, Color color)
		{
			EmitNormalized(t / m_Helix.Length, velocity, size, energy, color);
		}
		
		/// <summary>
		/// Do a colored emission for t seconds with supplied timeScale.
		/// </summary>
		/// <param name="t">Time in seconds.</param>
		/// <param name="timeScale">Time scale.</param>
		/// <param name="color">Color.</param>
		IEnumerator EmitCoroutine(float t, float timeScale, Color color)
		{
			float timer = 0f;
			while (timer < t)
			{
				EmitNormalized((timer * timeScale) % 1f,
					CompositeVelocities(t),
					Random.Range(m_MinSize, m_MaxSize),
					Random.Range(m_MinEnergy, m_MaxEnergy),
					color
				);
				timer += Time.deltaTime;
				yield return 0;
			}
		}
		
		/// <summary>
		/// Do an emission for t seconds with supplied timeScale.
		/// </summary>
		/// <param name="t">Time in seconds.</param>
		/// <param name="timeScale">Time scale.</param>
		IEnumerator EmitCoroutine(float t, float timeScale)
		{
			float timer = 0f;
			while (timer < t)
			{
				EmitNormalized((timer * timeScale) % 1f);
				timer += Time.deltaTime;
				yield return 0;
			}
		}
		
		/// <summary>
		/// Emit particles as specified in emitter at parameter t on the helix.
		/// </summary>
		/// <param name="t">A homogeneous parameter in the range [0f, 1f].</param>
		public void EmitNormalized(float t)
		{
			EmitNormalized(
				t,
				CompositeVelocities(t), 
				Random.Range(m_MinSize, m_MaxSize), 
				Random.Range(m_MinEnergy, m_MaxEnergy), 
				Color.white
			);
		}
		
		/// <summary>
		/// Emit particles as specified in emitter at parameter t on the helix.
		/// </summary>
		/// <param name="t">A homogeneous parameter in the range [0f, 1f].</param>
		/// <param name="velocity">Velocity.</param>
		/// <param name="size">Size.</param>
		/// <param name="energy">Energy.</param>
		/// <param name="color">Color.</param>
		public void EmitNormalized(float t, Vector3 velocity, float size, float energy, Color color)
		{
			m_Emitter.Emit(
				m_SimulateInWorldSpace ?
					m_Transform.TransformPoint(m_Helix.EvaluateNormalized(t)) : m_Helix.EvaluateNormalized(t),
				velocity,
				size,
				energy,
				color
			);
		}
		
		/// <summary>
		/// Initialize.
		/// </summary>
		void Start()
		{
			m_Transform = transform;
			m_Emitter = GetComponent<ParticleEmitter>();
			if (m_Emitter == null)
			{
#if UNITY_4_5 || UNITY_4_6
				m_Emitter = gameObject.AddComponent("EllipsoidParticleEmitter") as ParticleEmitter;
#else
				m_Emitter = gameObject.AddComponent<EllipsoidParticleEmitter>();
#endif
			}
			m_Emitter.emit = false;
			m_Emitter.useWorldSpace = m_SimulateInWorldSpace;
		}
		
		/// <summary>
		/// Transform a tangent velocty value at normalized parameter t into world space.
		/// </summary>
		/// <returns>The tangent velocity.</returns>
		/// <param name="t">A homogeneous parameter in the range [0f, 1f].</param>
		private Vector3 TransformTangentVelocity(float t)
		{
			return m_Transform.TransformDirection(
				Quaternion.LookRotation(m_Helix.GetNormal(t), m_Helix.GetTangent(t, 0.0001f)) * m_TangentVelocity
			);
		}
		
		/// <summary>
		/// Emit if requested.
		/// </summary>
		void Update()
		{
			if (m_ShouldEmit && m_OneShot && Time.time * m_TimeScale > 1f)
			{
				m_ShouldEmit = false;
			}
			if (!m_ShouldEmit)
			{
				return;
			}
			// not sure if this is the desired way to use minEmission and maxEmission, but seems sensible enough for me
			for (int i = MinEmission; i <= MaxEmission; ++i)
			{
				EmitNormalized((Time.time * m_TimeScale) % 1f);
			}
		}
	}
}