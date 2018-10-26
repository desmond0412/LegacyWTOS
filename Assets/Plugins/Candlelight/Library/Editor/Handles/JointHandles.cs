// 
// JointHandles.cs
// 
// Copyright (c) 2011-2015, Candlelight Interactive, LLC
// All rights reserved.
// 
// This file is licensed according to the terms of the Unity Asset Store EULA:
// http://download.unity3d.com/assetstore/customer-eula.pdf
// 
// This file contains a static class with methods for drawing ConfigurableJoint
// handles.
// 
// It requires the Interpolate class from unifycommunity wiki:
// http://www.unifycommunity.com/wiki/index.php?title=Interpolate

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Candlelight.Physics
{
	/// <summary>
	/// Joint handles.
	/// </summary>
	/// <remarks>
	/// Joint handles will not inherent nonuniform scale from the handles matrix.
	/// </remarks>
	public static class JointHandles
	{
		#region Base Hash Codes
		private static readonly int s_AngularHandleHash = 83;
		private static readonly int s_LinearHandleHash = 89;
		private static readonly int s_MaxHash = 179;
		private static readonly int s_MinHash = 181;
		#endregion

		/// <summary>
		/// The angular cone fill mesh segment count.
		/// </summary>
		private const int k_AngularConeFillMeshSegmentCount = 4;
		/// <summary>
		/// The angular cone fill mesh vertex count.
		/// </summary>
		private const int k_AngularConeFillMeshVertexCount = 
			k_StepsPerAngularFillMeshSegment * k_AngularConeFillMeshSegmentCount * 2;
		/// <summary>
		/// The linear cylinder fill mesh span count.
		/// </summary>
		private const int k_LinearCylinderFillMeshSpanCount = 360;
		/// <summary>
		/// The linear cylinder fill mesh vertex count.
		/// </summary>
		private const int k_LinearCylinderFillMeshVertexCount = k_LinearCylinderFillMeshSpanCount * 2;
		/// <summary>
		/// The steps per angular fill mesh segment.
		/// </summary>
		private const int k_StepsPerAngularFillMeshSegment = 90;
		/// <summary>
		/// The angular cone fill mesh colors.
		/// </summary>
		private static Color[] s_AngularConeFillMeshColors = new Color[k_AngularConeFillMeshVertexCount];
		/// <summary>
		/// The angular cone fill mesh vertices.
		/// </summary>
		private static Vector3[] s_AngularConeFillMeshVertices = new Vector3[k_AngularConeFillMeshVertexCount];
		/// <summary>
		/// The fill scalars for motion types.
		/// </summary>
		private static readonly Dictionary<ConfigurableJointMotion, float> s_FillScalarsForMotionTypes = 
			new Dictionary<ConfigurableJointMotion, float>()
		{
			{ ConfigurableJointMotion.Free, 1f },
			{ ConfigurableJointMotion.Limited, 1f },
			{ ConfigurableJointMotion.Locked, 0.1f }
		};
		/// <summary>
		/// The linear cylinder fill mesh colors.
		/// </summary>
		private static Color[] s_LinearCylinderFillMeshColors = new Color[k_LinearCylinderFillMeshVertexCount];
		/// <summary>
		/// The linear cylinder fill mesh vertices.
		/// </summary>
		private static Vector3[] s_LinearCylinderFillMeshVertices = new Vector3[k_LinearCylinderFillMeshVertexCount];
		/// <summary>
		/// The linear plane fill mesh colors.
		/// </summary>
		private static Color[] s_LinearPlaneFillMeshColors = new Color[4];
		/// <summary>
		/// The linear plane fill mesh vertices.
		/// </summary>
		private static Vector3[] s_LinearPlaneFillMeshVertices = new Vector3[4];
		/// <summary>
		/// The lock icon position.
		/// </summary>
		private static readonly Vector3 s_LockIconPosition = new Vector3(-1f, 1f, -1f) * 0.005f;
		/// <summary>
		/// The spline vertices for generating the catmull-rom cone.
		/// </summary>
		private static readonly Vector3[] s_SplineVertices = new Vector3[8];
		/// <summary>
		/// The color of the x axis.
		/// </summary>
		private static readonly Color s_XColor = Handles.xAxisColor;
		/// <summary>
		/// The color of the y axis.
		/// </summary>
		private static readonly Color s_YColor = Handles.yAxisColor;
		/// <summary>
		/// The color of the z axis.
		/// </summary>
		private static readonly Color s_ZColor = Handles.zAxisColor;
		/// <summary>
		/// Z handle scalar.
		/// </summary>
		private static readonly float s_ZHandleScalar = 0.5f;

		#region Backing Fields
		private static Mesh s_AngularConeFillMesh = null;
		private static Mesh s_LinearCylinderFillMesh = null;
		private static Mesh s_LinearPlaneFillMesh = null;
		#endregion
		/// <summary>
		/// Gets the angular cone fill mesh.
		/// </summary>
		/// <value>The angular cone fill mesh.</value>
		private static Mesh AngularConeFillMesh
		{
			get
			{
				if (s_AngularConeFillMesh == null)
				{
					s_AngularConeFillMesh = new Mesh();
					// compute colors
					float colorDiv = 1f / k_StepsPerAngularFillMeshSegment;
					for (int segment = 0; segment < k_AngularConeFillMeshSegmentCount; ++segment)
					{
						Color fromColor = s_XColor;
						Color toColor = s_YColor;
						if (segment % 2 == 1)
						{
							fromColor = s_YColor;
							toColor = s_XColor;
						}
						for (int i = 0; i < k_StepsPerAngularFillMeshSegment; ++i)
						{
							int j = (segment * k_StepsPerAngularFillMeshSegment + i) * 2;
							s_AngularConeFillMeshColors[j] = Color.Lerp(fromColor, toColor, i * colorDiv);
							s_AngularConeFillMeshColors[j].a = SceneGUI.FillAlphaScalar * 2f;
							s_AngularConeFillMeshColors[j+1] = s_AngularConeFillMeshColors[j];
						}
					}
					// compute triangles
					int[] triangles = new int[k_StepsPerAngularFillMeshSegment * k_AngularConeFillMeshSegmentCount * 6];
					int quadCount = k_StepsPerAngularFillMeshSegment * k_AngularConeFillMeshSegmentCount;
					for (int quad = 0; quad < quadCount; ++quad)
					{
						int startTriangleIndex = quad * 6;
						int startVertIndex = quad * 2;
						triangles[startTriangleIndex] = startVertIndex - 2;
						triangles[startTriangleIndex+1] = startVertIndex - 1;
						triangles[startTriangleIndex+2] = startVertIndex;
						triangles[startTriangleIndex+3] = startVertIndex;
						triangles[startTriangleIndex+4] = startVertIndex - 1;
						triangles[startTriangleIndex+5] = startVertIndex + 1;
					}
					// wrap triangles
					for (int i = 0; i < triangles.Length; ++i)
					{
						if (triangles[i] < 0)
						{
							triangles[i] += s_AngularConeFillMeshVertices.Length;
						}
						if (triangles[i] > s_AngularConeFillMeshVertices.Length-1)
						{
							triangles[i] -= s_AngularConeFillMeshVertices.Length;
						}
					}
					// set properties
					s_AngularConeFillMesh.vertices = s_AngularConeFillMeshVertices;
					s_AngularConeFillMesh.colors = s_AngularConeFillMeshColors;
					s_AngularConeFillMesh.triangles = triangles;
					s_AngularConeFillMesh.uv = new Vector2[k_AngularConeFillMeshVertexCount];
					s_AngularConeFillMesh.hideFlags = HideFlags.DontSave;
				}
				return s_AngularConeFillMesh;
			}
		}
		/// <summary>
		/// Gets a size for an infinite dimension.
		/// </summary>
		/// <value>The size of an infinite dimension.</value>
		private static float InfiniteSize
		{
			get { return -100f * HandleUtility.GetHandleSize(Handles.matrix.MultiplyPoint3x4(Vector3.zero)); }
		}
		/// <summary>
		/// Gets the linear cylinder fill mesh.
		/// </summary>
		/// <value>
		/// The linear cylinder fill mesh.
		/// </value>
		private static Mesh LinearCylinderFillMesh
		{
			get
			{
				if (s_LinearCylinderFillMesh == null)
				{
					s_LinearCylinderFillMesh = new Mesh();
					// initialize vertices
					s_LinearCylinderFillMesh.vertices = s_LinearCylinderFillMeshVertices;
					// compute triangles
					int[] triangles = new int[k_LinearCylinderFillMeshSpanCount * 6];
					for (int quad = 0; quad < k_LinearCylinderFillMeshSpanCount; ++quad)
					{
						int startTriangleIndex = quad * 6;
						int startVertIndex = quad * 2;
						triangles[startTriangleIndex] = startVertIndex - 2;
						triangles[startTriangleIndex+1] = startVertIndex - 1;
						triangles[startTriangleIndex+2] = startVertIndex;
						triangles[startTriangleIndex+3] = startVertIndex;
						triangles[startTriangleIndex+4] = startVertIndex - 1;
						triangles[startTriangleIndex+5] = startVertIndex + 1;
					}
					// wrap triangles
					for (int i=0; i<triangles.Length; ++i)
					{
						if (triangles[i] < 0)
						{
							triangles[i] += k_LinearCylinderFillMeshVertexCount;
						}
						if (triangles[i] > k_LinearCylinderFillMeshVertexCount - 1)
						{
							triangles[i] -= k_LinearCylinderFillMeshVertexCount;
						}
					}
					// set properties
					s_LinearCylinderFillMesh.colors = s_LinearCylinderFillMeshColors;
					s_LinearCylinderFillMesh.triangles = triangles;
					s_LinearCylinderFillMesh.RecalculateNormals();
					s_LinearCylinderFillMesh.uv = new Vector2[k_LinearCylinderFillMeshVertexCount];
					s_LinearCylinderFillMesh.hideFlags = HideFlags.DontSave;
				}
				for (int i = 0; i < s_LinearCylinderFillMeshColors.Length; ++i)
				{
					s_LinearCylinderFillMeshColors[i] = Handles.color;
				}
				s_LinearCylinderFillMesh.colors = s_LinearCylinderFillMeshColors;
				return s_LinearCylinderFillMesh;
			}
		}
		/// <summary>
		/// Gets the linear plane fill mesh.
		/// </summary>
		/// <value>The linear plane fill mesh.</value>
		private static Mesh LinearPlaneFillMesh
		{
			get
			{
				if (s_LinearPlaneFillMesh == null)
				{
					s_LinearPlaneFillMesh = new Mesh();
					// create vertices
					s_LinearPlaneFillMesh.vertices = s_LinearPlaneFillMeshVertices;
					// create triangles
					s_LinearPlaneFillMesh.triangles = new int[]
					{
						0, 1, 2,
						0, 2, 3,
						2, 1, 0,
						3, 2, 0,
					};
					// set properties
					s_LinearPlaneFillMesh.RecalculateNormals();
					s_LinearPlaneFillMesh.colors = s_LinearPlaneFillMeshColors;
					s_LinearPlaneFillMesh.uv = new Vector2[4];
					s_LinearPlaneFillMesh.hideFlags = HideFlags.DontSave;
				}
				for (int i=0; i<s_LinearPlaneFillMeshColors.Length; ++i)
				{
					s_LinearPlaneFillMeshColors[i] = Handles.color;
				}
				s_LinearPlaneFillMesh.colors = s_LinearPlaneFillMeshColors;
				return s_LinearPlaneFillMesh;
			}
		}

		/// <summary>
		/// Displays a joint angular limit handle.
		/// </summary>
		/// <returns>The joint angular limits.</returns>
		/// <param name="joint">Joint.</param>
		/// <param name="bindpose">Bindpose.</param>
		/// <param name="scale">Scale.</param>
		public static JointLimits AngularLimit(HingeJoint joint, Matrix4x4 bindpose, float scale)
		{
			// store old handle color
			Color oldColor = Handles.color;
			// set the handle matrix
			Matrix4x4 oldMatrix = Handles.matrix;
			bindpose = JointX.GetJointAngularWorldMatrix(joint, bindpose);
			Vector3 origin = bindpose.MultiplyPoint3x4(Vector3.zero);
			Quaternion orientation = Quaternion.LookRotation(
				bindpose.MultiplyVector(Vector3.forward), bindpose.MultiplyVector(Vector3.up)
			);
			Handles.matrix = Matrix4x4.TRS(
				Handles.matrix.MultiplyPoint(origin),
				Quaternion.LookRotation(
					Handles.matrix.MultiplyVector(orientation * Vector3.forward),
					Handles.matrix.MultiplyVector(orientation * Vector3.up)
				),
				// joint limits are never affected by object scaling
				Vector3.one * scale * oldMatrix.GetScale().GetMaxValue()
			);
			// get the actual axes of the joint
			Vector3 axis, secondaryAxis, tertiaryAxis;
			axis = joint.axis;
			JointX.GetActualJointAxes(
				joint.axis, Quaternion.LookRotation(joint.axis) * Vector3.forward,
				out axis, out secondaryAxis, out tertiaryAxis
			);
			// get colors for each handle
			Color xLimitColor = s_XColor; xLimitColor.a = Handles.color.a;
			// directions of the handles
			Vector3 minXHandle, maxXHandle;
			// xMin/xMax Handles
			JointLimits limits = joint.limits;
			JointAngularLimits lim = new JointAngularLimits(limits.min, limits.max, 0f, 0f);
			DisplayXAngleHandle(
				ref lim, -axis, tertiaryAxis, xLimitColor, // NOTE: axis is opposite configurable joint
				joint.useLimits ? ConfigurableJointMotion.Limited : ConfigurableJointMotion.Free,
				ConfigurableJointMotion.Locked,
				out minXHandle, out maxXHandle,
				360f, ObjectX.GenerateHashCode(joint.GetHashCode(), s_AngularHandleHash)
			);
			limits.min = lim.XMin;
			limits.max = lim.XMax;
			// draw ghost of z axis to show current motion - matches Unity's axis display
			Handles.color = EditorGUIX.zHandleColor;
			Handles.matrix = Matrix4x4.TRS(origin, joint.transform.rotation, Vector3.one);
			Handles.ArrowCap(
				0, Vector3.zero, Quaternion.LookRotation(tertiaryAxis), HandleUtility.GetHandleSize(Vector3.zero) * 0.5f
			);
			// reset handle matrix
			Handles.matrix = oldMatrix;
			// reset handle color
			Handles.color = oldColor;
			// return results
			return limits;
		}

		/// <summary>
		/// Displays a joint angular limit handle.
		/// </summary>
		/// <returns>The joint angular limits.</returns>
		/// <param name="joint">Joint.</param>
		/// <param name="bindpose">The local matrix for the joint's bindpose.</param>
		/// <param name="scale">Scale./param>
		public static JointAngularLimits AngularLimit(CharacterJoint joint, Matrix4x4 bindpose, float scale)
		{
			JointAngularLimits limits = new JointAngularLimits(joint);
			bindpose = joint.GetJointAngularWorldMatrix(bindpose);
			return AngularLimit(
				joint.GetHashCode(),
				limits,
				bindpose.MultiplyPoint3x4(Vector3.zero),
				Quaternion.LookRotation(bindpose.MultiplyVector(Vector3.forward), bindpose.MultiplyVector(Vector3.up)),
				joint.axis,
				joint.swingAxis,
				scale,
				ConfigurableJointMotion.Limited,
				ConfigurableJointMotion.Limited,
				ConfigurableJointMotion.Limited
			);
		}
		
		/// <summary>
		/// Displays a joint angular limit handle.
		/// </summary>
		/// <returns>The joint angular limits.</returns>
		/// <param name="joint">Joint.</param>
		/// <param name="bindpose">The local matrix for the joint's bindpose.</param>
		/// <param name="scale">Scale./param>
		public static JointAngularLimits AngularLimit(ConfigurableJoint joint, Matrix4x4 bindpose, float scale)
		{
			JointAngularLimits limits = new JointAngularLimits(joint);
			bindpose = joint.GetJointAngularWorldMatrix(bindpose);
			return AngularLimit(
				joint.GetHashCode(),
				limits,
				bindpose.MultiplyPoint3x4(Vector3.zero),
				Quaternion.LookRotation(bindpose.MultiplyVector(Vector3.forward), bindpose.MultiplyVector(Vector3.up)),
				joint.axis,
				joint.secondaryAxis,
				scale,
				joint.angularXMotion,
				joint.angularYMotion,
				joint.angularZMotion
			);
		}

		/// <summary>
		/// Displays a joint angular limit handle.
		/// </summary>
		/// <returns>The joint angular limits.</returns>
		/// <param name="baseId">Base identifier. Each handle has its own unique hash based off this value.</param>
		/// <param name="limits">The angular limits for the handle.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="axis">Axis.</param>
		/// <param name="secondaryAxis">Secondary axis.</param>
		/// <param name="scale">Scale.</param>
		/// <param name="angularXMotion">Angular X motion.</param>
		/// <param name="angularYMotion">Angular Y motion.</param>
		/// <param name="angularZMotion">Angular Z motion.</param>
		/// <param name="axisDefinitionMax">The maximum of the (symmetric) definition limit for axis.</param>
		private static JointAngularLimits AngularLimit(
			int baseId,
			JointAngularLimits limits,
			Vector3 origin,
			Quaternion orientation,
			Vector3 axis,
			Vector3 secondaryAxis, 
			float scale,
			ConfigurableJointMotion angularXMotion,
			ConfigurableJointMotion angularYMotion,
			ConfigurableJointMotion angularZMotion
		)
		{
			// store old handle color
			Color oldColor = Handles.color;
			// set the handle matrix
			Matrix4x4 oldMatrix = Handles.matrix;
			Handles.matrix = Matrix4x4.TRS(
				Handles.matrix.MultiplyPoint(origin),
				Quaternion.LookRotation(
					Handles.matrix.MultiplyVector(orientation * Vector3.forward),
					Handles.matrix.MultiplyVector(orientation * Vector3.up)
				),
				// joint limits are never affected by object scaling
				Vector3.one * scale * oldMatrix.GetScale().GetMaxValue()
			);
			// get the actual axes of the joint
			Vector3 tertiaryAxis;
			JointX.GetActualJointAxes(
				axis, secondaryAxis, out axis, out secondaryAxis, out tertiaryAxis
			);
			// get colors for each handle
			Color xLimitColor = s_XColor; xLimitColor.a = Handles.color.a;
			Color yLimitColor = s_YColor; yLimitColor.a = Handles.color.a;
			Color zLimitColor = s_ZColor; zLimitColor.a = Handles.color.a;
			// directions of the handles
			Vector3 minXHandle, maxXHandle, minYHandle, maxYHandle;
			baseId = ObjectX.GenerateHashCode(baseId, s_AngularHandleHash);
			// xMin/xMax Handles
			DisplayXAngleHandle(
				ref limits, axis, tertiaryAxis, xLimitColor, angularXMotion, angularYMotion,
				out minXHandle, out maxXHandle, 180f, baseId
			);
			// yMax Handles
			DisplayYAngleHandle(
				ref limits, secondaryAxis, tertiaryAxis, yLimitColor, angularXMotion, angularYMotion,
				out minYHandle, out maxYHandle, baseId
			);
			// zMax Handles
			DisplayZAngleHandle(ref limits, axis, tertiaryAxis, zLimitColor, angularZMotion, baseId);
			// shade the cone if x and y are both limited
			if (
				angularXMotion == ConfigurableJointMotion.Limited && angularYMotion == ConfigurableJointMotion.Limited
			)
			{
				DisplayAngularLimitCone(minXHandle, maxXHandle, minYHandle, maxYHandle);
			}
			// reset handle matrix
			Handles.matrix = oldMatrix;
			// reset handle color
			Handles.color = oldColor;
			// return results
			return limits;
		}
		
		/// <summary>
		/// Displays the joint angular limit cone.
		/// </summary>
		/// <param name="minXHandle">Minimum X handle.</param>
		/// <param name="maxXHandle">Maximum X handle.</param>
		/// <param name="minYHandle">Minimum Y handle.</param>
		/// <param name="maxYHandle">Maximum Y handle.</param>
		private static void DisplayAngularLimitCone(
			Vector3 minXHandle, Vector3 maxXHandle, Vector3 minYHandle, Vector3 maxYHandle
		)
		{
			// early out if not a repaint phase
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			// premultiply color alpha
			float alpha = Handles.color.a * SceneGUI.FillAlphaScalar * 2f; // multiply by two since the mesh is single-sided
			// generate a list of vertices for the spline
			s_SplineVertices[0] = minXHandle;
			s_SplineVertices[1] = minYHandle;
			s_SplineVertices[2] = maxXHandle;
			s_SplineVertices[3] = maxYHandle;
			s_SplineVertices[4] = minXHandle;
			s_SplineVertices[5] = minYHandle;
			s_SplineVertices[6] = maxXHandle;
			s_SplineVertices[7] = maxYHandle;
			// iterate spline
			int last = s_SplineVertices.Length - 1;
			for (int current = 0; current < k_AngularConeFillMeshSegmentCount; ++current)
			{
				if (current > last)
				{
					current = 0;
				}
				int previous = (current == 0) ? last : current - 1;
				int start = current;
				int end = (current == last) ? 0 : current + 1;
				int next = (end == last) ? 0 : end + 1;
				// compute vertices and colors in this segment
				for (int step = 1; step <= k_StepsPerAngularFillMeshSegment; ++step)
				{
					Vector3 vertex = Interpolate.CatmullRom(
						s_SplineVertices[previous], s_SplineVertices[start],
						s_SplineVertices[end], s_SplineVertices[next],
						step, k_StepsPerAngularFillMeshSegment
					);
					int i = (
						current * k_StepsPerAngularFillMeshSegment + step
					) * 2 - 1;
					s_AngularConeFillMeshVertices[i] = vertex;
					s_AngularConeFillMeshColors[i].a = alpha;
					s_AngularConeFillMeshColors[i - 1].a = alpha;
				}
			}
			// update mesh properties
			AngularConeFillMesh.vertices = s_AngularConeFillMeshVertices;
			AngularConeFillMesh.colors = s_AngularConeFillMeshColors;
			AngularConeFillMesh.RecalculateNormals();
			// draw the mesh
			Graphics.DrawMeshNow(AngularConeFillMesh, Handles.matrix);
		}
		
		/// <summary>
		/// Draws a linear limit handle volume when all axes are free.
		/// </summary>
		private static void DisplayLinearVolumeAllFree()
		{
			// draw free sphere
			Handles.SphereCap(0, Vector3.zero, Quaternion.identity, InfiniteSize);
			// draw unlocked padlock
			Handles.BeginGUI();
			{
				Color oldColor = GUI.color;
				GUI.color = Color.black;
				Vector3 pt = Camera.current.WorldToScreenPoint(Handles.matrix.MultiplyPoint3x4(s_LockIconPosition));
				Rect rect = new Rect(
					pt.x, Camera.current.pixelHeight - pt.y,
					EditorStylesX.UnlockedIcon.width, EditorStylesX.UnlockedIcon.height
				);
				GUI.DrawTexture(rect, EditorStylesX.UnlockedIcon);
				GUI.color = oldColor;
			}
			Handles.EndGUI();
		}

		/// <summary>
		/// Draws a linear limit handle volume when all axes are limited.
		/// </summary>
		/// <param name="value">Linear limit value.</param>
		/// <param name="axes">Joint axes in space of Handles.matrix.</param>
		private static void DisplayLinearVolumeAllLimited(float value, Vector3[] axes)
		{
			// draw a sphere
			for (int i = 0; i < 2; ++i)
			{
				Handles.SphereCap(0, Vector3.zero, Quaternion.identity, value * 2f);
			}
			// draw wire handles
			SceneGUI.SetHandleAlpha(SceneGUI.LineAlphaScalar);
			Handles.DrawWireDisc(Vector3.zero, axes[0], value);
			Handles.DrawWireDisc(Vector3.zero, axes[1], value);
			Handles.DrawWireDisc(Vector3.zero, axes[2], value);
		}

		/// <summary>
		/// Draws a linear limit handle volume when all axes are locked.
		/// </summary>
		private static void DisplayLinearVolumeAllLocked()
		{
			// draw a padlock
			Handles.BeginGUI();
			{
				Color oldColor = GUI.color;
				GUI.color = Color.black;
				Vector3 pt = Camera.current.WorldToScreenPoint(
					Handles.matrix.MultiplyPoint3x4(s_LockIconPosition)
				);
				Rect rect = new Rect(
					pt.x, Camera.current.pixelHeight - pt.y,
					EditorStylesX.LockedIcon.width, EditorStylesX.LockedIcon.height
				);
				GUI.DrawTexture(rect, EditorStylesX.LockedIcon);
				GUI.color = oldColor;
			}
			Handles.EndGUI();
		}

		/// <summary>
		/// Draws a linear limit handle volume when one axis is free and two are
		/// locked.
		/// </summary>
		/// <param name="freeAxis">Free axis in space of Handles.matrix.</param>
		private static void DisplayLinearVolumeOneFreeTwoLocked(Vector3 freeAxis)
		{
			// draw a line
			SceneGUI.SetHandleAlpha(SceneGUI.LineAlphaScalar);
			Handles.DrawLine(-freeAxis * InfiniteSize, freeAxis * InfiniteSize);
		}

		/// <summary>
		/// Draws a linear limit handle volume when one axis is limited and one is locked.
		/// </summary>
		/// <param name="lockedAxis">Locked axis in space of Handles.matrix.</param>
		/// <param name="freeAxis">Free axis in space of Handles.matrix.</param>
		private static void DisplayLinearVolumeOneLimitedOneLocked(float value, Vector3 lockedAxis, Vector3 freeAxis)
		{
			// draw an infinite plane
			Handles.matrix = Matrix4x4.TRS(
				Handles.matrix.MultiplyPoint3x4(Vector3.zero),
				Quaternion.LookRotation(
					Handles.matrix.MultiplyVector(lockedAxis), Handles.matrix.MultiplyVector(freeAxis)
				),
				Vector3.one
			);
			SetLinearPlaneFillMeshVertices(value);
			Graphics.DrawMeshNow(LinearPlaneFillMesh, Handles.matrix);
			// draw wire handles
			SceneGUI.SetHandleAlpha(SceneGUI.LineAlphaScalar);
			Handles.DrawLine(s_LinearPlaneFillMeshVertices[0], s_LinearPlaneFillMeshVertices[3]);
			Handles.DrawLine(s_LinearPlaneFillMeshVertices[1], s_LinearPlaneFillMeshVertices[2]);
		}

		/// <summary>
		/// Draws a linear limit handle volume when two axes are free and one is limited.
		/// </summary>
		/// <param name="value">Linear limit value.</param>
		/// <param name="limitedAxis">Limited axis in space of Handles.matrix.</param>
		private static void DisplayLinearVolumeOneLimitedTwoFree(float value, Vector3 limitedAxis)
		{
			// draw an "infinite box"
			Vector3[] linearPoints = new Vector3[]
			{
				value * limitedAxis, -value * limitedAxis
			};
			foreach (Vector3 p in linearPoints)
			{
				Handles.DrawSolidDisc(p, p, InfiniteSize);
			}
			// draw wire handles
			SceneGUI.SetHandleAlpha(SceneGUI.LineAlphaScalar);
			Handles.DrawLine(linearPoints[0], linearPoints[1]);
		}

		/// <summary>
		/// Draws a linear limit handle volume when two axes are locked and one is limited.
		/// </summary>
		/// <param name="value">Linear limit value.</param>
		/// <param name="limitedAxis">Limited axis in space of Handles.matrix.</param>
		private static void DisplayLinearVolumeOneLimitedTwoLocked(float value, Vector3 limitedAxis)
		{
			// draw a line
			SceneGUI.SetHandleAlpha(SceneGUI.LineAlphaScalar);
			Handles.DrawLine(value * limitedAxis, -value * limitedAxis);
		}

		/// <summary>
		/// Draws a linear limit handle volume when two axes are free and one is locked.
		/// </summary>
		/// <param name="lockedAxis">Locked axis in space of Handles.matrix.</param>
		private static void DisplayLinearVolumeTwoFreeOneLocked(Vector3 lockedAxis)
		{
			// draw a "free plane"
			Handles.DrawSolidDisc(Vector3.zero, lockedAxis, InfiniteSize);
		}

		/// <summary>
		/// Draws a linear limit handle volume when two axes are limited and one is free.
		/// </summary>
		/// <param name="value">Linear limit value.</param>
		/// <param name="freeAxis">Free axis in space of Handles.matrix.</param>
		private static void DisplayLinearVolumeTwoLimitedOneFree(float value, Vector3 freeAxis)
		{
			// draw infinite cylinder
			SetLinearCylinderFillMeshVertices(value);
			Graphics.DrawMeshNow(
				LinearCylinderFillMesh,
				Matrix4x4.TRS(
					Handles.matrix.MultiplyPoint3x4(Vector3.zero),
					Quaternion.LookRotation(Handles.matrix.MultiplyVector(freeAxis)),
					Vector3.one
				)
			);
			// draw wire handle
			SceneGUI.SetHandleAlpha(SceneGUI.LineAlphaScalar);
			Handles.DrawWireDisc(Vector3.zero, freeAxis, value);
		}

		/// <summary>
		/// Draws a linear limit handle volume when two axes are limited and one is locked.
		/// </summary>
		/// <param name="value">Linear limit value.</param>
		/// <param name="lockedAxis">Locked axis in space of Handles.matrix.</param>
		private static void DisplayLinearVolumeTwoLimitedOneLocked(float value, Vector3 lockedAxis)
		{
			// draw flat cylinder
			Handles.DrawSolidDisc(Vector3.zero, lockedAxis, value);
			// draw wire handle
			SceneGUI.SetHandleAlpha(SceneGUI.LineAlphaScalar);
			Handles.DrawWireDisc(Vector3.zero, lockedAxis, value);
		}
		
		/// <summary>
		/// Displays the X angle handle.
		/// </summary>
		/// <param name="limits">Limits.</param>
		/// <param name="axis">Axis.</param>
		/// <param name="tertiaryAxis">Tertiary axis.</param>
		/// <param name="color">Color.</param>
		/// <param name="xMotion">X motion.</param>
		/// <param name="yMotion">Y motion.</param>
		/// <param name="minHandlePosition">Minimum handle position.</param>
		/// <param name="maxHandlePosition">Maximum handle position.</param>
		/// <param name="axisDefinitionMax">The maximum of the (symmetric) joint definition limit.</param>
		/// <param name="baseId">Base identifier. Each handle has its own unique hash based off this value.</param>
		private static void DisplayXAngleHandle(
			ref JointAngularLimits limits,
			Vector3 axis, Vector3 tertiaryAxis, Color color,
			ConfigurableJointMotion xMotion, ConfigurableJointMotion yMotion,
			out Vector3 minHandlePosition, out Vector3 maxHandlePosition,
			float axisDefinitionMax, int baseId
		)
		{
			// set default values
			minHandlePosition = maxHandlePosition = Vector3.zero;
			// display proper handle based on motion type
			switch (xMotion)
			{
			case ConfigurableJointMotion.Limited:
				// get the handle orientation
				Quaternion handleOrientation = Quaternion.LookRotation(tertiaryAxis, -axis);
				// display handles
				Handles.color = color;
				float xMin = ArcHandles.WireAngle(
					ObjectX.GenerateHashCode(baseId, (int)EditAxis.X, 1),
					limits.XMin,
					Vector3.zero,
					handleOrientation,
					1f
				);
				float xMax = ArcHandles.WireAngle(
					ObjectX.GenerateHashCode(baseId, (int)EditAxis.X, 2),
					limits.XMax,
					Vector3.zero,
					handleOrientation,
					1f
				);
				if (yMotion != ConfigurableJointMotion.Limited)
				{
					SceneGUI.SetHandleAlpha(color.a * SceneGUI.FillAlphaScalar);
					Handles.DrawSolidArc(
						Vector3.zero,
						handleOrientation * Vector3.up,
						handleOrientation * Quaternion.AngleAxis(limits.XMin, Vector3.up) * Vector3.forward,
						limits.XMax - limits.XMin,
						1f
					);
				}
				// validate values
				xMin = Mathf.Min(xMin, xMax); // cannot be larger than x max
				xMin = Mathf.Clamp(xMin, -axisDefinitionMax, axisDefinitionMax);
				xMax = Mathf.Max(xMax, xMin); // cannot be smaller than x min
				xMax = Mathf.Clamp(xMax, -axisDefinitionMax, axisDefinitionMax);
				limits = new JointAngularLimits(xMin, xMax, limits.YMax, limits.ZMax);
				// store final orientation of handles
				minHandlePosition = Quaternion.AngleAxis(-limits.XMin, axis) * handleOrientation * Vector3.forward;
				maxHandlePosition = Quaternion.AngleAxis(-limits.XMax, axis) * handleOrientation * Vector3.forward;
				break;
			default:
				color.a *= SceneGUI.FillAlphaScalar * s_FillScalarsForMotionTypes[xMotion];
				Handles.color = color;
				Handles.DrawSolidDisc(Vector3.zero, axis, 1f);
				break;
			}
		}
		
		/// <summary>
		/// Displays the Y angle handle.
		/// </summary>
		/// <param name="limits">Limits.</param>
		/// <param name="secondaryAxis">Secondary axis.</param>
		/// <param name="tertiaryAxis">Tertiary axis.</param>
		/// <param name="color">Color.</param>
		/// <param name="xMotion">X motion.</param>
		/// <param name="yMotion">Y motion.</param>
		/// <param name="minHandlePosition">Minimum handle position.</param>
		/// <param name="maxHandlePosition">Maximum handle position.</param>
		/// <param name="baseId">Base identifier.</param>
		private static void DisplayYAngleHandle(
			ref JointAngularLimits limits,
			Vector3 secondaryAxis, Vector3 tertiaryAxis, Color color,
			ConfigurableJointMotion xMotion, ConfigurableJointMotion yMotion,
			out Vector3 minHandlePosition, out Vector3 maxHandlePosition,
			int baseId
		)
		{
			// set default values
			minHandlePosition = maxHandlePosition = Vector3.zero;
			// display proper handle based on motion type
			switch (yMotion)
			{
			case ConfigurableJointMotion.Limited:
				// get the handle orientation
				Quaternion handleOrientation = Quaternion.LookRotation(tertiaryAxis, secondaryAxis);
				// display handles
				Handles.color = color;
				float angle = limits.YMax * 2f;
				baseId = ObjectX.GenerateHashCode(baseId, (int)EditAxis.Y);
				if (xMotion == ConfigurableJointMotion.Limited)
				{
					angle = ArcHandles.WireWedge(baseId, angle, Vector3.zero, handleOrientation, 1f);
				}
				else
				{
					angle = ArcHandles.SolidWedge(baseId, angle, Vector3.zero, handleOrientation, 1f);
				}
				limits = new JointAngularLimits(
					limits.XMin, limits.XMax, Mathf.Clamp(Mathf.Max(angle * 0.5f, 0f), 0f, 180f), limits.ZMax
				);
				// store orientation of handles
				minHandlePosition =
					Quaternion.AngleAxis(-limits.YMax, secondaryAxis) * handleOrientation * Vector3.forward;
				maxHandlePosition =
					Quaternion.AngleAxis(limits.YMax, secondaryAxis) * handleOrientation * Vector3.forward;
				break;
			default:
				color.a *= SceneGUI.FillAlphaScalar * s_FillScalarsForMotionTypes[yMotion];
				Handles.color = color;
				Handles.DrawSolidDisc(Vector3.zero, secondaryAxis, 1f);
				break;
			}
		}
		
		/// <summary>
		/// Displays the Z angle handle.
		/// </summary>
		/// <param name="limits">Limits.</param>
		/// <param name="axis">Axis.</param>
		/// <param name="tertiaryAxis">Tertiary axis.</param>
		/// <param name="color">Color.</param>
		/// <param name="motion">Motion.</param>
		/// <param name="baseId">Base identifier.</param>
		private static void DisplayZAngleHandle(
			ref JointAngularLimits limits,
			Vector3 axis,
			Vector3 tertiaryAxis,
			Color color,
			ConfigurableJointMotion motion,
			int baseId
		)
		{
			// display proper handle based on motion type
			switch (motion)
			{
			case ConfigurableJointMotion.Limited:
				// get orientation of each side of the handle
				Quaternion handleOrientation = Quaternion.LookRotation(axis, tertiaryAxis);
				Quaternion oppositeHandleOrientation = Quaternion.AngleAxis(180f, tertiaryAxis) * handleOrientation;
				// display handles
				Handles.color = color;
				float newAngle = 2f * limits.ZMax;
				baseId = ObjectX.GenerateHashCode(baseId, (int)EditAxis.Z);
				newAngle = ArcHandles.SolidWedge(baseId, newAngle, Vector3.zero, handleOrientation, s_ZHandleScalar);
				newAngle =
					ArcHandles.SolidWedge(baseId, newAngle, Vector3.zero, oppositeHandleOrientation, s_ZHandleScalar);
				// set value
				limits = new JointAngularLimits(
					limits.XMin,
					limits.XMax,
					limits.YMax,
					Mathf.Clamp(limits.ZMax + ArcHandles.GetDeltaAngle(limits.ZMax % 360f, newAngle * 0.5f), 0f, 180f)
				);
				break;
			default:
				Color oldColor = Handles.color;
				color.a *= SceneGUI.FillAlphaScalar * s_FillScalarsForMotionTypes[motion];
				Handles.color = color;
				Handles.DrawSolidDisc(Vector3.zero, tertiaryAxis, 1f);
				Handles.color = oldColor;
				break;
			}
		}

		/// <summary>
		/// Displays a configurable joint linear limit handle.
		/// </summary>
		/// <returns>The linear limit.</returns>
		/// <param name="joint">Joint.</param>
		/// <param name="bindpose">The local matrix for the joint's bindpose.</param>
		public static float LinearLimit(ConfigurableJoint joint, Matrix4x4 bindpose)
		{
			// get values
			SoftJointLimit limit = joint.linearLimit;
			float value = limit.limit;
			// store handle matrix
			Matrix4x4 oldMatrix = Handles.matrix;
			// store handle color
			Color oldColor = Handles.color;
			// set color
			Handles.color = Color.yellow;
			// set matrix
			bindpose = joint.GetJointLinearWorldMatrix(bindpose);
			Handles.matrix = Matrix4x4.TRS(
				bindpose.MultiplyPoint3x4(Vector3.zero),
				Quaternion.LookRotation(bindpose.MultiplyVector(Vector3.forward), bindpose.MultiplyVector(Vector3.up)),
				Vector3.one
			);
			// get actual joint axes and motion types
			Vector3[] axes = joint.GetActualJointAxes();
			List<ConfigurableJointMotion> motions = new List<ConfigurableJointMotion>();
			motions.Add(joint.xMotion);
			motions.Add(joint.yMotion);
			motions.Add(joint.zMotion);
			int numAxesWithLimitedMotion = motions.FindAll(m => m == ConfigurableJointMotion.Limited).Count;
			int numAxesWithLockedMotion = motions.FindAll(m => m == ConfigurableJointMotion.Locked).Count;
			// draw handles for limited axes
			int baseId = ObjectX.GenerateHashCode(joint.GetHashCode(), s_LinearHandleHash);
			for (int i = 0; i < 3; ++i)
			{
				if (motions[i] == ConfigurableJointMotion.Limited)
				{
					int id = ObjectX.GenerateHashCode(baseId, i);
					value = LinearHandles.Dot(id, value, Vector3.zero, axes[i]);
					value = LinearHandles.Dot(id, value, Vector3.zero, -axes[i]);
				}
			}
			// draw volume
			SceneGUI.SetHandleAlpha(SceneGUI.FillAlphaScalar);
			int index = -1;
			index = motions.FindIndex(m => m == ConfigurableJointMotion.Free);
			Vector3 freeAxis = index < 0 ? Vector3.zero : axes[index];
			index = motions.FindIndex(m => m == ConfigurableJointMotion.Limited);
			Vector3 limitedAxis = index < 0 ? Vector3.zero : axes[index];
			index = motions.FindIndex(m => m == ConfigurableJointMotion.Locked);
			Vector3 lockedAxis = index < 0 ? Vector3.zero : axes[index];
			switch (numAxesWithLimitedMotion)
			{
			case 3:
				DisplayLinearVolumeAllLimited(value, axes);
				break;
			case 2:
				if (numAxesWithLockedMotion == 1)
				{
					DisplayLinearVolumeTwoLimitedOneLocked(value, lockedAxis);
				}
				else
				{
					DisplayLinearVolumeTwoLimitedOneFree(value, freeAxis);
				}
				break;
			case 1:
				switch (numAxesWithLockedMotion)
				{
				case 2:
					DisplayLinearVolumeOneLimitedTwoLocked(value, limitedAxis);
					break;
				case 1:
					DisplayLinearVolumeOneLimitedOneLocked(value, lockedAxis, freeAxis);
					break;
				case 0:
					DisplayLinearVolumeOneLimitedTwoFree(value, limitedAxis);
					break;
				}
				break;
			case 0:
				switch (numAxesWithLockedMotion)
				{
				case 3:
					DisplayLinearVolumeAllLocked();
					break;
				case 2:
					DisplayLinearVolumeOneFreeTwoLocked(freeAxis);
					break;
				case 1:
					DisplayLinearVolumeTwoFreeOneLocked(lockedAxis);
					break;
				case 0:
					DisplayLinearVolumeAllFree();
					break;
				}
				break;
			}
			value = Mathf.Abs(value);
			// reset handle color
			Handles.color = oldColor;
			// reset handle matrix
			Handles.matrix = oldMatrix;
			// return the result
			return value;
		}
		
		/// <summary>
		/// Sets the linear cylinder fill mesh vertices.
		/// </summary>
		/// <param name="radius">Radius (i.e. linear limit value of joint).</param>
		private static void SetLinearCylinderFillMeshVertices(float radius)
		{
			float pctDiv = 2f * Mathf.PI / k_LinearCylinderFillMeshSpanCount;
			for (int span=0; span<k_LinearCylinderFillMeshSpanCount; ++span)
			{
				float angle = span * pctDiv;
				s_LinearCylinderFillMeshVertices[span*2] = new Vector3(
					Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0.5f * InfiniteSize
				);
				s_LinearCylinderFillMeshVertices[span*2+1] = s_LinearCylinderFillMeshVertices[span*2];
				s_LinearCylinderFillMeshVertices[span*2+1].z *= -1f;
			}
			LinearCylinderFillMesh.vertices = s_LinearCylinderFillMeshVertices;
		}

		/// <summary>
		/// Sets the linear plane fill mesh vertices.
		/// </summary>
		/// <param name="value">Linear limit value.</param>
		private static void SetLinearPlaneFillMeshVertices(float value)
		{
			s_LinearPlaneFillMeshVertices = new Vector3[]
			{
				Vector3.up * InfiniteSize * 0.5f + Vector3.right * value,
				Vector3.up * InfiniteSize * 0.5f + Vector3.left * value,
				Vector3.down * InfiniteSize * 0.5f + Vector3.left * value,
				Vector3.down * InfiniteSize * 0.5f + Vector3.right * value
			};
			LinearPlaneFillMesh.vertices = s_LinearPlaneFillMeshVertices;
		}

		/// <summary>
		/// Creates a handle for spring limits min and max.
		/// </summary>
		/// <returns>The limit.</returns>
		/// <param name="joint">Joint.</param>
		/// <param name="bindpose">Bindpose.</param>
		public static Vector2 SpringLimit(SpringJoint joint, Matrix4x4 bindpose)
		{
			// get values
			Vector2 value = new Vector2(joint.minDistance, joint.maxDistance);
			// compute spring "strength" scalar
			float strength = joint.spring / joint.GetComponent<Rigidbody>().mass * 0.05f;
			// store handle matrix
			Matrix4x4 oldMatrix = Handles.matrix;
			// store handle color
			Color oldColor = Handles.color;
			// draw spring line
			Handles.matrix = Matrix4x4.identity;
			Handles.color = Color.Lerp(Color.red, Color.yellow, strength);
			Handles.DrawLine(
				joint.GetJointLinearWorldMatrix(bindpose).MultiplyPoint3x4(Vector3.zero),
				joint.transform.TransformPoint(joint.anchor)
			);
			// set matrix
			bindpose = joint.GetJointLinearWorldMatrix(bindpose);
			Handles.matrix = Matrix4x4.TRS(
				bindpose.MultiplyPoint3x4(Vector3.zero),
				Quaternion.LookRotation(bindpose.MultiplyVector(Vector3.forward), bindpose.MultiplyVector(Vector3.up)),
				Vector3.one
			);
			// draw handles
			float fillAlpha = SceneGUI.FillAlphaScalar * Mathf.Clamp01(strength);
			Handles.color = Color.yellow;
			// NOTE: RadiusHandle gets whigged out with custom Handles matrix
			float radius = value.x;
			Vector3 center = Vector3.zero;
			ShapeHandles.WireSphere(
				ObjectX.GenerateHashCode(joint.GetHashCode(), s_LinearHandleHash, s_MinHash),
				ref radius,
				ref center,
				Quaternion.identity
			);
			value.x = radius;
			SceneGUI.SetHandleAlpha(fillAlpha);
			Handles.SphereCap(0, Vector3.zero, Quaternion.identity, value.x * 2f);
			Handles.color = Color.red;
			radius = Mathf.Max(value.x, value.y);
			ShapeHandles.WireSphere(
				ObjectX.GenerateHashCode(joint.GetHashCode(), s_LinearHandleHash, s_MaxHash),
				ref radius,
				ref center,
				Quaternion.identity
			);
			value.y = radius;
			SceneGUI.SetHandleAlpha(fillAlpha);
			Handles.SphereCap(0, Vector3.zero, Quaternion.identity, value.y * 2f);
			value.x = Mathf.Min(value.x, value.y);
			// reset handle color
			Handles.color = oldColor;
			// reset handle matrix
			Handles.matrix = oldMatrix;
			// return the result
			return value;
		}
	}
}