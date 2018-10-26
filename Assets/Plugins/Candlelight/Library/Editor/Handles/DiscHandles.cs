// 
// DiscHandles.cs
// 
// Copyright (c) 2011-2015, Candlelight Interactive, LLC
// All rights reserved.
// 
// This file is licensed according to the terms of the Unity Asset Store EULA:
// http://download.unity3d.com/assetstore/customer-eula.pdf
// 
// This file contains a static class with methods for creating disc handles.

using UnityEditor;
using UnityEngine;

namespace Candlelight
{
	/// <summary>
	/// Disc handles.
	/// </summary>
	public static class DiscHandles
	{
		/// <summary>
		/// Fill mode.
		/// </summary>
		internal enum FillMode { Wire, Solid };

		#region Base Hash Codes
		private static readonly int s_SolidDiscHash = 53;
		private static readonly int s_SolidSphereHash = 57;
		private static readonly int s_WireDiscHash = 59;
		private static readonly int s_WireSphereHash = 61;
		#endregion
		
		/// <summary>
		/// Displays a disc handle.
		/// </summary>
		/// <returns>
		/// The disc radius.
		/// </returns>
		/// <param name="id">Control identifier.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="label">Label.</param>
		/// <param name="fillMode">Fill mode.</param>
		private static float DoDisc(
			int id, float radius, Vector3 origin, Quaternion orientation, string label, FillMode fillMode
		)
		{
			// set handle matrix
			Matrix4x4 oldMatrix = Handles.matrix;
			Handles.matrix *= Matrix4x4.TRS(origin, orientation, Vector3.one);
			// create handles
			radius =
				LinearHandles.Dot(id, val: radius, origin: Vector3.zero, direction: Vector3.forward, label: label);
			radius = LinearHandles.Dot(id, val: radius, origin: Vector3.zero, direction: Vector3.right);
			radius = LinearHandles.Dot(id, val: radius, origin: Vector3.zero, direction: Vector3.back);
			radius = LinearHandles.Dot(id, val: radius, origin: Vector3.zero, direction: Vector3.left);
			// draw disc
			Color color = Handles.color;
			SceneGUI.SetHandleAlpha(color.a * SceneGUI.LineAlphaScalar);
			Handles.DrawWireDisc(Vector3.zero, Vector3.up, radius);
			// optionally fill the disc
			if (fillMode == FillMode.Solid)
			{
				SceneGUI.SetHandleAlpha(color.a * SceneGUI.FillAlphaScalar);
				Handles.DrawSolidDisc(Vector3.zero, Vector3.up, radius);
			}
			// reset color
			Handles.color = color;
			// reset handle matrix
			Handles.matrix = oldMatrix;
			// return the result
			return radius;
		}
		
		/// <summary>
		/// Displays a sphere handle.
		/// </summary>
		/// <returns>The sphere radius.</returns>
		/// <param name="id">Control identifier.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="fillMode">Fill mode.</param>
		/// <param name="label">Label.</param>
		private static float DoSphere(int id, float radius, Vector3 origin, string label, FillMode fillMode)
		{
			Quaternion orientation = GetScreenSpaceOrientationForDisc(origin);
			return DoDisc(id, radius, origin, orientation, label, fillMode);
		}
		
		/// <summary>
		/// Gets the screen space orientation for a disc.
		/// </summary>
		/// <remarks>
		/// Return value represents a disc oriented to the screen (its normal is screen-facing, its forward is up).
		/// </remarks>
		/// <returns>The screen space orientation for a disc.</returns>
		/// <param name="worldPosition">World position.</param>
		public static Quaternion GetScreenSpaceOrientationForDisc(Vector3 worldPosition)
		{
			// world position in screen space
			Vector2 p = HandleUtility.WorldToGUIPoint(worldPosition);
			// up orientation is is a ray form the camera through the transform
			Vector3 up = -Camera.current.ScreenPointToRay(p).direction;
			// forward orientation is straight up from the transform position
			p.y += 10f;
			Vector3 forward = Camera.current.ScreenToWorldPoint(
				new Vector3(p.x, p.y, Camera.current.farClipPlane)
			) - worldPosition;
			return Quaternion.LookRotation(up, forward) * Quaternion.AngleAxis(90f, Vector3.left);
		}

		/// <summary>
		/// Displays a solid disc handle.
		/// </summary>
		/// <returns>The disc radius.</returns>
		/// <param name="id">Control identifier.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="label">Label.</param>
		public static float SolidDisc(int id, float radius, Vector3 origin, Quaternion orientation, string label = "")
		{
			return DoDisc(
				ObjectX.GenerateHashCode(id, s_SolidDiscHash), radius, origin, orientation, label, FillMode.Solid
			);
		}
		
		/// <summary>
		/// Displays a solid sphere handle.
		/// </summary>
		/// <returns>The sphere radius.</returns>
		/// <param name="id">Control identifier.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="label">Label.</param>
		public static float SolidSphere(int id, float radius, Vector3 origin, string label = "")
		{
			return DoSphere(ObjectX.GenerateHashCode(id, s_SolidSphereHash), radius, origin, label, FillMode.Solid);
		}
		
		/// <summary>
		/// Displays a wire disc handle.
		/// </summary>
		/// <returns>The disc radius.</returns>
		/// <param name="id">Control identifier.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="label">Label.</param>
		public static float WireDisc(int id, float radius, Vector3 origin, Quaternion orientation, string label = "")
		{
			return DoDisc(
				ObjectX.GenerateHashCode(id, s_WireDiscHash), radius, origin, orientation, label, FillMode.Wire
			);
		}
		
		/// <summary>
		/// Displays a wire sphere handle.
		/// </summary>
		/// <returns>The sphere radius.</returns>
		/// <param name="id">Control identifier.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="label">Label.</param>
		public static float WireSphere(int id, float radius, Vector3 origin, string label = "")
		{
			return DoSphere(ObjectX.GenerateHashCode(id, s_WireSphereHash), radius, origin, label, FillMode.Wire);
		}

		#region Obsolete
		[System.Obsolete("Use Candlelight.DiscHandles.SolidDisc(int id, float radius, Vector3 origin, Quaternion orientation, string label)")]
		public static float SolidDisc(
			float radius, Vector3 origin, Quaternion orientation, string label = "", int id = 0
		)
		{
			return SolidDisc(id, radius, origin, orientation, label);
		}
		[System.Obsolete("Use Candlelight.DiscHandles.SolidSphere(int id, float radius, Vector3 origin, string label)")]
		public static float SolidSphere(float radius, Vector3 origin, string label = "", int id = 0)
		{
			return SolidSphere(id, radius, origin, label);
		}
		[System.Obsolete("Use Candlelight.DiscHandles.WireDisc(int id, float radius, Vector3 origin, Quaternion orientation, string label)")]
		public static float WireDisc(
			float radius, Vector3 origin, Quaternion orientation, string label = "", int id = 0
		)
		{
			return WireDisc(id, radius, origin, orientation, label);
		}
		[System.Obsolete("Use Candlelight.DiscHandles.WireSphere(int id, float radius, Vector3 origin, string label)")]
		public static float WireSphere(float radius, Vector3 origin, string label = "", int id = 0)
		{
			return WireSphere(id, radius, origin, label);
		}
		#endregion
	}
}