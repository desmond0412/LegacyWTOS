// 
// Helix.cs
// 
// Copyright (c) 2011-2014, Candlelight Interactive, LLC
// All rights reserved.
// 
// This file is licensed according to the terms of the Unity Asset Store EULA:
// http://download.unity3d.com/assetstore/customer-eula.pdf
// 
// This class describes a helix consisting of a length value and domain-
// normalized AnimationCurves to describe the width and twist down the length.
// 
// Construct a new Helix as needed. Use the Evaluate methods to obtain a point
// on the helix.
// 
// The AnimationCurve inspectors for the twist and width properties sometimes
// seem to freak out since they're applying changes in real-time. If you have
// problems, then try closing and re-opening the curve editors.

using UnityEngine;

namespace Candlelight
{
	/// <summary>
	/// A class to describe a helix.
	/// </summary>
	[System.Serializable]
	public class Helix
	{
		#region Backing Fields
		[SerializeField]
		private bool m_AutoSmoothCurveTangents = true;
		[SerializeField]
		private float m_Length = 1f;
		[SerializeField]
		private AnimationCurve m_Twist =
			new AnimationCurve(new Keyframe[2] { new Keyframe(0f, 0f, 360f, 360f), new Keyframe(1f, 360, 360f, 360f) });
		[SerializeField]
		private AnimationCurve m_Width = AnimationCurve.Linear(0f, 1f, 1f, 1f);
		#endregion
		/// <summary>
		/// Gets or sets a value indicating whether the tangents for the twist and width curves should be auto-smoothed.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if curve tangents should be auto-smoother; otherwise, <see langword="false"/>.
		/// </value>
		public bool AutoSmoothCurveTangents
		{
			get { return m_AutoSmoothCurveTangents; }
			set { m_AutoSmoothCurveTangents = value; }
		}
		/// <summary>
		/// Gets or sets the length.
		/// </summary>
		/// <value>The length.</value>
		public float Length
		{
			get { return m_Length; }
			set { m_Length = value; }
		}
		/// <summary>
		/// Gets or sets the twist curve.
		/// </summary>
		/// <value>The twist curve.</value>
		public AnimationCurve Twist
		{
			get { return m_Twist; }
			set
			{
				AnimationCurve curve = new AnimationCurve(value.keys);
				curve.NormalizeDomain();
				if (AutoSmoothCurveTangents)
				{
					curve.SmoothTangents();
				}
				m_Twist = curve;
			}
		}
		/// <summary>
		/// Gets or sets the twist end.
		/// </summary>
		/// <value>The twist end.</value>
		public float TwistEnd
		{
			get { return m_Twist.keys[m_Twist.length-1].value; }
			set
			{
				Keyframe[] newKeys = m_Twist.keys;
				newKeys[newKeys.Length-1].value = value;
				Twist = new AnimationCurve(newKeys);
			}
		}
		/// <summary>
		/// Gets or sets the twist start.
		/// </summary>
		/// <value>The twist start.</value>
		public float TwistStart
		{
			get { return m_Twist.keys[0].value; }
			set
			{
				Keyframe[] newKeys = m_Twist.keys;
				newKeys[0].value = value;
				Twist = new AnimationCurve(newKeys);
			}
		}
		/// <summary>
		/// Gets or sets the width curve.
		/// </summary>
		/// <value>The width curve.</value>
		public AnimationCurve Width
		{
			get { return m_Width; }
			set
			{
				AnimationCurve curve = new AnimationCurve(value.keys);
				curve.NormalizeDomain();
				if (AutoSmoothCurveTangents)
				{
					curve.SmoothTangents();
				}
				m_Width = curve;
			}
		}
		/// <summary>
		/// Gets or sets the width end.
		/// </summary>
		/// <value>The width end.</value>
		public float WidthEnd
		{
			get { return m_Width.keys[m_Width.length-1].value; }
			set
			{
				Keyframe[] newKeys = m_Width.keys;
				newKeys[newKeys.Length-1].value = value;
				Width = new AnimationCurve(newKeys);
			}
		}
		/// <summary>
		/// Gets or sets the width start.
		/// </summary>
		/// <value>The width start.</value>
		public float WidthStart
		{
			get { return m_Width.keys[0].value; }
			set
			{
				Keyframe[] newKeys = m_Width.keys;
				newKeys[0].value = value;
				Width = new AnimationCurve(newKeys);
			}
		}
		
