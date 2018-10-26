// 
// TransformHandles.cs
// 
// Copyright (c) 2015, Candlelight Interactive, LLC
// All rights reserved.
// 
// This file is licensed according to the terms of the Unity Asset Store EULA:
// http://download.unity3d.com/assetstore/customer-eula.pdf
// 
// This file contains a static class with functions for drawing transform
// handles. It primarily exists to add functionality or address bugs in Unity's
// transform handles.

using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Reflection;

namespace Candlelight
{
	/// <summary>
	/// Different possible axes to edit. The value for each is the base hash value.
	/// </summary>
	public enum EditAxis
	{
		X = 211,
		Y = 223,
		Z = 227,
		None = 229
	}

	/// <summary>
	/// Transform handles.
	/// </summary>
	public static class TransformHandles
	{
		#region Base Hash Codes
		private static readonly int s_TranslationHandleHash = 37;
		#endregion

		/// <summary>
		/// Displays a rotation handle at the specified location that works with non-identity handle matrices.
		/// </summary>
		/// <param name="rotation">Rotation.</param>
		/// <param name="position">Position.</param>
		public static Quaternion Rotation(Quaternion rotation, Vector3 position)
		{
			Matrix4x4 oldMatrix = Handles.matrix;
			Quaternion oldMatrixOrientation = Quaternion.LookRotation(
				oldMatrix.MultiplyVector(Vector3.forward), oldMatrix.MultiplyVector(Vector3.up)
			);
			rotation = oldMatrixOrientation * rotation;
			Handles.matrix = Matrix4x4.identity; // NOTE: For some reason Unity doesn't like this in other matrices...
			rotation = Handles.RotationHandle(rotation, oldMatrix.MultiplyPoint(position));
			Handles.matrix = oldMatrix;
			return Quaternion.Inverse(oldMatrixOrientation) * rotation;
		}

		/// <summary>
		/// Displays a translation handle at the specified location with a customizable size and control ID that also
		/// respects the current scene GUI alpha.
		/// </summary>
		/// <remarks>
		/// Unity's built-in translation handle does not allow size specification, always renders fully opaque, and will
		/// result in control ID conflicts if there are multiple instances on the screen at once.
		/// </remarks>
		/// <param name="baseId">Base identifier. Each axis its own unique hash based off this value.</param>
		/// <param name="position">Position.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="size">Size.</param>
		public static Vector3 Translation(int baseId, Vector3 position, Quaternion orientation, float size)
		{
			Color oldColor = Handles.color;
			size = SceneGUI.GetFixedHandleSize(position, size);
			Handles.color = EditorGUIX.xHandleColor * SceneGUI.CurrentAlphaScalar;
			GUI.SetNextControlName(
				ObjectX.GenerateHashCode(baseId, s_TranslationHandleHash, (int)EditAxis.X).ToString()
			);
			position = Handles.Slider(position, orientation * Vector3.right, size, Handles.ArrowCap, 1f);
			Handles.color = EditorGUIX.yHandleColor * SceneGUI.CurrentAlphaScalar;
			GUI.SetNextControlName(
				ObjectX.GenerateHashCode(baseId, s_TranslationHandleHash, (int)EditAxis.Y).ToString()
			);
			position = Handles.Slider(position, orientation * Vector3.up, size, Handles.ArrowCap, 1f);
			Handles.color = EditorGUIX.zHandleColor * SceneGUI.CurrentAlphaScalar;
			GUI.SetNextControlName(
				ObjectX.GenerateHashCode(baseId, s_TranslationHandleHash, (int)EditAxis.Z).ToString()
			);
			position = Handles.Slider(position, orientation * Vector3.forward, size, Handles.ArrowCap, 1f);
			// TODO: add 2-axis sliders
//			position = Handles.DoPlanarHandle(Handles.PlaneHandle.xzPlane, position, orientation, size * 0.25f);
//			position = Handles.DoPlanarHandle(Handles.PlaneHandle.xyPlane, position, orientation, size * 0.25f);
//			position = Handles.DoPlanarHandle(Handles.PlaneHandle.yzPlane, position, orientation, size * 0.25f);
			Handles.color = oldColor;
			return position;
		}

		#region Obsolete
		[System.Obsolete("Use (int)Candlelight.EditAxis.X")]
		public static int XHandleHash { get { return (int)EditAxis.X; } }
		[System.Obsolete("Use (int)Candlelight.EditAxis.Y")]
		public static int YHandleHash { get { return (int)EditAxis.Y; } }
		[System.Obsolete("Use (int)Candlelight.EditAxis.Z")]
		public static int ZHandleHash { get { return (int)EditAxis.Z; } }
		[System.Obsolete("Use Candlelight.TransformHandles.Translation(int baseId, Vector3 position, Quaternion orientation, float size)")]
		public static Vector3 Translation(Vector3 position, Quaternion orientation, float size, int baseId)
		{
			return Translation(baseId, position, orientation, size);
		}
		#endregion
	}
}