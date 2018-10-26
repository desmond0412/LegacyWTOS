// 
// ArcHandles.cs
// 
// Copyright (c) 2011-2015, Candlelight Interactive, LLC
// All rights reserved.
// 
// This file is licensed according to the terms of the Unity Asset Store EULA:
// http://download.unity3d.com/assetstore/customer-eula.pdf

using UnityEditor;
using UnityEngine;

namespace Candlelight
{
	/// <summary>
	/// A class for creating arc handles.
	/// </summary>
	/// <remarks>
	/// Arc handles will not inherent nonuniform scale from the handles matrix.
	/// </remarks>
	public static class ArcHandles
	{
		/// <summary>
		/// Fill type.
		/// </summary>
		internal enum FillType { Wire, Solid }

		/// <summary>
		/// The radius handle scale.
		/// </summary>
		private static readonly float s_RadiusHandleScale = 0.1f;
		/// <summary>
		/// The smallest change in arc handle's angle that triggers value change.
		/// </summary>
		/// <remarks>
		/// This value is the smallest change in an arc handle's angle that will trigger a value change. If this number
		/// is 0, then the handle risks continually changing a prefab value.
		/// </remarks>
		private static readonly float s_RequiredMinAngleChange = 0.001f;
		
		/// <summary>
		/// Gets the delta angle.
		/// </summary>
		/// <returns>The handle's angle value.</returns>
		/// <returns>The delta angle if greater than minimum required change; otherwise, 0.</returns>
		/// <param name="oldAngle">Old angle.</param>
		/// <param name="newAngle">New angle.</param>
		public static float GetDeltaAngle(float oldAngle, float newAngle)
		{
			float deltaAngle = newAngle - oldAngle;
			// assume huge changes cannot happen - improves multiple rotations
			if (deltaAngle < -180f)
			{
				deltaAngle += 360f;
			}
			if (deltaAngle > 180f)
			{
				deltaAngle -= 360f;
			}
			// return delta if passes threshold, otherwise 0
			if (Mathf.Abs(deltaAngle) > s_RequiredMinAngleChange)
			{
				return deltaAngle;
			}
			else
			{
				return 0f;
			}
		}
		
		/// <summary>
		/// Displays an angle handle.
		/// </summary>
		/// <returns>The handle's angle value.</returns>
		/// <param name="id">Control identifier.</param>
		/// <param name="angle">Angle.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="label">Label.</param>
		/// <param name="fillType">Fill type.</param>
		private static float Angle(
			int id,
			float angle,
			Vector3 origin,
			Quaternion orientation,
			float radius,
			string label,
			FillType fillType
		)
		{
			// set handle matrix
			Matrix4x4 oldMatrix = Handles.matrix;
			Handles.matrix *= Matrix4x4.TRS(origin, orientation, Vector3.one);
			// BUG: Slider2D only works in a matrix with a scale of 1
			float scaleFactor = Handles.matrix.GetScale().GetMaxValue();
			radius *= scaleFactor;
			Handles.matrix = Matrix4x4.TRS(
				Handles.matrix.MultiplyPoint(Vector3.zero),
				Quaternion.LookRotation(
					Handles.matrix.MultiplyVector(Vector3.forward), Handles.matrix.MultiplyVector(Vector3.up)
				),
				Vector3.one
			);
			// create handle
			Vector3 handlePosition = Quaternion.AngleAxis(angle, Vector3.up) *
				Vector3.forward * radius;
			handlePosition = Handles.Slider2D(
				handlePosition, Vector3.right,
				Vector3.forward, Vector3.right,
				SceneGUI.GetFixedHandleSize(handlePosition, SceneGUI.DotHandleSize),
				Handles.DotCap, 0f
			);
			Handles.DrawLine(Vector3.zero, handlePosition);
			float newAngle = Vector3.Angle(
				Vector3.forward, handlePosition
			) * Mathf.Sign(Vector3.Dot(Vector3.right, handlePosition.normalized));
			angle += GetDeltaAngle(angle % 360f, newAngle);
			// fill arc
			if (fillType == FillType.Solid)
			{
				Color c = Handles.color;
				SceneGUI.SetHandleAlpha(c.a * SceneGUI.FillAlphaScalar);
				Handles.DrawSolidArc(Vector3.zero, Vector3.up, Vector3.forward, angle, radius);
				Handles.color = c;
			}
			// draw the label if requested
			if (!string.IsNullOrEmpty(label))
			{
				Handles.Label(Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward * radius, label);
			}
			// reset handle matrix
			Handles.matrix = oldMatrix;
			// return result
			return angle;
		}
		
