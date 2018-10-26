// 
// HelixHandles.cs
// 
// Copyright (c) 2011-2015, Candlelight Interactive, LLC
// All rights reserved.
// 
// This file is licensed according to the terms of the Unity Asset Store EULA:
// http://download.unity3d.com/assetstore/customer-eula.pdf
// 
// This file contains a static class with methods for creating helix handles.
// 
// Helix handles currently produce strange behavior when dragging keyframes
// past each other along the domain of an AnimationCurve, as control IDs are
// not deterministic.

using UnityEditor;
using UnityEngine;

namespace Candlelight
{
	/// <summary>
	/// Helix handles.
	/// </summary>
	public static class HelixHandles
	{
		/// <summary>
		/// Shape mode.
		/// </summary>
		internal enum ShapeMode { Helix, Ribbon };
		/// <summary>
		/// Fill mode.
		/// </summary>
		internal enum FillMode { Wire, Solid, Mesh };

		#region Base Hash Codes
		private static readonly int s_LengthHandleHash = 101;
		private static readonly int s_TwistHandleHash = 103;
		private static readonly int s_WidthHandleHash = 107;
		#endregion
		/// <summary>
		/// The fill mesh.
		/// </summary>
		private static Mesh s_FillMesh = null;
		/// <summary>
		/// Length handle size.
		/// </summary>
		private static readonly float s_LengthHandleSize = 0.5f;
		/// <summary>
		/// Required minimum handle change.
		/// </summary>
		/// <remarks>
		/// This value represents the smallest change in a handle's 3D position that will trigger a value change. If
		/// this number is 0, then many linear handles must transform along a cardinal axis or risk continually
		/// changing a prefab value.
		/// </remarks>
		private static readonly float s_RequiredMinHandleChange = 0.00001f;
		/// <summary>
		/// Smooth division count.
		/// </summary>
		private static readonly int s_SmoothDivisionCount = 128;
		/// <summary>
		/// Time adjust handle size.
		/// </summary>
		private static readonly float s_TimeAdjustHandleSize = 0.075f;
		/// <summary>
		/// Width handle offset.
		/// </summary>
		private static readonly float s_WidthHandleOffset = 0.1f;
		/// <summary>
		/// Width handle size.
		/// </summary>
		private static readonly float s_WidthHandleSize = 0.1f;

		/// <summary>
		/// Display a helix handle.
		/// </summary>
		/// <returns>A new helix with any modifications applied.</returns>
		/// <param name="baseId">Base identifier. Each handle has its own unique hash based off this value.</param>
		/// <param name="helix">Helix.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="scale">Scale.</param>
		/// <param name="color">Color.</param>
		/// <param name="divisions">Divisions.</param>
		/// <param name="fillMode">Fill mode.</param>
		/// <param name="shapeMode">Shape mode.</param>
		private static Helix DoHelixHandle(
			int baseId,
			Helix helix,
			Vector3 origin,
			Quaternion orientation,
			Vector3 scale,
			Color color,
			int divisions,
			FillMode fillMode,
			ShapeMode shapeMode
		)
		{
			// early out if the scale is too small
			if (scale.sqrMagnitude < s_RequiredMinHandleChange)
			{
				Debug.LogWarning("Scale vector for helix handle is too close to 0. Handle is disabled");
				return null;
			}
			// create copy of helix
			helix = new Helix(helix);
			// store the handle matrix
			Matrix4x4 oldMatrix = Handles.matrix;
			Handles.matrix *= Matrix4x4.TRS(origin, orientation, scale);
			// set the helix handle's color
			Color oldColor = Handles.color;
			Handles.color = color;
			// validate the number of divisions
			divisions = Mathf.Max(1, divisions);
			// create a handle to adjust length
			DoHelixLengthHandle(baseId, helix);
			// next, create handles for the twist curve
			DoHelixTwistHandles(baseId, helix);
			// next, create handles for the width curve
			DoHelixWidthHandles(baseId, helix);
			// draw lines to visualize the helix
			DrawHelix(helix, divisions, fillMode, shapeMode);
			// reset handle color
			Handles.color = oldColor;
			// reset handle matrix
			Handles.matrix = oldMatrix;
			// return result
			return helix;
		}
		
