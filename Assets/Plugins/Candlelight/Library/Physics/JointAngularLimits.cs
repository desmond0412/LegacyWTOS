// 
// JointAngularLimits.cs
// 
// Copyright (c) 2013-2015, Candlelight Interactive, LLC
// All rights reserved.
// 
// This file is licensed according to the terms of the Unity Asset Store EULA:
// http://download.unity3d.com/assetstore/customer-eula.pdf

using UnityEngine;

namespace Candlelight.Physics
{
	/// <summary>
	/// A struct for working with <see cref="UnityEngine.ConfigurableJoint"/> angular limits.
	/// </summary>
	[System.Serializable]
	public struct JointAngularLimits : IPropertyBackingFieldCompatible<JointAngularLimits>
	{
		#region Backing Fields
		[SerializeField, PropertyBackingField]
		private float m_XMin;
		[SerializeField, PropertyBackingField]
		private float m_XMax;
		[SerializeField, PropertyBackingField]
		private float m_YMax;
		[SerializeField, PropertyBackingField]
		private float m_ZMax;
		#endregion
		/// <summary>
		/// Gets the X maximum.
		/// </summary>
		/// <value>The X maximum.</value>
		public float XMax
		{
			get { return m_XMax; }
			private set { m_XMax = Mathf.Max(Mathf.Min(value, 180f), m_XMin); } // included for inspector only
		}
		/// <summary>
		/// Gets the X minimum.
		/// </summary>
		/// <value>The X minimum.</value>
		public float XMin
		{
			get { return m_XMin; }
			private set { m_XMin = Mathf.Min(Mathf.Max(value, -180f), m_XMax); } // included for inspector only
		}
		/// <summary>
		/// Gets the Y maximum (symmetrical).
		/// </summary>
		/// <value>The Y maximum (symmetrical).</value>
		public float YMax
		{
			get { return m_YMax; }
			set { m_YMax = Mathf.Clamp(value, 0f, 180f); } // included for inspector only
		}
		/// <summary>
		/// Gets the Z maximum (symmetrical).
		/// </summary>
		/// <value>The Z maximum (symmetrical).</value>
		public float ZMax
		{
			get { return m_ZMax; }
			set { m_ZMax = Mathf.Clamp(value, 0f, 180f); } // included for inspector only
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JointAngularLimits"/> struct.
		/// </summary>
		/// <param name="xMin">X minimum.</param>
		/// <param name="xMax">X maximum.</param>
		/// <param name="yMax">Y maximum.</param>
		/// <param name="zMax">Z maximum.</param>
		public JointAngularLimits(float xMin, float xMax, float yMax, float zMax) : this()
		{
			xMin = Mathf.Min(xMin, xMax);
			xMax = Mathf.Max(xMin, xMax);
			m_XMin = xMin;
			m_XMax = xMax;
			m_YMax = yMax;
			m_ZMax = zMax;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JointAngularLimits"/> struct.
		/// </summary>
		/// <param name="joint">A <see cref="UnityEngine.Joint"/>.</param>
		public JointAngularLimits(Joint joint) : this()
		{
			if (joint == null)
			{
				return;
			}
			if (joint is CharacterJoint)
			{
				CharacterJoint cj = joint as CharacterJoint;
				m_XMin = cj.lowTwistLimit.limit;
				m_XMax = cj.highTwistLimit.limit;
				m_YMax = cj.swing1Limit.limit;
				m_ZMax = cj.swing2Limit.limit;
			}
			else if (joint is ConfigurableJoint)
			{
				ConfigurableJoint cj = joint as ConfigurableJoint;
				m_XMin = cj.lowAngularXLimit.limit;
				m_XMax = cj.highAngularXLimit.limit;
				m_YMax = cj.angularYLimit.limit;
				m_ZMax = cj.angularZLimit.limit;
			}
			else if (joint is HingeJoint)
			{
				HingeJoint hj = joint as HingeJoint;
				m_XMin = hj.limits.min;
				m_XMax = hj.limits.max;
			}
		}

		/// <summary>
		/// Applies this instance to a configurable joint.
		/// </summary>
		/// <param name="joint">A configurable joint.</param>
		public void ApplyToJoint(Joint joint)
		{
			if (joint == null)
			{
				return;
			}
			else if (joint is CharacterJoint)
			{
				CharacterJoint cj = joint as CharacterJoint;
				SoftJointLimit limit = cj.lowTwistLimit;
				limit.limit = this.XMin;
				cj.lowTwistLimit = limit;
				limit = cj.highTwistLimit;
				limit.limit = this.XMax;
				cj.highTwistLimit = limit;
				limit = cj.swing1Limit;
				limit.limit = this.YMax;
				cj.swing1Limit = limit;
				limit = cj.swing2Limit;
				limit.limit = this.ZMax;
				cj.swing2Limit = limit;
			}
			else if (joint is ConfigurableJoint)
			{
				ConfigurableJoint cj = joint as ConfigurableJoint;
				SoftJointLimit limit = cj.lowAngularXLimit;
				limit.limit = m_XMin;
				cj.lowAngularXLimit = limit;
				limit = cj.highAngularXLimit;
				limit.limit = m_XMax;
				cj.highAngularXLimit = limit;
				limit = cj.angularYLimit;
				limit.limit = m_YMax;
				cj.angularYLimit = limit;
				limit = cj.angularZLimit;
				limit.limit = m_ZMax;
				cj.angularZLimit = limit;
			}
			else if (joint is HingeJoint)
			{
				HingeJoint hj = joint as HingeJoint;
				JointLimits limits = hj.limits;
				limits.min = m_XMin;
				limits.max = m_XMax;
				hj.limits = limits;
			}
		}

#if UNITY_EDITOR
		/// <summary>
		/// Applies this instance to a joint.
		/// </summary>
		/// <param name="joint">A SerializedObject representation of a <see cref="UnityEngine.CharacterJoint"/> or
		/// <see cref="UnityEngine.ConfigurableJoint"/>.
		/// </param>
		public void ApplyToJoint(UnityEditor.SerializedObject joint)
		{
			if (joint == null || joint.targetObject == null)
			{
				return;
			}
			if (joint.targetObject is CharacterJoint)
			{
				joint.FindProperty("m_LowTwistLimit.limit").floatValue = this.XMin;
				joint.FindProperty("m_HighTwistLimit.limit").floatValue = this.XMax;
				joint.FindProperty("m_Swing1Limit.limit").floatValue = this.YMax;
				joint.FindProperty("m_Swing2Limit.limit").floatValue = this.ZMax;
			}
			else if (joint.targetObject is ConfigurableJoint)
			{
				joint.FindProperty("m_LowAngularXLimit.limit").floatValue = this.XMin;
				joint.FindProperty("m_HighAngularXLimit.limit").floatValue = this.XMax;
				joint.FindProperty("m_AngularYLimit.limit").floatValue = this.YMax;
				joint.FindProperty("m_AngularZLimit.limit").floatValue = this.ZMax;
			}
			else
			{
				Debug.LogError(string.Format("Unsupported type: {0}", joint.targetObject.GetType().Name));
			}
		}
#endif

		/// <summary>
		/// Clone this instance.
		/// </summary>
		/// <returns>A clone of this instance.</returns>
		public object Clone()
		{
			return this;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="JointAngularLimits"/>.
		/// </summary>
		/// <param name="obj">
		/// The <see cref="System.Object"/> to compare with the current <see cref="JointAngularLimits"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="JointAngularLimits"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public override bool Equals(object obj)
		{
			return ObjectX.Equals(ref this, obj);
		}

		/// <summary>
		/// Determines whether the specified <see cref="JointAngularLimits"/> is equal to the current
		/// <see cref="JointAngularLimits"/>.
		/// </summary>
		/// <param name="other">
		/// The <see cref="JointAngularLimits"/> to compare with the current <see cref="JointAngularLimits"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the specified <see cref="JointAngularLimits"/> is equal to the current
		/// <see cref="JointAngularLimits"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public bool Equals(JointAngularLimits other)
		{
			return GetHashCode() == other.GetHashCode();
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="JointAngularLimits"/> object.
		/// </summary>
		/// <returns>
		/// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
		/// hash table.
		/// </returns>
		public override int GetHashCode()
		{
			return ObjectX.GenerateHashCode(
				m_XMax.GetHashCode(), m_XMin.GetHashCode(), m_YMax.GetHashCode(), m_ZMax.GetHashCode()
			);
		}

		/// <summary>
		/// Gets a hash value that is based on the values of the serialized properties of this instance.
		/// </summary>
		/// <returns>A hash value based on the values of the serialized properties on this instance.</returns>
		public int GetSerializedPropertiesHash()
		{
			return GetHashCode();
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current
		/// <see cref="JointAngularLimits"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="JointAngularLimits"/>.
		/// </returns>
		public override string ToString()
		{
			return string.Format(
				"[JointAngularLimits: XMin={0}, XMax={1}, YMax={2}, ZMax={3}]", m_XMin, m_XMax, m_YMax, m_ZMax
			);
		}
	}
}