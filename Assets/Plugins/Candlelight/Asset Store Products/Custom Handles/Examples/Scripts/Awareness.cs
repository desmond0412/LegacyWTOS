// 
// Awareness.cs
// 
// Copyright (c) 2011-2015, Candlelight Interactive, LLC
// All rights reserved.
// 
// This file is licensed according to the terms of the Unity Asset Store EULA:
// http://download.unity3d.com/assetstore/customer-eula.pdf

using UnityEngine;

namespace Candlelight.Examples.CustomHandles
{
	/// <summary>
	/// A component that performs awareness tests for hearing and vision against some target transform (e.g., a player).
	/// </summary>
	/// <remarks>
	/// The component sends the following messages:
	///     OnSawSomething: saw an object being sought
	///     OnHeardSomething: heard an object being sought
	///     OnBecameUnaware: is unaware of lookingFor object
	/// </remarks>
	[AddComponentMenu("AI/Awareness")]
	public class Awareness : MonoBehaviour
	{
		/// <summary>
		/// A timer to count down from the attention span and determine when to force another awareness test.
		/// </summary>
		private float awarenessTimer = 0f;
		#region Backing Fields
		[SerializeField]
		private Transform m_LookingFor;
		[SerializeField, PropertyBackingField]
		private float m_VisionAngle = 90f;
		[SerializeField, PropertyBackingField]
		private float m_VisionDistance = 12f;
		[SerializeField]
		private AnimationCurve m_HearingFalloff = AnimationCurve.EaseInOut(0f, 1f, 8f, 0f);
		[SerializeField, PropertyBackingField]
		private float m_AttentionSpan = 2f;
		#endregion
		/// <summary>
		/// Gets or sets how long the component will remain aware of the target before it loses interest.
		/// </summary>
		/// <value>The attention span.</value>
		public float AttentionSpan
		{
			get { return m_AttentionSpan; }
			set { m_AttentionSpan = Mathf.Max(0f, value); }
		}
		/// <summary>
		/// Gets or sets the hearing falloff curve.
		/// </summary>
		/// <value>The hearing falloff curve.</value>
		public AnimationCurve HearingFalloff
		{
			get { return m_HearingFalloff; }
			set { m_HearingFalloff = value; }
		}
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Awareness"/> is aware.
		/// </summary>
		/// <value><see langword="true"/> if is aware; otherwise, <see langword="false"/>.</value>
		public bool IsAware { get; private set; }
		/// <summary>
		/// Gets or sets the object for which this component is looking.
		/// </summary>
		/// <value>The object for which this component is looking.</value>
		public Transform LookingFor
		{
			get { return m_LookingFor; }
			set { m_LookingFor = value; }
		}
		/// <summary>
		/// Gets or sets the vision angle.
		/// </summary>
		/// <value>The vision angle.</value>
		public float VisionAngle
		{
			get { return m_VisionAngle; }
			set { m_VisionAngle = Mathf.Clamp(value, 0f, 360f); }
		}
		/// <summary>
		/// Gets or sets the vision distance.
		/// </summary>
		/// <value>The vision distance.</value>
		public float VisionDistance
		{
			get { return m_VisionDistance; }
			set { m_VisionDistance = Mathf.Max(0f, value); }
		}
		
		/// <summary>
		/// Check vision and hearing to see if the specified target is within  awareness. You could invoke this manually
		/// with any transform at any time you like.
		/// </summary>
		/// <param name='target'>Target.</param>
		private void Test(Transform target)
		{	
			// the vector to the target from the bot's location
			Vector3 toTarget = target.position - transform.position;
			toTarget.y = 0f; // zero-out the y-component to constrain to a 2-dimensional plane
			// perform a vision test
			if (
				toTarget.sqrMagnitude < VisionDistance * VisionDistance && 
				Vector3.Angle(transform.forward, toTarget) < 0.5f * VisionAngle
			)
			{
				IsAware = true;
				awarenessTimer = AttentionSpan;
				gameObject.SendMessage(
					"OnSawSomething", new SightEvent(this, target), SendMessageOptions.DontRequireReceiver
				);
			}
			// perform a hearing test
			if (toTarget.sqrMagnitude <= Mathf.Pow(HearingFalloff.keys[HearingFalloff.length-1].time, 2))
			{
				IsAware = true;
				awarenessTimer = AttentionSpan;
				gameObject.SendMessage(
					"OnHeardSomething",
					new HearingEvent(this, target, HearingFalloff.Evaluate(toTarget.magnitude)),
					SendMessageOptions.DontRequireReceiver
				);
			}
		}
		
		/// <summary>
		/// Perform an awareness test.
		/// </summary>
		void Update()
		{
			// if there is a target, and the component is not currently aware, do an awareness test
			if (LookingFor != null && !IsAware)
			{
				Test(LookingFor);
			}
			// decrement the awareness timer to force the bot to do another manual awareness test
			if (awarenessTimer > 0f)
			{
				awarenessTimer -= Time.deltaTime;
			}
			else
			{
				IsAware = false;
				gameObject.SendMessage("OnBecameUnaware", this, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	/// <summary>
	/// A class to describe a hearing event.
	/// </summary>
	public class HearingEvent
	{
		/// <summary>
		/// Gets the tester.
		/// </summary>
		/// <value>The tester.</value>
		public Awareness Tester { get; private set; }
		/// <summary>
		/// Gets the target.
		/// </summary>
		/// <value>The target.</value>
		public Transform Target { get; private set; }
		/// <summary>
		/// Gets the falloff value.
		/// </summary>
		/// <value>The falloff value.</value>
		public float FalloffValue { get; private set; }
		/// <summary>
		/// Initializes a new instance of the <see cref="HearingEvent"/> class.
		/// </summary>
		/// <param name='tester'>Tester.</param>
		/// <param name='target'>Target.</param>
		/// <param name='falloffValue'>Falloff value.</param>
		public HearingEvent(Awareness tester, Transform target, float falloffValue)
		{
			this.Tester = tester;
			this.Target = target;
			this.FalloffValue = falloffValue;
		}
	}

	/// <summary>
	/// A class to describe a sight event.
	/// </summary>
	public class SightEvent
	{
		/// <summary>
		/// Gets the tester.
		/// </summary>
		/// <value>The tester.</value>
		public Awareness Tester { get; private set; }
		/// <summary>
		/// Gets the target.
		/// </summary>
		/// <value>The target.</value>
		public Transform Target { get; private set; }
		/// <summary>
		/// Initializes a new instance of the <see cref="SightEvent"/> class.
		/// </summary>
		/// <param name='tester'>Tester.</param>
		/// <param name='target'>Target.</param>
		public SightEvent(Awareness tester, Transform target)
		{
			this.Tester = tester;
			this.Target = target;
		}
	}
}