		/// <summary>
		/// Display a wedge handle for editing a symmetrical angle.
		/// </summary>
		/// <returns>The handle's angle value.</returns>
		/// <param name="id">Control identifier.</param>
		/// <param name="angle">Angle.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="angleLabel">Angle label.</param>
		/// <param name="fillType">Fill type.</param>
		private static float Wedge(
			int id,
			float angle,
			Vector3 origin,
			Quaternion orientation,
			float radius,
			string angleLabel,
			FillType fillType
		)
		{
			float newAngle = angle * 0.5f;
			newAngle = Angle(id, newAngle, origin, orientation, radius, angleLabel, fillType);
			Quaternion otherSide = Quaternion.LookRotation(orientation * Vector3.forward, orientation * Vector3.down);
			newAngle = Angle(id, newAngle, origin, otherSide, radius, angleLabel, fillType);
			angle += GetDeltaAngle(angle % 360f, newAngle * 2f);
			return angle;
		}
		
		/// <summary>
		/// Displays the wedge radius handle.
		/// </summary>
		/// <returns>The handle's angle value.</returns>
		/// <param name="id">Control identifier.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="wedgeCenter">Wedge center.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="label">Label.</param>
		private static float WedgeRadiusHandle(
			int id, float radius, Vector3 wedgeCenter, Vector3 direction, string label
		)
		{
			return LinearHandles.Cone(id, radius, wedgeCenter, direction, label, s_RadiusHandleScale);
		}
		
		/// <summary>
		/// Displays a solid angle handle.
		/// </summary>
		/// <returns>The handle's angle value.</returns>
		/// <param name="id">Control identifier.</param>
		/// <param name="angle">Angle.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="label">Label.</param>
		public static float SolidAngle(
			int id, float angle, Vector3 origin, Quaternion orientation, float radius, string label = ""
		)
		{
			return Angle(id, angle, origin, orientation, radius, label, FillType.Solid);
		}
		
		/// <summary>
		/// Display a solid wedge handle for editing a symmetrical angle.
		/// </summary>
		/// <returns>The handle's angle value.</returns>
		/// <param name="baseId">Base identifier. Each handle has its own unique hash based off this value.</param>
		/// <param name="angle">Angle.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="angleLabel">Angle label.</param>
		public static float SolidWedge(
			int baseId, float angle, Vector3 origin, Quaternion orientation, float radius, string angleLabel = ""
		)
		{
			return Wedge(baseId, angle, origin, orientation, radius, angleLabel, FillType.Solid);
		}

		/// <summary>
		/// Display a solid wedge handle for editing a symmetrical angle and its radius.
		/// </summary>
		/// <returns>The handle's angle value.</returns>
		/// <param name="baseId">Base identifier. Each handle has its own unique hash based off this value.</param>
		/// <param name="angle">Angle.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="angleLabel">Angle label.</param>
		/// <param name="radiusLabel">Radius label.</param>
		public static float SolidWedge(
			int baseId, float angle, Vector3 origin, Quaternion orientation, ref float radius,
			string angleLabel = "", string radiusLabel = ""
		)
		{
			// radius handle
			radius = WedgeRadiusHandle(baseId, radius, origin, orientation * Vector3.forward, radiusLabel);
			// angle handle
			return SolidWedge(baseId, angle, origin, orientation, radius, angleLabel);
		}
		
		/// <summary>
		/// Displays a wire angle handle.
		/// </summary>
		/// <returns>The handle's angle value.</returns>
		/// <param name="id">Control identifier.</param>
		/// <param name="angle">Angle.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="label">Label.</param>
		public static float WireAngle(
			int id, float angle, Vector3 origin, Quaternion orientation, float radius, string label = ""
		)
		{
			return Angle(id, angle, origin, orientation, radius, label, FillType.Wire);
		}
		