		/// <summary>
		/// Display a helix length handle.
		/// </summary>
		/// <param name="baseId">Base identifier.</param>
		/// <param name="helix">Helix.</param>
		private static void DoHelixLengthHandle(int baseId, Helix helix)
		{
			float length = helix.Length;
			int id = ObjectX.GenerateHashCode(baseId, s_LengthHandleHash);
			length = LinearHandles.Arrow(
				id,
				length,
				Vector3.zero,
				Vector3.forward,
				string.Format("Length: {0:0.###}", length),
				s_LengthHandleSize
			);
			if (Mathf.Abs(length - helix.Length) < s_RequiredMinHandleChange)
			{
				length = helix.Length;
			}
			helix.Length = length;
		}
		
		/// <summary>
		/// Displays the helix twist curve handles.
		/// </summary>
		/// <param name="baseId">Base identifier.</param>
		/// <param name="helix">Helix.</param>
		private static void DoHelixTwistHandles(int baseId, Helix helix)
		{
			float v;
			AnimationCurve width = helix.Width;
			AnimationCurve newTwist = new AnimationCurve(helix.Twist.keys);
			Keyframe[] newKeys = newTwist.keys;
			for (int i = 0; i < newKeys.Length; ++i)
			{
				// compute the disc center
				Vector3 discCenter = Vector3.forward * newKeys[i].time * helix.Length;
				v = newKeys[i].value;
				// create an arc handle
				float helixWidth = width.Evaluate(newKeys[i].time);
				v = ArcHandles.SolidAngle(
					ObjectX.GenerateHashCode(baseId, s_TwistHandleHash, i, 1),
					v, discCenter,
					Quaternion.LookRotation(Vector3.right, Vector3.forward),
					helixWidth,
					string.Format("{0:0} Degrees", v)
				);
				Handles.DrawWireDisc(discCenter, Vector3.forward, helixWidth);
				newKeys[i].value = v;
				// create time adjustment handles for intermediary keys
				if (i ==0 || i == newKeys.Length - 1)
				{
					continue;
				}
				v = newKeys[i].time * helix.Length;
				int id = ObjectX.GenerateHashCode(baseId, s_TwistHandleHash, i, 2);
				v = LinearHandles.Cone(id, v, Vector3.zero, Vector3.forward, capScale: s_TimeAdjustHandleSize);
				v = Mathf.Clamp(
					v / helix.Length,
					Mathf.Epsilon * 2f,
					1f - Mathf.Epsilon * 2f
				);
				if (Mathf.Abs(v-newKeys[i].time) > s_RequiredMinHandleChange)
				{
					newKeys[i].time = v;
				}
				// TODO: Fix problems with keys moving over each other
			}
			newTwist.keys = newKeys;
			helix.Twist = newTwist;
		}

		/// <summary>
		/// Displays helix width handles.
		/// </summary>
		/// <param name="baseId">Base identifier.</param>
		/// <param name="helix">Helix.</param>
		private static void DoHelixWidthHandles(int baseId, Helix helix)
		{
			float v;
			AnimationCurve twist = helix.Twist;
			AnimationCurve newWidth = helix.Width;
			Keyframe[] newKeys = newWidth.keys;
			for (int i = 0; i < newKeys.Length; ++i)
			{
				// compute the disc center
				Vector3 discCenter = Vector3.forward * newKeys[i].time * helix.Length;
				// compute the angle of the handle around the length
				Quaternion angle = Quaternion.AngleAxis(twist.Evaluate(newKeys[i].time), Vector3.forward);
				// create a cone handle with a slight offset to not overlap the arc handle
				v = newKeys[i].value + s_WidthHandleOffset;
				int id = ObjectX.GenerateHashCode(baseId, s_WidthHandleHash, i, 1);
				v = LinearHandles.Cone(id, v, discCenter, angle * Vector3.right, capScale: s_WidthHandleSize);
				newKeys[i].value = v - s_WidthHandleOffset;
				// create time adjustment handles for intermediary keys
				if (i ==0 || i == newKeys.Length - 1)
				{
					continue;
				}
				v = newKeys[i].time * helix.Length;
				id = ObjectX.GenerateHashCode(baseId, s_WidthHandleHash, i, 2); 
				v = LinearHandles.Cone(id, v, Vector3.zero, Vector3.forward, capScale: s_TimeAdjustHandleSize);
				v = Mathf.Clamp(
					v / helix.Length,
					Mathf.Epsilon * 2f,
					1f - Mathf.Epsilon * 2f
				);
				if (Mathf.Abs(v - newKeys[i].time) > s_RequiredMinHandleChange)
				{
					newKeys[i].time = v;
				}
				// TODO: Fix problems with keys moving over each other
			}
			newWidth.keys = newKeys;
			helix.Width = newWidth;
		}