		/// <summary>
		/// Tests if two helices are equivalent.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if all field values are equivalent; otherwise, <see langword="false"/>.
		/// </returns>
		/// <param name="h1">Helix 1.</param>
		/// <param name="h2">Helix 2.</param>
		public static bool AreEquivalent(Helix h1, Helix h2)
		{
			if (h1 == null)
			{
				return h2 == null;
			}
			else if (h2 == null)
			{
				return false;
			}
			else if (
				h1.AutoSmoothCurveTangents == h2.AutoSmoothCurveTangents &&
				h1.Length == h2.Length &&
				h1.Twist.IsValueEqualTo(h2.Twist) &&
				h1.Width.IsValueEqualTo(h2.Width)
			)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Helix"/> class.
		/// </summary>
		public Helix()
		{
			
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Helix"/> class.
		/// </summary>
		/// <param name="length">Length.</param>
		public Helix(float length)
		{
			this.Length = length;
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Helix"/> class.
		/// </summary>
		/// <param name="length">Length.</param>
		/// <param name="startTwist">Start twist.</param>
		/// <param name="endTwist">End twist.</param>
		public Helix(float length, float startTwist, float endTwist)
		{
			this.Length = length;
			Twist = AnimationCurve.Linear(0f, startTwist, 1f, endTwist);
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Helix"/> class.
		/// </summary>
		/// <param name="length">Length.</param>
		/// <param name="startTwist">Start twist.</param>
		/// <param name="endTwist">End twist.</param>
		/// <param name="startWidth">Start width.</param>
		/// <param name="endWidth">End width.</param>
		public Helix(float length, float startTwist, float endTwist, float startWidth, float endWidth)
		{
			this.Length = length;
			Twist = AnimationCurve.Linear(0f, startTwist, 1f, endTwist);
			Width = AnimationCurve.Linear(0f, startWidth, 1f, endWidth);
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Helix"/> class.
		/// </summary>
		/// <param name="length">Length.</param>
		/// <param name="twist">Twist.</param>
		/// <param name="width">Width.</param>
		/// <param name="autoSmoothCurveTangents">Auto smooth curve tangents.</param>
		/// <exception cref='System.ArgumentNullException'>
		/// Is thrown when a <see langword="null" /> AnimationCurve is passed.
		/// </exception>
		public Helix(float length, AnimationCurve twist, AnimationCurve width, bool autoSmoothCurveTangents)
		{
			this.Length = length;
			this.AutoSmoothCurveTangents = autoSmoothCurveTangents;
			if (twist == null)
			{
				throw new System.ArgumentNullException("twist");
			}
			else
			{
				Twist = twist;
			}
			if (width == null)
			{
				throw new System.ArgumentNullException("width");
			}
			else
			{
				Width = width;
			}
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Helix"/> class.
		/// </summary>
		/// <param name="helix">Helix.</param>
		/// <exception cref='System.ArgumentNullException'>
		/// Is thrown when a <see langword="null" /> Helix is passed.
		/// </exception>
		public Helix(Helix helix)
		{
			if (helix == null)
			{
				throw new System.ArgumentNullException("helix");
			}
			else
			{
				this.Length = helix.Length;
				this.AutoSmoothCurveTangents = helix.AutoSmoothCurveTangents;
				Twist = helix.Twist;
				Width = helix.Width;
			}
		}
		
		/// <summary>
		/// Evaluate the helix at the specified length value.
		/// </summary>
		/// <returns>The position on the helix at the specified length value.</returns>
		/// <param name="t">Length value.</param>
		public Vector3 Evaluate(float t)
		{
			return EvaluateNormalized(t / Length);
		}
		
		/// <summary>
		/// Evaluates the opposite point on the helix at the specified length value.
		/// </summary>
		/// <returns>The opposite position on the helix at the specified length value.</returns>
		/// <param name="t">Length value.</param>
		public Vector3 EvaluateOpposite(float t)
		{
			return EvaluateNormalizedOpposite(t / Length);
		}
		
		/// <summary>
		/// Evaluates the helix at the specified homogeneous parameter value.
		/// </summary>
		/// <returns>The position on the helix at the specified parameter value.</returns>
		/// <param name="t">A parameter in the range [0f, 1f].</param>
		public Vector3 EvaluateNormalized(float t)
		{
			Vector3 v = Quaternion.AngleAxis(m_Twist.Evaluate(t), Vector3.forward) *
				Vector3.right * m_Width.Evaluate(t); // default orientation at twist = 0 is Vector3.right
			v.z = Length * t;
			return v;
		}
		
		/// <summary>
		/// Evaluates the opposite point on the helix at the specified homogeneous parameter value.
		/// </summary>
		/// <returns>The opposite position on the helix at the specified parameter value.</returns>
		/// <param name="t">A parameter in the range [0f, 1f].</param>
		public Vector3 EvaluateNormalizedOpposite(float t)
		{
			Vector3 v = EvaluateNormalized(t);
			// NOTE: Vector3.forward would be tangent on curve if using spline
			return v.GetPointReflectedAcrossPlane(v - Vector3.forward * t * Length);
		}
		
		/// <summary>
		/// Gets the tangent on the helix at the specified homogeneous parameter value.
		/// </summary>
		/// <returns>The tangent on the helix at the specified parameter value.</returns>
		/// <param name="t">A parameter in the range [0f, 1f].</param>
		/// <param name="integratorPrecision">Integrator precision.</param>
		public Vector3 GetTangent(float t, float integratorPrecision)
		{
			return EvaluateNormalized(t + integratorPrecision) - EvaluateNormalized(t - integratorPrecision);
		}
		
		/// <summary>
		/// Gets the normal on the helix at the specified homogeneous parameter value.
		/// </summary>
		/// <returns>The normal on the helix at the specified parameter value.</returns>
		/// <param name="t">A parameter in the range [0f, 1f].</param>
		public Vector3 GetNormal(float t)
		{
			return EvaluateNormalized(t) - Length * Vector3.forward;
		}
	}
}