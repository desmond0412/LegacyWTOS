// 
// AnimationCurveX.cs
// 
// Copyright (c) 2014-2016, Candlelight Interactive, LLC
// All rights reserved.
// 
// This file is licensed according to the terms of the Unity Asset Store EULA:
// http://download.unity3d.com/assetstore/customer-eula.pdf
// 
// This file contains extension methods for AnimationCurve.

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Candlelight
{
	/// <summary>
	/// Extension methods for AnimationCurve.
	/// </summary>
	public static class AnimationCurveX
	{
		/// <summary>
		/// The default sampling frequency.
		/// </summary>
		private const float k_DefaultSamplingFrequency = 0.00001f;

		/// <summary>
		/// Gets the default sampling frequency.
		/// </summary>
		/// <value>The default sampling frequency.</value>
		public static float DefaultSamplingFrequency { get { return k_DefaultSamplingFrequency; } }

		#region Status Methods
		/// <summary>
		/// Gets the sampling frequency for inspector validation methods.
		/// </summary>
		/// <returns>The sampling frequency for inspector validation methods.</returns>
		/// <param name="curve">Curve.</param>
		private static float GetSamplingFrequencyForInspector(AnimationCurve curve)
		{
			return curve.length == 0 ? 0f : 0.001f * (curve[curve.length - 1].time - curve[0].time);
		}

		/// <summary>
		/// Gets the status of a backing field that should be monotonically decreasing.
		/// </summary>
		/// <returns>The normalized range profile status.</returns>
		/// <param name="provider">Provider.</param>
		/// <param name="testValue">Test value.</param>
		/// <param name="message">Message.</param>
		private static ValidationStatus GetMonotonicallyDecreasingCurveStatus(
			object provider, object testValue, out string message
		)
		{
			AnimationCurve curve = (AnimationCurve)testValue;
			if (curve == null)
			{
				message = "Backing field is not an AnimationCurve.";
				return ValidationStatus.Error;
			}
			if (!IsMonotonic(curve, -1, GetSamplingFrequencyForInspector(curve)))
			{
				message = "This curve is not monotonically decreasing.";
				return ValidationStatus.Warning;
			}
			message = "";
			return ValidationStatus.None;
		}

		/// <summary>
		/// Gets the status of a backing field that should be monotonically decreasing and within the range [0, 1].
		/// </summary>
		/// <returns>The normalized range profile status.</returns>
		/// <param name="provider">Provider.</param>
		/// <param name="testValue">Test value.</param>
		/// <param name="message">Message.</param>
		private static ValidationStatus GetMonotonicallyDecreasingNormalizedRangeCurveStatus(
			object provider, object testValue, out string message
		)
		{
			ValidationStatus status = GetNormalizedRangeCurveStatus(provider, testValue, out message);
			if (status == ValidationStatus.None)
			{
				return GetMonotonicallyDecreasingCurveStatus(provider, testValue, out message);
			}
			return status;
		}

		/// <summary>
		/// Gets the status of a backing field that should be monotonically increasing.
		/// </summary>
		/// <returns>The normalized range profile status.</returns>
		/// <param name="provider">Provider.</param>
		/// <param name="testValue">Test value.</param>
		/// <param name="message">Message.</param>
		private static ValidationStatus GetMonotonicallyIncreasingCurveStatus(
			object provider, object testValue, out string message
		)
		{
			AnimationCurve curve = (AnimationCurve)testValue;
			if (curve == null)
			{
				message = "Backing field is not an AnimationCurve.";
				return ValidationStatus.Error;
			}
			if (!IsMonotonic(curve, 1, GetSamplingFrequencyForInspector(curve)))
			{
				message = "This curve is not monotonically increasing.";
				return ValidationStatus.Warning;
			}
			message = "";
			return ValidationStatus.None;
		}

		/// <summary>
		/// Gets the status of a backing field that should be monotonically increasing and within the range [0, 1].
		/// </summary>
		/// <returns>The normalized range profile status.</returns>
		/// <param name="provider">Provider.</param>
		/// <param name="testValue">Test value.</param>
		/// <param name="message">Message.</param>
		private static ValidationStatus GetMonotonicallyIncreasingNormalizedRangeCurveStatus(
			object provider, object testValue, out string message
		)
		{
			ValidationStatus status = GetNormalizedRangeCurveStatus(provider, testValue, out message);
			if (status == ValidationStatus.None)
			{
				return GetMonotonicallyIncreasingCurveStatus(provider, testValue, out message);
			}
			return status;
		}

		/// <summary>
		/// Gets the status of a backing field whose should be within the range [0, 1].
		/// </summary>
		/// <returns>The normalized range profile status.</returns>
		/// <param name="provider">Provider.</param>
		/// <param name="testValue">Test value.</param>
		/// <param name="message">Message.</param>
		private static ValidationStatus GetNormalizedRangeCurveStatus(
			object provider, object testValue, out string message
		)
		{
			AnimationCurve curve = (AnimationCurve)testValue;
			if (curve == null)
			{
				message = "Backing field is not an AnimationCurve.";
				return ValidationStatus.Error;
			}
			float high, low;
			curve.GetRange(out high, out low, GetSamplingFrequencyForInspector(curve));
			if (high > 1f || low < 0f)
			{
				message = "This curve falls outside the range [0, 1].";
				return ValidationStatus.Warning;
			}
			message = "";
			return ValidationStatus.None;
		}
		#endregion

		/// <summary>
		/// Clamps the time values on the <paramref name="curve"/>'s keys to the specified domain.
		/// </summary>
		/// <remarks>
		/// In contrast to <see cref="AnimationCurveX.TransformDomain()"/>, this method simply adjusts the times of the
		/// first and last <see cref="UnityEngine.Keyframe"/>s without respecting the underlying curve shape. It is
		/// intended to be used for continuous validation of input data from the inspector, as it yields more
		/// predictable results.
		/// </remarks>
		/// <param name="curve">Curve.</param>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Maximum.</param>
		public static void ClampKeysToDomain(this AnimationCurve curve, float min, float max)
		{
			if (curve == null || curve.length == 0 || (curve[0].time >= min && curve[curve.length - 1].time <= max))
			{
				return;
			}
			Keyframe[] keys = curve.keys;
			if (curve.length == 1)
			{
				keys[0].time = Mathf.Clamp(keys[0].time, min, max);
			}
			else
			{
				keys[0].time = Mathf.Max(keys[0].time, min);
				keys[keys.Length - 1].time = Mathf.Min(keys[keys.Length - 1].time, max);
			}
			curve.keys = keys;
		}

		/// <summary>
		/// Clamps the values on the <paramref name="curve"/>'s keys to the specified domain.
		/// </summary>
		/// <remarks>
		/// In contrast to <see cref="AnimationCurveX.TransformRange()"/>, this method simply adjusts the values of the
		/// first and last <see cref="UnityEngine.Keyframe"/>s without respecting the underlying curve shape. This
		/// method is intended to be used for continuous validation of input data from the inspector, as it yields more
		/// predictable results, even though it ignores the fact that intermediate values may fall outside the range
		/// due to tangents.
		/// </remarks>
		/// <param name="curve">Curve.</param>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Maximum.</param>
		public static void ClampKeysToRange(this AnimationCurve curve, float min, float max)
		{
			if (curve == null || curve.length == 0)
			{
				return;
			}
			using (ListPool<Keyframe>.Scope keys = new ListPool<Keyframe>.Scope())
			{
				bool setNewKeys = false;
				for (int i = 0; i < curve.length; ++i)
				{
					Keyframe key = curve[i];
					if (key.value < min || key.value > max)
					{
						key.value = Mathf.Clamp(key.value, min, max);
						setNewKeys = true;
					}
					keys.List.Add(key);
				}
				if (setNewKeys)
				{
					curve.keys = keys.List.ToArray();
				}
			}
		}

		/// <summary>
		/// Evaluate the specified curve at the specified time.
		/// </summary>
		/// <returns>The value of <paramref name="curve"/> at the specified <paramref name="time"/>.</returns>
		/// <param name="curve">Curve.</param>
		/// <param name="time">Time.</param>
		/// <param name="inTangent">In tangent.</param>
		/// <param name="outTangent">Out tangent.</param>
		/// <param name="samplingFrequency">
		/// Sampling frequency (<see cref="UnityEngine.Mathf.Epsilon"/>, <see cref="UnityEngine.Mathf.Infinity"/>).
		/// </param>
		public static float Evaluate(
			this AnimationCurve curve, float time, out float inTangent, out float outTangent,
			float samplingFrequency = k_DefaultSamplingFrequency
		)
		{
			// ensure precision is a positive, nonzero number
			samplingFrequency = Mathf.Max(samplingFrequency, Mathf.Epsilon);
			float result = curve.Evaluate(time);
			float div = 1f / samplingFrequency;
			inTangent = (result - curve.Evaluate(time - samplingFrequency)) * div;
			outTangent = (curve.Evaluate(time + samplingFrequency) - result) * div;
			return result;
		}

		/// <summary>
		/// Gets a <see cref="System.String"/> representation of the keyframe values for the specified curve.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> representation of the keyframe values for the specified curve.
		/// </returns>
		/// <param name="curve">Curve.</param>
		public static string GetKeyframeString(this AnimationCurve curve)
		{
			return string.Format(
				"[{0}]", ", ".Join(from k in curve.keys select string.Format("({0:0.000}, {1:0.000})", k.time, k.value))
			);
		}
		
		/// <summary>
		/// Gets the curve range.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the range was successfully gotten; otherwise, <see langword="false"/>.
		/// </returns>
		/// <param name="curve">Curve.</param>
		/// <param name="high">Highest value.</param>
		/// <param name="low">Lowest value.</param>
		/// <param name="samplingFrequency">
		/// Sampling frequency (<see cref="UnityEngine.Mathf.Epsilon"/>, <see cref="UnityEngine.Mathf.Infinity"/>).
		/// </param>
		public static bool GetRange(this AnimationCurve curve, out float high, out float low, float samplingFrequency)
		{
			// early out if there are no keys
			if (curve.length == 0)
			{
				high = low = 0f;
				return false;
			}
			// initialize high and low values
			high = low = curve[0].value;
			// early out if there is only one key
			if (curve.length == 1)
			{
				return true;
			}
			// ensure precision is a positive, nonzero number
			samplingFrequency = Mathf.Max(samplingFrequency, Mathf.Epsilon);
			// sample the curve
			float t = curve[0].time;
			float v;
			while (t < curve[curve.length - 1].time)
			{
				v = curve.Evaluate(t);
				high = Mathf.Max(high, v);
				low = Mathf.Min(low, v);
				t += samplingFrequency;
			}
			// ensure a sample is taken at the last keyframe
			v = curve.Evaluate(curve[curve.length - 1].time);
			high = Mathf.Max(high, v);
			low = Mathf.Min(low, v);
			return true;
		}
		
		/// <summary>
		/// Gets the time at the specified normalized parameter value.
		/// </summary>
		/// <returns>The time at the specified normalized parameter value.</returns>
		/// <param name="curve">Curve.</param>
		/// <param name="t">A normalized parameter value.</param>
		public static float GetTimeAtParameter(this AnimationCurve curve, float t)
		{
			return curve[0].time + t * (curve[curve.length - 1].time - curve[0].time);
		}
		
		/// <summary>
		/// Gets a trimmed version of the supplied curve.
		/// </summary>
		/// <returns>A new curve trimmed to the specified domain.</returns>
		/// <param name="curve">Curve.</param>
		/// <param name="startFrame">Start frame.</param>
		/// <param name="endFrame">End frame.</param>
		/// <param name="frameRate">Frame rate.</param>
		/// <param name="tangentSamplingFrequency">Tangent sampling frequency.</param>
		public static AnimationCurve GetTrimmedCurve(
			this AnimationCurve curve, int startFrame, int endFrame, float frameRate, float tangentSamplingFrequency
		)
		{
			return curve.GetTrimmedCurve(startFrame / frameRate, endFrame / frameRate, tangentSamplingFrequency);
		}
		
		/// <summary>
		/// Gets a trimmed version of the supplied curve.
		/// </summary>
		/// <returns>A new curve trimmed to the specified domain.</returns>
		/// <param name="curve">Curve.</param>
		/// <param name="startTime">Start time.</param>
		/// <param name="endTime">End time.</param>
		/// <param name="tangentSamplingFrequency">Tangent sampling frequency.</param>
		public static AnimationCurve GetTrimmedCurve(
			this AnimationCurve curve, float startTime, float endTime, float tangentSamplingFrequency
		)
		{
			// new keys
			List<Keyframe> keys = new List<Keyframe>();
			// add first frame
			Keyframe firstKey = new Keyframe(0f, curve.Evaluate(startTime));
			firstKey.outTangent = (curve.Evaluate(startTime + tangentSamplingFrequency) - firstKey.value) /
				tangentSamplingFrequency;
			keys.Add(firstKey);
			// add middle frames
			for (int i = 0; i < curve.length; ++i)
			{
				if (curve[i].time > startTime && curve[i].time < endTime)
				{
					Keyframe key = curve[i];
					key.time -= startTime;
					keys.Add(key);
				}
			}
			// add last frame
			Keyframe lastKey = new Keyframe(endTime - startTime, curve.Evaluate(endTime));
			firstKey.inTangent = (curve.Evaluate(endTime - tangentSamplingFrequency) - lastKey.value) /
				tangentSamplingFrequency;
			keys.Add(lastKey);
			// return new curve
			AnimationCurve newCurve = new AnimationCurve(keys.ToArray());
			newCurve.postWrapMode = curve.postWrapMode;
			newCurve.preWrapMode = curve.preWrapMode;
			return newCurve;
		}

		/// <summary>
		/// Determines if the specified <paramref name="curve"/> is monotonic in the specified direction.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the specified <paramref name="curve"/> is monotonic in the specified direction;
		/// otherwise, <see langword="false"/>.
		/// </returns>
		/// <param name="curve">Curve.</param>
		/// <param name="direction">
		/// 0 to test for uniformity, greater than 0 to test for increasing, and less than 0 to test for decreasing.
		/// </param>
		/// <param name="samplingFrequency">
		/// Sampling frequency (<see cref="UnityEngine.Mathf.Epsilon"/>, <see cref="UnityEngine.Mathf.Infinity"/>).
		/// </param>
		public static bool IsMonotonic(
			this AnimationCurve curve, int direction, float samplingFrequency = k_DefaultSamplingFrequency
		)
		{
			if (curve == null)
			{
				return false;
			}
			if (curve.length < 2)
			{
				return true;
			}
			// ensure precision is a positive, nonzero number
			samplingFrequency = Mathf.Max(samplingFrequency, Mathf.Epsilon);
			direction = direction == 0 ? 0 : (int)Mathf.Sign(direction);
			int cmp;
			float t = curve[0].time + samplingFrequency;
			float lastTime = curve[curve.length - 1].time;
			while (t <= lastTime)
			{
				cmp = curve.Evaluate(t).CompareTo(curve.Evaluate(t - samplingFrequency));
				if (cmp != 0 && cmp != direction)
				{
					return false;
				}
				t += samplingFrequency;
			}
			cmp = curve.Evaluate(lastTime).CompareTo(curve.Evaluate(lastTime - samplingFrequency));
			return cmp == 0 || cmp == direction;
		}

		/// <summary>
		/// Determines if two curves are value-equivalent.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if instance is value-equivalent to the specified other curve; otherwise,
		/// <see langword="false"/>.
		/// </returns>
		/// <param name="c1">An <see cref="AnimationCurve"/>.</param>
		/// <param name="c2">An <see cref="AnimationCurve"/>.</param>
		/// <param name="tolerance">Threshold within which keyframe fields are assumed to be the same.</param>
		public static bool IsValueEqualTo(this AnimationCurve c1, AnimationCurve c2, float tolerance = 0f)
		{
			if (c1.postWrapMode != c2.postWrapMode || c1.preWrapMode != c2.preWrapMode || c1.length != c2.length)
			{
				return false;
			}
			bool areDifferent = false;
			tolerance = Mathf.Max(0f, tolerance);
			if (tolerance == 0f)
			{
				for (int i = 0; i < c1.length; ++i)
				{
					Keyframe k1 = c1[i];
					Keyframe k2 = c2[i];
					if (
						k1.tangentMode != k2.tangentMode ||
						k1.inTangent != k2.inTangent ||
						k1.outTangent != k2.outTangent ||
						k1.time != k2.time ||
						k1.value != k2.value
					)
					{
						areDifferent = true;
						break;
					}
				}
			}
			else
			{
				for (int i = 0; i < c1.length; ++i)
				{
					Keyframe k1 = c1[i];
					Keyframe k2 = c2[i];
					if (
						Mathf.Abs(k1.tangentMode - k2.tangentMode) > tolerance ||
						Mathf.Abs(k1.inTangent - k2.inTangent) > tolerance ||
						Mathf.Abs(k1.outTangent - k2.outTangent) > tolerance ||
						Mathf.Abs(k1.time - k2.time) > tolerance||
						Mathf.Abs(k1.value - k2.value) > tolerance
					)
					{
						areDifferent = true;
						break;
					}
				}
			}
			return !areDifferent;
		}
		
		/// <summary>
		/// Normalizes the curve's time to the domain [0, 1].
		/// </summary>
		/// <param name="curve">Curve.</param>
		public static void NormalizeDomain(this AnimationCurve curve)
		{
			curve.TransformDomain(0f, 1f);
		}
		
		/// <summary>
		/// Normalizes the curve's values to the range [0, 1].
		/// </summary>
		/// <param name="curve">Curve.</param>
		/// <param name="samplingFrequency">
		/// Sampling frequency (<see cref="UnityEngine.Mathf.Epsilon"/>, <see cref="UnityEngine.Mathf.Infinity"/>).
		/// </param>
		public static void NormalizeRange(this AnimationCurve curve, float samplingFrequency)
		{
			curve.TransformRange(0f, 1f, samplingFrequency);
		}
		
		/// <summary>
		/// Reverses the curve.
		/// </summary>
		/// <remarks>
		/// Reflects curve across its midpoint and swaps pre- and postWrapModes.
		/// </remarks>
		/// <param name="curve">Curve.</param>
		public static void Reverse(this AnimationCurve curve)
		{
			curve.TransformDomain(curve[curve.length - 1].time, curve[0].time);
			WrapMode pre = curve.preWrapMode;
			curve.preWrapMode = curve.postWrapMode;
			curve.postWrapMode = pre;
		}

		/// <summary>
		/// Scales the <see cref="UnityEngine.Keyframe"/>s on the specified <see cref="UnityEngine.AnimationCurve"/> by
		/// the specified amount.
		/// </summary>
		/// <param name="curve">Curve.</param>
		/// <param name="s">Scale factor.</param>
		public static void Scale(this AnimationCurve curve, Vector2 s)
		{
			using (ListPool<Keyframe>.Scope newKeys = new ListPool<Keyframe>.Scope())
			{
				newKeys.List.AddRange(curve.keys);
				float scaleTangent = s.y / s.x;
				for (int i = 0; i < newKeys.List.Count; ++i)
				{
					Keyframe newKey = new Keyframe(
						newKeys.List[i].time * s.x,
						newKeys.List[i].value * s.y,
						newKeys.List[i].inTangent * scaleTangent,
						newKeys.List[i].outTangent * scaleTangent
					);
					newKeys.List[i] = newKey;
				}
				if (newKeys.List[0].time > newKeys.List[newKeys.List.Count - 1].time)
				{
					newKeys.List.Reverse();
				}
				curve.keys = newKeys.List.ToArray();
			}
		}
		
		/// <summary>
		/// Smooths the animation curve tangents.
		/// </summary>
		/// <param name="curve">Curve.</param>
		public static void SmoothTangents(this AnimationCurve curve)
		{
			Keyframe[] keys = curve.keys;
			for (int i = 0; i < keys.Length; ++i)
			{
				// set tangents for first key
				if (i == 0)
				{
					keys[i].inTangent = keys[i].outTangent =
						(keys[i + 1].value - keys[i].value) / (keys[i + 1].time - keys[i].time);
				}
				// set tangents for final key
				else if (i == keys.Length-1)
				{
					keys[i].inTangent = keys[i].outTangent =
						(keys[i].value - keys[i - 1].value) / (keys[i].time - keys[i - 1].time);
				}
				// set tangents for all other keys
				else
				{
					keys[i].inTangent = keys[i].outTangent = 0.5f * (
						(keys[i + 1].value - keys[i].value) / (keys[i + 1].time - keys[i].time) +
						(keys[i].value - keys[i - 1].value) / (keys[i].time - keys[i - 1].time)
					);
				}
			}
			curve.keys = keys;
		}
		
		/// <summary>
		/// Transform the curve's keys' time values to fit in the specified domain.
		/// </summary>
		/// <param name="curve">Curve.</param>
		/// <param name="min">Minimum time value.</param>
		/// <param name="max">Maximum time value.</param>
		public static void TransformDomain(this AnimationCurve curve, float min, float max)
		{
			float domainScalar = (max - min) / (curve[curve.length - 1].time - curve[0].time);
			float tangentScalar = 1f / domainScalar;
			bool isFlipped = tangentScalar < 0f; // if transformation flips the curve, then swap tangents
			Keyframe[] newKeys = curve.keys;
			for (int i = 0; i < newKeys.Length; ++i)
			{
				newKeys[i].time = min + domainScalar * (curve[i].time - curve[0].time);
				newKeys[i].inTangent = tangentScalar * (isFlipped ? curve[i].outTangent : curve[i].inTangent);
				newKeys[i].outTangent = tangentScalar * (isFlipped ? curve[i].inTangent : curve[i].outTangent);
			}
			curve.keys = newKeys;
		}
		
		/// <summary>
		/// Transforms the curve range.
		/// </summary>
		/// <param name="curve">Curve.</param>
		/// <param name="min">Minimum value.</param>
		/// <param name="max">Maximum value.</param>
		/// <param name="samplingFrequency">
		/// Sampling frequency (<see cref="UnityEngine.Mathf.Epsilon"/>, <see cref="UnityEngine.Mathf.Infinity"/>).
		/// </param>
		public static void TransformRange(this AnimationCurve curve, float min, float max, float samplingFrequency)
		{
			float high, low;
			curve.GetRange(out high, out low, samplingFrequency);
			curve.TransformRange(min, max, low, high);
		}
		
		/// <summary>
		/// Transforms the curve range.
		/// </summary>
		/// <param name="curve">Curve.</param>
		/// <param name="min">Minimum value.</param>
		/// <param name="max">Maximum value.</param>
		/// <param name="low">Low value on the curve.</param>
		/// <param name="high">High value on the curve.</param>
		private static void TransformRange(this AnimationCurve curve, float min, float max, float low, float high)
		{
			float rangeScalar = (max - min) / (high - low);
			Keyframe[] newKeys = curve.keys;
			for (int i = 0; i < newKeys.Length; ++i)
			{
				newKeys[i].value = min + (curve[i].value - low) * rangeScalar;
				newKeys[i].inTangent = rangeScalar * curve[i].inTangent;
				newKeys[i].outTangent = rangeScalar * curve[i].outTangent;
			}
			curve.keys = newKeys;
		}
	}
}