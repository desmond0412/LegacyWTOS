// 
// ShapeHandles.cs
// 
// Copyright (c) 2011-2015, Candlelight Interactive, LLC
// All rights reserved.
// 
// This file is licensed according to the terms of the Unity Asset Store EULA:
// http://download.unity3d.com/assetstore/customer-eula.pdf

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Candlelight
{
	/// <summary>
	/// A static class for drawing handles to define collider shapes.
	/// </summary>
	public static class ShapeHandles
	{
		/// <summary>
		/// Different cylinder cap styles.
		/// </summary>
		internal enum CylinderCap { Capsule, Cylinder }
		/// <summary>
		/// Different wire shape types. The value for each is the base hash value.
		/// </summary>
		internal enum WireShapeType
		{
			Box = 491,
			Capsule = 499,
			Cylinder = 503,
			Sphere = 509
		}
		/// <summary>
		/// A cache of the size of a shape handle when it is first clicked.
		/// </summary>
		private static Dictionary<int, Vector3> s_OnClickSize = new Dictionary<int, Vector3>();
		/// <summary>
		/// A cache of the center of a shape handle when it is first clicked.
		/// </summary>
		private static Dictionary<int, Vector3> s_OnClickCenter = new Dictionary<int, Vector3>();

		#region Backing Fields
		private static float s_HandleOffset = 0.01f;
		#endregion
		/// <summary>
		/// Gets or sets the handle offset. This value allows you to overshoot the lines for the shape. It helps prevent
		/// gizmos for built-in colliders from intercepting mouse clicks.
		/// </summary>
		/// <value>The handle offset.</value>
		public static float HandleOffset
		{
			get { return s_HandleOffset; }
			set { s_HandleOffset = value; }
		}
		
		/// <summary>
		/// Displays a shape handle of the specified type.
		/// </summary>
		/// <remarks> If the user is holding Alt, the center will stay locked in place.</remarks>
		/// <param name="baseId">Base identifier.</param>
		/// <param name="size">Size.</param>
		/// <param name="center">Center.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="type">Type.</param>
		private static void DoShapeHandle(
			int baseId, ref Vector3 size, ref Vector3 center, Quaternion orientation, WireShapeType type
		)
		{
			// set handle matrix
			Matrix4x4 oldMatrix = Handles.matrix;
			Handles.matrix *= Matrix4x4.TRS(center, orientation, Vector3.one);
			// generate control Ids
			int rightId = ObjectX.GenerateHashCode(baseId, (int)type, (int)EditAxis.X, 1);
			int leftId = ObjectX.GenerateHashCode(baseId, (int)type, (int)EditAxis.X, 2);
			int upId = ObjectX.GenerateHashCode(baseId, (int)type, (int)EditAxis.Y, 1);
			int downId = ObjectX.GenerateHashCode(baseId, (int)type, (int)EditAxis.Y, 2);
			int forwardId = ObjectX.GenerateHashCode(baseId, (int)type, (int)EditAxis.Z, 1);
			int backId = ObjectX.GenerateHashCode(baseId, (int)type, (int)EditAxis.Z, 2);
			int currentId = GUIUtility.hotControl;
			int nearestId = HandleUtility.nearestControl;
			// cache the center and size when a handle is clicked to use for shift/alt dragging
			switch (Event.current.type)
			{
			case EventType.MouseDown:
				if (
					nearestId == leftId || nearestId == rightId ||
					nearestId == upId || nearestId == downId ||
					nearestId == forwardId || nearestId == backId
				)
				{
					s_OnClickCenter[baseId] = center;
					s_OnClickSize[baseId] = size;
				}
				break;
			case EventType.MouseUp:
				s_OnClickCenter.Remove(baseId);
				s_OnClickSize.Remove(baseId);
				currentId = 0;
				break;
			}
			// make sure size is not negative on any dimension
			size = new Vector3(Mathf.Max(0f, size.x), Mathf.Max(0f, size.y), Mathf.Max(0f, size.z));
			// display right/left handles
			float right = size.x * 0.5f;
			float left = -size.x * 0.5f;
			Vector3 offset = Handles.matrix.GetScale();
			offset = new Vector3(
				offset.x == 0f ? 0f : 1f / offset.x,
				offset.y == 0f ? 0f : 1f / offset.y,
				offset.z == 0f ? 0f : 1f / offset.z
			);
			offset *= s_HandleOffset;
			right = LinearHandles.Dot(
				rightId, val: right, origin: Vector3.right * offset.x, direction: Vector3.right
			);
			right = Mathf.Max(right, left);
			left =
				LinearHandles.Dot(leftId, val: left, origin: Vector3.left * offset.x, direction: Vector3.right);
			left = Mathf.Min(right, left);
			// display up/down handles
			float up = size.y * 0.5f;
			float down = -size.y * 0.5f;
			up = LinearHandles.Dot(upId, val: up, origin: Vector3.up * offset.y, direction: Vector3.up);
			up = Mathf.Max(up, down);
			down = LinearHandles.Dot(downId, val: down, origin: Vector3.down * offset.y, direction: Vector3.up);
			down = Mathf.Min(up, down);
			// display forward/back handles
			float forward = size.z * 0.5f;
			float back = -size.z * 0.5f;
			forward = LinearHandles.Dot(
				forwardId, val: forward, origin: Vector3.forward * offset.z, direction: Vector3.forward
			);
			forward = Mathf.Max(forward, back);
			back =
				LinearHandles.Dot(backId, val: back, origin: Vector3.back * offset.z, direction: Vector3.forward);
			back = Mathf.Min(forward, back);
			// store which axes are being edited
			EditAxis editAxis = EditAxis.None;
			if (currentId == rightId || currentId == leftId)
			{
				editAxis = EditAxis.X;
			}
			else if (currentId == upId || currentId == downId)
			{
				editAxis = EditAxis.Y;
			}
			else if (currentId == forwardId || currentId == backId)
			{
				editAxis = EditAxis.Z;
			}
			// apply constraints to size based on shape type
			size.x = Mathf.Max(0f, right - left);
			size.y = Mathf.Max(0f, up - down);
			size.z = Mathf.Max(0f, forward - back);
			Vector3 deltaCenter = 0.5f * new Vector3(right + left, up + down, forward + back);
			float delta;
			switch (type)
			{
			case WireShapeType.Capsule:
				switch (editAxis)
				{
				case EditAxis.X:
					delta = Mathf.Min(size.x, size.y) - size.x;
					size.x += delta;
					size.z = size.x;
					deltaCenter.x += delta * 0.5f;
					break;
				case EditAxis.Y:
					delta = Mathf.Max(size.y, size.x, size.z) - size.y;
					size.y += delta;
					deltaCenter.y += delta * 0.5f;
					break;
				case EditAxis.Z:
					delta = Mathf.Min(size.z, size.y) - size.z;
					size.z += delta;
					size.x = size.z;
					deltaCenter.z += delta * 0.5f;
					break;
				}
				break;
			case WireShapeType.Cylinder:
				switch (editAxis)
				{
				case EditAxis.X:
					delta = Mathf.Min(size.x, size.y) - size.x;
					size.x += delta;
					size.z = size.x;
					deltaCenter.x += delta * 0.5f;
					break;
				case EditAxis.Z:
					delta = Mathf.Min(size.z, size.y) - size.z;
					size.z += delta;
					size.x = size.z;
					deltaCenter.z += delta * 0.5f;
					break;
				}
				break;
			case WireShapeType.Sphere:
				switch (editAxis)
				{
				case EditAxis.X:
					size.y = size.z = size.x;
					break;
				case EditAxis.Y:
					size.x = size.z = size.y;
					break;
				case EditAxis.Z:
					size.x = size.y = size.z;
					break;
				}
				break;
			}
			// apply new center
			center += orientation * deltaCenter;
			// scale uniformly using cached size if holding shift key
			if (s_OnClickSize.ContainsKey(baseId) && Event.current.shift)
			{
				float scaleFactor = 1f;
				switch (editAxis)
				{
				case EditAxis.X:
					scaleFactor = s_OnClickSize[baseId].x > 0f ? size.x / s_OnClickSize[baseId].x : 0f;
					break;
				case EditAxis.Y:
					scaleFactor = s_OnClickSize[baseId].y > 0f ? size.y / s_OnClickSize[baseId].y : 0f;
					break;
				case EditAxis.Z:
					scaleFactor = s_OnClickSize[baseId].z > 0f ? size.z / s_OnClickSize[baseId].z : 0f;
					break;
				}
				size = s_OnClickSize[baseId] * scaleFactor;
			}
			// use cached center if holding alt key
			if (s_OnClickCenter.ContainsKey(baseId) && Event.current.alt)
			{
				center = s_OnClickCenter[baseId];
			}
			// draw wire shape
			switch (type)
			{
			case WireShapeType.Box:
				DrawWireBox(size);
				break;
			case WireShapeType.Capsule:
				DrawWireCylinder(new CylinderProperties(height: size.y, radius: size.x * 0.5f), CylinderCap.Capsule);
				break;
			case WireShapeType.Cylinder:
				DrawWireCylinder(new CylinderProperties(height: size.y, radius: size.x * 0.5f), CylinderCap.Cylinder);
				break;
			case WireShapeType.Sphere:
				float radius = size.x * 0.5f;
				Handles.DrawWireDisc(Vector3.zero, Vector3.right, radius);
				Handles.DrawWireDisc(Vector3.zero, Vector3.up, radius);
				Handles.DrawWireDisc(Vector3.zero, Vector3.forward, radius);
				// TODO: get this working in non-identity orientations
//				Vector3 nml = Handles.matrix.MultiplyPoint(Vector3.zero) - Camera.current.transform.position;
//				float sqrMagRecip = 1f / nml.sqrMagnitude;
//				float sqrRadius = radius * radius;
//				radius = Mathf.Sqrt(sqrRadius - (sqrRadius * sqrRadius * sqrMagRecip));
//				Handles.DrawWireDisc(
//					Vector3.zero - sqrRadius * nml * sqrMagRecip, Handles.matrix.inverse.MultiplyVector(nml), radius
//				);
				break;
			}
			// reset handle matrix
			Handles.matrix = oldMatrix;
		}

		/// <summary>
		/// Draws a wire box at the origin.
		/// </summary>
		/// <param name="size">
		/// Size.</param>
		public static void DrawWireBox(Vector3 size)
		{
			// find the corners for the shape
			Vector3[] corners = new Vector3[8];
			corners[0] = new Vector3( size.x * 0.5f, size.y * 0.5f, size.z * 0.5f);
			corners[1] = new Vector3( size.x * 0.5f,-size.y * 0.5f, size.z * 0.5f);
			corners[2] = new Vector3( size.x * 0.5f, size.y * 0.5f,-size.z * 0.5f);
			corners[3] = new Vector3( size.x * 0.5f,-size.y * 0.5f,-size.z * 0.5f);
			corners[4] = new Vector3(-size.x * 0.5f,-size.y * 0.5f,-size.z * 0.5f);
			corners[5] = new Vector3(-size.x * 0.5f, size.y * 0.5f, size.z * 0.5f);
			corners[6] = new Vector3(-size.x * 0.5f,-size.y * 0.5f, size.z * 0.5f);
			corners[7] = new Vector3(-size.x * 0.5f, size.y * 0.5f,-size.z * 0.5f);
			// draw the shape
			Handles.DrawLine(corners[0], corners[1]);
			Handles.DrawLine(corners[0], corners[2]);
			Handles.DrawLine(corners[0], corners[5]);
			Handles.DrawLine(corners[1], corners[3]);
			Handles.DrawLine(corners[1], corners[6]);
			Handles.DrawLine(corners[2], corners[3]);
			Handles.DrawLine(corners[2], corners[7]);
			Handles.DrawLine(corners[3], corners[4]);
			Handles.DrawLine(corners[4], corners[6]);
			Handles.DrawLine(corners[4], corners[7]);
			Handles.DrawLine(corners[5], corners[6]);
			Handles.DrawLine(corners[5], corners[7]);
		}

		/// <summary>
		/// Draws a wire cylinder or capsule at the origin.
		/// </summary>
		/// <returns>The cylinder properties.</returns>
		/// <param name="radius">Radius.</param>
		/// <param name="height">Height.</param>
		/// <param name="center">Center.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="label">Label.</param>
		/// <param name="color">Color.</param>
		/// <param name="cap">Specifies whether capsule tips should be drawn.</param>
		private static void DrawWireCylinder(CylinderProperties properties, CylinderCap cap)
		{
			// clamp values
			float radius = properties.Radius;
			float height = properties.Height;
			// compute disc locations
			Vector3 right = Vector3.right * radius;
			Vector3 up = Vector3.up * height * 0.5f;
			Vector3 forward = Vector3.forward * radius;
			// draw connecting lines for height
			float discHandleOffset = cap == CylinderCap.Capsule ? radius : 0f;
			Vector3 upperPoint = Vector3.up * (height * 0.5f - discHandleOffset);
			Vector3 lowerPoint = Vector3.up * (height * -0.5f + discHandleOffset);
			Handles.DrawLine(upperPoint + forward, lowerPoint + forward);
			Handles.DrawLine(upperPoint - forward, lowerPoint - forward);
			Handles.DrawLine(upperPoint + right, lowerPoint + right);
			Handles.DrawLine(upperPoint - right, lowerPoint - right);
			// draw end caps for radius
			Handles.DrawWireDisc(upperPoint, Vector3.up, radius);
			Handles.DrawWireDisc(lowerPoint, Vector3.up, radius);
			// draw capsule caps
			if (cap == CylinderCap.Capsule)
			{
				Handles.DrawWireArc(-up + up.normalized * radius, right, forward, 180f, radius);
				Handles.DrawWireArc(-up + up.normalized * radius, forward, right, -180f, radius);
				Handles.DrawWireArc(up - up.normalized * radius, right, forward, -180f, radius);
				Handles.DrawWireArc(up - up.normalized * radius, forward, right, 180f, radius);
			}
		}

		/// <summary>
		/// Displays a wire box handle. Hold Shift to scale proportionally, hold alt to lock the center.
		/// </summary>
		/// <remarks>
		/// To prevent interfering with BoxCollider gizmos in the scene, the handles are drawn with a slight offset from
		/// the shape.
		/// </remarks>
		/// <param name="baseId">Base identifier. Each handle has its own unique hash based off this value.</param>
		/// <param name="size">Size.</param>
		/// <param name="center">Center.</param>
		/// <param name="orientation">Orientation.</param>
		public static void WireBox(int baseId, ref Vector3 size, ref Vector3 center, Quaternion orientation)
		{
			DoShapeHandle(baseId, ref size, ref center, orientation, WireShapeType.Box);
		}
		
		/// <summary>
		/// Displays a wire capsule handle with its height oriented along the y-axis. Hold Shift to scale
		/// proportionally, hold alt to lock the center.
		/// </summary>
		/// <remarks>
		/// <para>This handle improves upon Unity's built-in one by simultaneously editing the center and size, much
		/// like the built-in box collider editor.</para>
		/// <para>To prevent interfering with CapsuleCollider gizmos in the scene, the handles are drawn with a slight
		/// offset from the shape.</para>
		/// </remarks>
		/// <param name="baseId">Base identifier. Each handle has its own unique hash based off this value.</param>
		/// <param name="properties"><see cref="CylinderProperties"/> describing the size of the capsule.</param>
		/// <param name="center">Center.</param>
		/// <param name="orientation">Orientation.</param>
		public static void WireCapsule(
			int baseId, ref CylinderProperties properties, ref Vector3 center, Quaternion orientation
		)
		{
			float diameter = Mathf.Min(properties.Radius * 2f, properties.Height);
			Vector3 size = new Vector3(diameter, properties.Height, diameter);
			DoShapeHandle(baseId, ref size, ref center, orientation, WireShapeType.Capsule);
			diameter = Mathf.Min(size.y, size.x);
			properties = new CylinderProperties(height: size.y, radius: diameter * 0.5f);
		}
		
		/// <summary>
		/// Displays a wire cylinder handle with its height oriented along the y-axis. Hold Shift to scale
		/// proportionally, hold alt to lock the center.
		/// </summary>
		/// <para>To prevent interfering with any possible gizmos in the scene, the handles are drawn with a slight
		/// offset from the shape.</para>
		/// <param name="baseId">Base identifier. Each handle has its own unique hash based off this value.</param>
		/// <param name="properties"><see cref="CylinderProperties"/> describing the size of the cylinder.</param>
		/// <param name="center">Center.</param>
		/// <param name="orientation">Orientation.</param>
		public static void WireCylinder(
			int baseId, ref CylinderProperties properties, ref Vector3 center, Quaternion orientation
		)
		{
			Vector3 size = new Vector3(properties.Radius * 2f, properties.Height, properties.Radius * 2f);
			DoShapeHandle(baseId, ref size, ref center, orientation, WireShapeType.Cylinder);
			properties = new CylinderProperties(height: size.y, radius: size.x * 0.5f);
		}

		/// <summary>
		/// Displays a wire sphere handle. Hold Shift to scale proportionally, hold alt to lock the center.
		/// </summary>
		/// <para>This handle improves upon Unity's built-in one by simultaneously editing the center and size, much
		/// like the built-in box collider editor, while also being compatible with different handle matrices.</para>
		/// <para>To prevent interfering with SphereCollider gizmos in the scene, the handles are drawn with a slight
		/// offset from the shape.</para>
		/// <param name="baseId">Base identifier. Each handle has its own unique hash based off this value.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="center">Center.</param>
		/// <param name="orientation">Orientation.</param>
		public static void WireSphere(int baseId, ref float radius, ref Vector3 center, Quaternion orientation)
		{
			Vector3 size = Vector3.one * radius * 2f;
			DoShapeHandle(baseId, ref size, ref center, orientation, WireShapeType.Sphere);
			radius = 0.5f * (size.x);
		}

		#region Obsolete
		[System.Obsolete("Use Candlelight.ShapeHandles.WireBox(int baseId, ref Vector3 size, ref Vector3 center, Quaternion orientation)")]
		public static Vector3 WireBox(
			Vector3 size, Vector3 center, Quaternion orientation, string label = "", int baseId = 0
		)
		{
			WireBox(baseId, ref size, ref center, orientation);
			return size;
		}
		[System.Obsolete("Use Candlelight.ShapeHandles.WireCapsule(int baseId, ref CylinderProperties properties, ref Vector3 center, Quaternion orientation)")]
		public static CylinderProperties WireCapsule(
			CylinderProperties properties, Vector3 center, Quaternion orientation, string label = "", int baseId = 0
		)
		{
			WireCapsule(baseId, ref properties, ref center, orientation);
			return properties;
		}
		[System.Obsolete("Use Candlelight.ShapeHandles.WireCylinder(int baseId, ref CylinderProperties properties, ref Vector3 center, Quaternion orientation)")]
		public static CylinderProperties WireCylinder(
			CylinderProperties properties, Vector3 center, Quaternion orientation, string label = "", int baseId = 0
		)
		{
			WireCylinder(baseId, ref properties, ref center, orientation);
			return properties;
		}
		#endregion
	}
}