		/// <summary>
		/// Display a wire wedge handle for editing a symmetrical angle.
		/// </summary>
		/// <returns>The handle's angle value.</returns>
		/// <param name="baseId">Base identifier. Each handle has its own unique hash based off this value.</param>
		/// <param name="angle">Angle.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="angleLabel">Angle label.</param>
		public static float WireWedge(
			int baseId, float angle, Vector3 origin, Quaternion orientation, float radius, string angleLabel = ""
		)
		{
			return Wedge(baseId, angle, origin, orientation, radius, angleLabel, FillType.Wire);
		}

		/// <summary>
		/// Display a wire wedge handle for editing a symmetrical angle and its radius.
		/// </summary>
		/// <returns>The handle's angle value.</returns>
		/// <param name="baseId">Base identifier. Each handle has its own unique hash based off this value.</param>
		/// <param name="angle">Angle.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="angleLabel">Angle label.</param>
		/// <param name="radiusLabel">Radius label.</param>
		public static float WireWedge(
			int baseId, float angle, Vector3 origin, Quaternion orientation, ref float radius,
			string angleLabel = "", string radiusLabel = ""
		)
		{
			// radius handle
			radius = WedgeRadiusHandle(baseId, radius, origin, orientation * Vector3.forward, radiusLabel);
			// angle handle
			return WireWedge(baseId, angle, origin, orientation, radius, angleLabel);
		}

		#region Obsolete
		[System.Obsolete("Use Candlelight.ArcHandles.SolidAngle(int id, float angle, Vector3 origin, Quaternion orientation, float radius, string label)")]
		public static float SolidAngle(
			float angle, Vector3 origin, Quaternion orientation, float radius, string label = ""
		)
		{
			return SolidAngle(0, angle, origin, orientation, radius, label);
		}
		[System.Obsolete("Use Candlelight.ArcHandles.SolidWedge(int baseId, float angle, Vector3 origin, Quaternion orientation, float radius, string angleLabel)")]
		public static float SolidWedge(
			float angle, Vector3 origin, Quaternion orientation, float radius, string angleLabel = ""
		)
		{
			return SolidWedge(0, angle, origin, orientation, radius, angleLabel);
		}
		[System.Obsolete("Use Candlelight.ArcHandles.SolidWedge(int baseId, float angle, Vector3 origin, Quaternion orientation, ref float radius, string angleLabel, string radiusLabel)")]
		public static float SolidWedge(
			float angle, Vector3 origin, Quaternion orientation, ref float radius,
			string angleLabel = "", string radiusLabel = ""
		)
		{
			return SolidWedge(0, angle, origin, orientation, ref radius, angleLabel, radiusLabel);
		}
		[System.Obsolete("Use Candlelight.ArcHandles.WireAngle(int id, float angle, Vector3 origin, Quaternion orientation, float radius, string label)")]
		public static float WireAngle(
			float angle, Vector3 origin, Quaternion orientation, float radius, string label = ""
		)
		{
			return WireAngle(0, angle, origin, orientation, radius, label);
		}
		[System.Obsolete("Use Candlelight.ArcHandles.WireWedge(int baseId, float angle, Vector3 origin, Quaternion orientation, float radius, string angleLabel)")]
		public static float WireWedge(
			float angle, Vector3 origin, Quaternion orientation, float radius, string angleLabel = ""
		)
		{
			return WireWedge(0, angle, origin, orientation, radius, angleLabel);
		}
		[System.Obsolete("Use Candlelight.ArcHandles.WireWedge(int baseId, float angle, Vector3 origin, Quaternion orientation, ref float radius, string angleLabel, string radiusLabel)")]
		public static float WireWedge(
			float angle, Vector3 origin, Quaternion orientation, ref float radius,
			string angleLabel = "", string radiusLabel = ""
		)
		{
			return WireWedge(0, angle, origin, orientation, ref radius, angleLabel, radiusLabel);
		}
		#endregion
	}
}