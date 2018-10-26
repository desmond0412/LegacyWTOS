// 
// JointX.cs
// 
// Copyright (c) 2011-2016, Candlelight Interactive, LLC
// All rights reserved.
// 
// This file is licensed according to the terms of the Unity Asset Store EULA:
// http://download.unity3d.com/assetstore/customer-eula.pdf

using UnityEngine;
using System.Collections.Generic;

namespace Candlelight.Physics
{
	/// <summary>
	/// Extension methods for <see cref="UnityEngine.Joint"/> and its subclasses.
	/// </summary>
	public static class JointX
	{
		/// <summary>
		/// Gets or sets a value indicating whether this class should emit debug messages. This value should only be set
		/// to <see langword="true"/> when performing automated tests.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if this class will emit no debug messages; otherwise, <see langword="false"/>.
		/// </value>
		public static bool IsSilent { get; set; }

		/// <summary>
		/// Gets the actual primary, secondary, and tertiary axes, post correction.
		/// </summary>
		/// <returns>The actual primary, secondary, and tertiary axes, post correction.</returns>
		/// <param name="joint">A <see cref="UnityEngine.CharacterJoint"/>.</param>
		public static Vector3[] GetActualJointAxes(this CharacterJoint joint)
		{
			Vector3[] axes = new Vector3[3];
			GetActualJointAxes(joint.axis, joint.swingAxis, out axes[0], out axes[1], out axes[2]);
			return axes;
		}

		/// <summary>
		/// Gets the actual primary, secondary, and tertiary axes, post correction.
		/// </summary>
		/// <returns>The actual primary, secondary, and tertiary axes, post correction.</returns>
		/// <param name="joint">A <see cref="UnityEngine.ConfigurableJoint"/>.</param>
		public static Vector3[] GetActualJointAxes(this ConfigurableJoint joint)
		{
			Vector3[] axes = new Vector3[3];
			GetActualJointAxes(joint.axis, joint.secondaryAxis, out axes[0], out axes[1], out axes[2]);
			return axes;
		}

		/// <summary>
		/// Gets the actual primary, secondary, and tertiary axes, post correction.
		/// </summary>
		/// <param name="axis">Input axis.</param>
		/// <param name="secondaryAxis">Input secondary axis.</param>
		/// <param name="actualAxis">Actual orthonormalized axis.</param>
		/// <param name="actualSecondaryAxis">Actual orthonormalized secondary axis.</param>
		/// <param name="actualTertiaryAxis">Actual orthonormalized tertiary axis.</param>
		public static void GetActualJointAxes(
			Vector3 axis, Vector3 secondaryAxis,
			out Vector3 actualAxis, out Vector3 actualSecondaryAxis, out Vector3 actualTertiaryAxis
		)
		{
			// ConfigurableJoint defaults to Vector3.right if axis is Vector3.zero
			actualAxis = Mathf.Approximately(axis.sqrMagnitude, 0f) ? Vector3.right : axis;
			actualSecondaryAxis = secondaryAxis;
			actualTertiaryAxis = secondaryAxis;
			// orthonormalize axes
			Vector3.OrthoNormalize(ref actualAxis, ref actualSecondaryAxis, ref actualTertiaryAxis);
		}

		/// <summary>
		/// Gets the bindpose local matrix.
		/// </summary>
		/// <returns>The bindpose local matrix.</returns>
		/// <param name="joint">A <see cref="UnityEngine.Joint"/>.</param>
		public static Matrix4x4 GetBindposeLocalMatrix(this Joint joint)
		{
			if (Application.isPlaying && !JointX.IsSilent)
			{
				Debug.LogWarning(
					string.Format("GetBindPoseLocalMatrix() assumes {0} is currently in its bindpose.", joint)
				);
			}
			return GetParentWorldMatrix(joint).inverse * Matrix4x4.TRS(
				joint.transform.TransformPoint(joint.anchor),
				joint is ConfigurableJoint && (joint as ConfigurableJoint).configuredInWorldSpace ?
					Quaternion.identity : joint.transform.rotation,
				Vector3.one
			);
		}

		/// <summary>
		/// Gets the joint's angular world matrix.
		/// </summary>
		/// <returns>The joint's angular world matrix.</returns>
		/// <param name="joint">Joint.</param>
		/// <param name="bindPose">Bind pose.</param>
		public static Matrix4x4 GetJointAngularWorldMatrix(this Joint joint, Matrix4x4 bindPose)
		{
			// lock scale
			bindPose = Matrix4x4.TRS(
				bindPose.MultiplyPoint3x4(Vector3.zero),
				Quaternion.LookRotation(bindPose.MultiplyVector(Vector3.forward), bindPose.MultiplyVector(Vector3.up)),
				Vector3.one
			);
			return GetParentWorldMatrix(joint) * bindPose;
		}