		/// <summary>
		/// Draws the helix.
		/// </summary>
		/// <param name="helix">Helix.</param>
		/// <param name="divisions">Divisions.</param>
		/// <param name="fillMode">Fill mode.</param>
		/// <param name="shapeMode">Shape mode.</param>
		private static void DrawHelix(Helix helix, int divisions, FillMode fillMode, ShapeMode shapeMode)
		{
			// early out if not repaint phase
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			// initialize mesh properties
			Vector3[] vertices = new Vector3[divisions * 2 + 2];
			Vector2[] uv = new Vector2[vertices.Length];
			Color[] colors = new Color[vertices.Length];
			int[] triangles = new int[divisions * 12];
			// compute mesh data
			bool isRibbon = shapeMode == ShapeMode.Ribbon;
			bool isSolid = fillMode != FillMode.Wire;
			bool isMesh = fillMode == FillMode.Mesh;
			float oneHalfLengthOverDivisions = 0.5f * helix.Length / divisions;
			for (int i = 0; i<vertices.Length; i += 2)
			{
				// current parameter value
				float parameter = (float)(i) * oneHalfLengthOverDivisions;
				// compute vertex
				vertices[i] = helix.Evaluate(parameter);
				// draw line
				if (i > 0)
				{
					Handles.DrawLine(vertices[i - 2], vertices[i]);
				}
				// compute corresponding vertex
				if (isRibbon)
				{
					vertices[i+1] = helix.EvaluateOpposite(parameter);
					// draw opposite line
					if (i > 0)
					{
						Handles.DrawLine(vertices[i - 1], vertices[i + 1]);
					}
				}
				else
				{
					vertices[i+1].z = vertices[i].z;
				}
				// draw connecting lines if requested
				if (isMesh)
				{
					Handles.DrawLine(vertices[i], vertices[i + 1]);
				}
				// populate colors
				colors[i] = Handles.color;
				colors[i].a *= SceneGUI.FillAlphaScalar;
				colors[i + 1] = colors[i];
			}
			// compute triangles
			int stepCount = 6;
			for (int i=0; i<triangles.Length; i+=stepCount)
			{
				int start = i / stepCount;
				triangles[i] = start;
				triangles[i + 1] = start + 1;
				triangles[i + 2] = start + 2;
				triangles[i + 3] = start + 2;
				triangles[i + 4] = start + 1;
				triangles[i + 5] = start;
			}
			// update mesh
			if (s_FillMesh == null)
			{
				s_FillMesh = new Mesh();
				s_FillMesh.hideFlags = HideFlags.DontSave;
			}
			s_FillMesh.Clear();
			s_FillMesh.vertices = vertices;
			s_FillMesh.colors = colors;
			s_FillMesh.triangles = triangles;
			s_FillMesh.uv = uv;
			s_FillMesh.RecalculateNormals();
			// draw the mesh
			if (isSolid)
			{
				Graphics.DrawMeshNow(s_FillMesh, Handles.matrix);
			}
		}

		/// <summary>
		/// Display a mesh ribbon handle.
		/// </summary>
		/// <returns>A new helix with any modifications applied.</returns>
		/// <param name="ribbon">Ribbon.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="scale">Scale.</param>
		/// <param name="color">Color.</param>
		public static Helix MeshRibbon(
			HelixMeshRibbon ribbon, Vector3 origin, Quaternion orientation, Vector3 scale, Color color
		)
		{
			return DoHelixHandle(
				ribbon.GetHashCode(),
				ribbon.Helix,
				origin,
				orientation,
				scale,
				color,
				ribbon.Divisions,
				FillMode.Mesh,
				ShapeMode.Ribbon
			);
		}