		/// <summary>
		/// Gets the joint's linear world matrix.
		/// </summary>
		/// <returns>The joint's linear world matrix.</returns>
		/// <param name="joint">Joint.</param>
		/// <param name="bindPose">Bind pose.</param>
		public static Matrix4x4 GetJointLinearWorldMatrix(this Joint joint, Matrix4x4 bindPose)
		{
			// lock orientation and scale
			bindPose = Matrix4x4.TRS(
				bindPose.MultiplyPoint3x4(Vector3.zero),
				Quaternion.LookRotation(bindPose.MultiplyVector(Vector3.forward), bindPose.MultiplyVector(Vector3.up)),
				Vector3.one
			);
			return GetParentWorldMatrix(joint) * bindPose;
		}
		
		/// <summary>
		/// Gets the joint frame offset (the transformation from the joint's frame into the space of its axes).
		/// </summary>
		/// <returns>The joint frame offset.</returns>
		/// <param name="joint">Joint.</param>
		public static Quaternion GetJointFrameOffset(this CharacterJoint joint)
		{
			return GetJointFrameOffset(joint.axis, joint.swingAxis);
		}

		/// <summary>
		/// Gets the joint frame offset (the transformation from the joint's frame into the space of its axes).
		/// </summary>
		/// <returns>The joint frame offset.</returns>
		/// <param name="joint">Joint.</param>
		public static Quaternion GetJointFrameOffset(this ConfigurableJoint joint)
		{
			return GetJointFrameOffset(joint.axis, joint.secondaryAxis);
		}

		/// <summary>
		/// Gets the joint frame offset (the transformation from the joint's frame into the space of its axes).
		/// </summary>
		/// <returns>The joint frame offset.</returns>
		/// <param name="axis">Axis.</param>
		/// <param name="secondaryAxis">Secondary axis.</param>
		private static Quaternion GetJointFrameOffset(Vector3 axis, Vector3 secondaryAxis)
		{
			Vector3 workingAxis, workingSecondaryAxis, workingTertiaryAxis;
			GetActualJointAxes(
				axis, secondaryAxis,
				out workingAxis, out workingSecondaryAxis, out workingTertiaryAxis
			);
			return Quaternion.LookRotation(workingTertiaryAxis, workingSecondaryAxis);
		}
		
		/// <summary>
		/// Gets the parent world matrix.
		/// </summary>
		/// <returns>The parent world matrix.</returns>
		/// <param name="joint">Joint.</param>
		private static Matrix4x4 GetParentWorldMatrix(this Joint joint)
		{
			return (joint.connectedBody != null) ?
				Matrix4x4.TRS(
					joint.connectedBody.transform.position, joint.connectedBody.transform.rotation, Vector3.one
				) : (
					joint.transform.parent != null ? joint.transform.parent.localToWorldMatrix : Matrix4x4.identity
				);
		}

		/// <summary>
		/// Sets all angular motion properties on the joint.
		/// </summary>
		/// <param name="joint">Joint.</param>
		/// <param name="motion">Motion.</param>
		public static void SetAllAngularMotion(this ConfigurableJoint joint, ConfigurableJointMotion motion)
		{
			joint.angularXMotion = joint.angularYMotion = joint.angularZMotion = motion;
		}
		
		/// <summary>
		/// Sets all translation motion properties on the joint.
		/// </summary>
		/// <param name="joint">Joint.</param>
		/// <param name="motion">Motion.</param>
		public static void SetAllTranslationMotion(this ConfigurableJoint joint, ConfigurableJointMotion motion)
		{
			joint.xMotion = joint.yMotion = joint.zMotion = motion;
		}

		/// <summary>
		/// Transform a supplied desired localRotation into the joint's frame and apply the result as
		/// <see cref="UnityEngine.ConfigurableJoint.targetRotation"/> on the joint.
		/// </summary>
		/// <param name="joint">Joint.</param>
		/// <param name="bindPose">
		/// A <see cref="UnityEngine.Quaternion"/> representing orientation of the joint at
		/// <see cref="UnityEngine.MonoBehaviour.Awake()"/> time or when joint was added.
		/// </param>
		/// <param name="jointFrame">
		/// A <see cref="UnityEngine.Quaternion"/> to get from identity into the joint's frame.
		/// </param>
		/// <param name="targetLocalRotation">
		/// A <see cref="UnityEngine.Quaternion"/> representing the desired localRotation for the transform with the
		/// joint attached.
		/// </param>
		public static void SetJointTargetRotationUsingLocalRotation(
			this ConfigurableJoint joint, Quaternion bindPose, Quaternion jointFrame, Quaternion targetLocalRotation
		)
		{
			Quaternion offset = Quaternion.Inverse(targetLocalRotation) * bindPose;
			joint.targetRotation = Quaternion.Inverse(jointFrame) * offset * jointFrame;
		}
	}
}