		/// <summary>
		/// Display a solid helix handle.
		/// </summary>
		/// <returns>A new helix with any modifications applied.</returns>
		/// <param name="baseId">Base identifier. Each handle has its own unique hash based off this value.</param>
		/// <param name="helix">Helix.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="scale">Scale.</param>
		/// <param name="color">Color.</param>
		public static Helix SolidHelix(
			int baseId, Helix helix, Vector3 origin, Quaternion orientation, Vector3 scale, Color color
		)
		{
			return DoHelixHandle(
				baseId, helix, origin, orientation, scale, color, s_SmoothDivisionCount, FillMode.Solid, ShapeMode.Helix
			);
		}

		/// <summary>
		/// Display a solid ribbon handle.
		/// </summary>
		/// <returns>A new helix with any modifications applied.</returns>
		/// <param name="baseId">Base identifier. Each handle has its own unique hash based off this value.</param>
		/// <param name="helix">Helix.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="scale">Scale.</param>
		/// <param name="color">Color.</param>
		public static Helix SolidRibbon(
			int baseId, Helix helix, Vector3 origin, Quaternion orientation, Vector3 scale, Color color
		)
		{
			return DoHelixHandle(
				baseId, helix, origin, orientation, scale, color, s_SmoothDivisionCount, FillMode.Solid, ShapeMode.Ribbon
			);
		}

		/// <summary>
		/// Display a wire helix handle.
		/// </summary>
		/// <returns>A new helix with any modifications applied.</returns>
		/// <param name="baseId">Base identifier. Each handle has its own unique hash based off this value.</param>
		/// <param name="helix">Helix.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="scale">Scale.</param>
		/// <param name="color">Color.</param>
		public static Helix WireHelix(
			int baseId, Helix helix, Vector3 origin, Quaternion orientation, Vector3 scale, Color color
		)
		{
			return DoHelixHandle(
				baseId, helix, origin, orientation, scale, color, s_SmoothDivisionCount, FillMode.Wire, ShapeMode.Helix
			);
		}
		
		/// <summary>
		/// Display a wire ribbon handle.
		/// </summary>
		/// <returns>A new helix with any modifications applied.</returns>
		/// <param name="baseId">Base identifier. Each handle has its own unique hash based off this value.</param>
		/// <param name="helix">Helix.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="scale">Scale.</param>
		/// <param name="color">Color.</param>
		public static Helix WireRibbon(
			int baseId, Helix helix, Vector3 origin, Quaternion orientation, Vector3 scale, Color color
		)
		{
			return DoHelixHandle(
				baseId, helix, origin, orientation, scale, color, s_SmoothDivisionCount, FillMode.Wire, ShapeMode.Ribbon
			);
		}

		#region Obsolete
		[System.Obsolete("Use Candlelight.HelixHandles.SolidHelix(int baseId, Helix helix, Vector3 origin, Quaternion orientation, Vector3 scale, Color color)")]
		public static void SolidHelix(Helix helix, Vector3 origin, Quaternion orientation, Vector3 scale, Color color)
		{
			SolidHelix(0, helix, origin, orientation, scale, color);
		}
		[System.Obsolete("Use Candlelight.HelixHandles.SolidRibbon(int baseId, Helix helix, Vector3 origin, Quaternion orientation, Vector3 scale, Color color)")]
		public static Helix SolidRibbon(Helix helix, Vector3 origin, Quaternion orientation, Vector3 scale, Color color)
		{
			return SolidRibbon(0, helix, origin, orientation, scale, color);
		}
		[System.Obsolete("Use Candlelight.HelixHandles.WireHelix(int baseId, Helix helix, Vector3 origin, Quaternion orientation, Vector3 scale, Color color)")]
		public static Helix WireHelix(Helix helix, Vector3 origin, Quaternion orientation, Vector3 scale, Color color)
		{
			return WireHelix(0, helix, origin, orientation, scale, color);
		}
		[System.Obsolete("Use Candlelight.HelixHandles.WireRibbon(int baseId, Helix helix, Vector3 origin, Quaternion orientation, Vector3 scale, Color color)")]
		public static Helix WireRibbon(Helix helix, Vector3 origin, Quaternion orientation, Vector3 scale, Color color)
		{
			return WireRibbon(0, helix, origin, orientation, scale, color);
		}
		#endregion
	}
}