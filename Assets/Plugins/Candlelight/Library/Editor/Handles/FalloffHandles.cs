// 
// FalloffHandles.cs
// 
// Copyright (c) 2011-2016, Candlelight Interactive, LLC
// All rights reserved.
// 
// This file is licensed according to the terms of the Unity Asset Store EULA:
// http://download.unity3d.com/assetstore/customer-eula.pdf
// 
// This file contains a static class with methods for creating falloff handles.
// 
// Falloff handles currently produce strange behavior when dragging keyframes
// past each other along the domain of an AnimationCurve, as control IDs are
// not deterministic.

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Candlelight
{
	/// <summary>
	/// A class for creating handles for adjusting AnimationCurve-based falloff
	/// </summary>
	public static class FalloffHandles
	{
		/// <summary>
		/// Shape mode.
		/// </summary>
		internal enum ShapeMode { NoGraph, Graph, NormalizedGraph }

		/// <summary>
		/// Relevant menu data to pass when adding a keyframe.
		/// </summary>
		private class AddKeyframeMenuData
		{
			public int HandleId { get; private set; }
			public Vector2 MousePosition { get; private set; }
			public float XMax { get; private set; }
			public float XMin { get; private set; }

			public AddKeyframeMenuData(int handleId, Vector2 mousePosition, float xMin, float xMax)
			{
				this.HandleId = handleId;
				this.MousePosition = mousePosition;
				this.XMax = xMax;
				this.XMin = xMin;
			}
		}

		/// <summary>
		/// Relevant menu data to pass when deleting a keyframe.
		/// </summary>
		private class DeleteKeyframeMenuData
		{
			public int HandleId { get; private set; }
			public int KeyframeIndex { get; private set; }

			public DeleteKeyframeMenuData(int handleId, int keyframeIndex)
			{
				this.HandleId = handleId;
				this.KeyframeIndex = keyframeIndex;
			}
		}

		/// <summary>
		/// Data to persist for falloff handles being drawn.
		/// </summary>
		private class FalloffHandleData
		{
			public AnimationCurve Curve { get; set; }
			public int Id { get; set; }
			public Matrix4x4 GraphMatrix { get; set; }
			public Vector2 ScreenPosition { get; set; }

			public FalloffHandleData(int id)
			{
				this.Id = id;
			}
		}

		#region Base Hash Codes
		private static readonly int s_KeyframeValueHash = 29;
		private static readonly int s_KeyframeTimeHash = 31;
		private static int s_InTangentHash = 1181;
		private static int s_OutTangentHash = 1187;
		private static readonly int s_WireDiscHash = typeof(DiscHandles).GetStaticFieldValue<int>("s_WireDiscHash");
		#endregion

		#region Constants
		private const int k_DiscMeshSubdivisions = 90;
		private const int k_DiscMeshVertexCount = (k_DiscMeshSubdivisions + 1) * (k_GradientMeshSubdivisions + 1);
		private const int k_GradientMeshSubdivisions = 10;
		private const int k_GraphMeshSubdivisionCount = 128;
		private const int k_GraphMeshVertexCount = (k_GraphMeshSubdivisionCount + 1) * 2;
		#endregion

		/// <summary>
		/// All of the handles currently known to be active, keyed by their base control ID.
		/// </summary>
		private static readonly Dictionary<int, FalloffHandleData> s_ActiveHandles =
			new Dictionary<int, FalloffHandleData>();
		/// <summary>
		/// The handle offset to apply inside of <see cref="FalloffHandles.CircleCap()"/> 
		/// </summary>
		private static Quaternion s_CircleCapOffset = Quaternion.identity;
		/// <summary>
		/// The disc mesh colors.
		/// </summary>
		private static Color[] s_DiscMeshColors = new Color[k_DiscMeshVertexCount];
		/// <summary>
		/// The disc mesh normalized vertices.
		/// </summary>
		private static Vector3[] s_DiscMeshNormalizedVertices = new Vector3[k_DiscMeshVertexCount];
		/// <summary>
		/// The disc mesh vertices.
		/// </summary>
		private static Vector3[] s_DiscMeshVertices = new Vector3[k_DiscMeshVertexCount];
		/// <summary>
		/// The vertices making up the line in the graph.
		/// </summary>
		private static readonly Vector3[] s_GraphLineVertices = new Vector3[k_GraphMeshVertexCount / 2];
		/// <summary>
		/// The graph mesh colors.
		/// </summary>
		private static Color[] s_GraphMeshColors = new Color[k_GraphMeshVertexCount];
		/// <summary>
		/// The graph mesh vertices.
		/// </summary>
		private static readonly Vector3[] s_GraphMeshVertices = new Vector3[k_GraphMeshVertexCount];
		/// <summary>
		/// The <see cref="UnityEditor.Handles.StartCapDraw()"/> method.
		/// </summary>
		private static readonly System.Reflection.MethodInfo s_HandlesStartCapDraw =
			typeof(Handles).GetStaticMethod("StartCapDraw");
		/// <summary>
		/// The parameters to pass to <see cref="FalloffHandles.m_HandlesStartCapDraw"/>.
		/// </summary>
		private static readonly object[] s_HandlesStartCapDrawParams = new object[3];
		/// <summary>
		/// A reusable list for storing keyframes while working.
		/// </summary>
		private static readonly List<Keyframe> s_Keyframes = new List<Keyframe>(64);
		/// <summary>
		/// For each base control ID, the index of a keyframe to delete.
		/// </summary>
		private static readonly Dictionary<int, int> s_KeyframesToDelete = new Dictionary<int, int>();
		/// <summary>
		/// The control ID of the most recently clicked keyframe handle.
		/// </summary>
		private static int s_LastClickedKeyframeHandleId = 0;
		/// <summary>
		/// The index of the last clicked keyframe.
		/// </summary>
		private static int s_LastClickedKeyframeHandleIndex = -1;
		/// <summary>
		/// For each base control ID, the new keyframe to insert.
		/// </summary>
		private static readonly Dictionary<int, Keyframe> s_NewKeyframes = new Dictionary<int, Keyframe>();
		/// <summary>
		/// The sphere graph offset.
		/// </summary>
		private static readonly Quaternion s_SphereGraphOffset =
			Quaternion.FromToRotation(Vector3.right, Vector3.forward) *
			Quaternion.FromToRotation(Vector3.up, Vector3.right);
		/// <summary>
		/// Angles to which tangent handles can snap.
		/// </summary>
		private static readonly float[] s_TangentSnapAngles =
			new [] { 0.5f * Mathf.PI, 0.25f * Mathf.PI, 0f, -0.25f * Mathf.PI, - 0.5f * Mathf.PI };

		#region Backing Fields
		private static readonly ColorGradient s_DefaultColorGradient =
			new ColorGradient(Color.red, Color.yellow, ColorInterpolationSpace.HSV);
		private static Mesh m_DiscMesh = null;
		private static Mesh m_GraphMesh = null;
		#endregion
		/// <summary>
		/// Gets the default color gradient.
		/// </summary>
		/// <value>The default color gradient.</value>
		public static ColorGradient DefaultColorGradient { get { return s_DefaultColorGradient; } }
		/// <summary>
		/// Gets the graph mesh.
		/// </summary>
		/// <value>The graph mesh.</value>
		private static Mesh GraphMesh
		{
			get
			{
				if (m_GraphMesh == null)
				{
					m_GraphMesh = new Mesh();
					// initialize mesh properties
					int[] triangles = new int[k_GraphMeshSubdivisionCount * 6];
					Vector2[] uv = new Vector2[k_GraphMeshVertexCount];
					// compute mesh properties
					bool isEvenTriangle = true;
					int stepCount = 3;
					for (int i = 0; i < triangles.Length; i += stepCount)
					{
						int start = i / stepCount;
						if (isEvenTriangle)
						{
							triangles[i] = start;
							triangles[i + 1] = start + 1;
							triangles[i + 2] = start + 2;
						}
						else
						{
							triangles[i] = start + 2;
							triangles[i + 1] = start + 1;
							triangles[i + 2] = start;
						}
						isEvenTriangle = !isEvenTriangle;
					}
					// set mesh properties
					m_GraphMesh.vertices = s_GraphMeshVertices;
					m_GraphMesh.colors = s_GraphMeshColors;
					m_GraphMesh.triangles = triangles;
					m_GraphMesh.uv = uv;
					m_GraphMesh.RecalculateNormals();
					m_GraphMesh.hideFlags = HideFlags.DontSave;
				}
				return m_GraphMesh;
			}
		}
		/// <summary>
		/// Gets the disc mesh.
		/// </summary>
		/// <value>The disc mesh.</value>
		private static Mesh DiscMesh
		{
			get
			{
				if (m_DiscMesh == null)
				{
					m_DiscMesh = new Mesh();
					// initialize mesh properties
					List<int> triangles = new List<int>();
					Vector2[] uv = new Vector2[k_DiscMeshVertexCount];
					// compute mesh properties
					float gradientDiv = 1f / k_GradientMeshSubdivisions;
					float radialDiv = 2f * Mathf.PI / k_DiscMeshSubdivisions;
					for (int c = 0; c <= k_DiscMeshSubdivisions; ++c)
					{
						for (int r = 0; r <= k_GradientMeshSubdivisions; ++r)
						{
							int i = c * (k_GradientMeshSubdivisions + 1) + r;
							// compute vertex position
							s_DiscMeshNormalizedVertices[i] =
								new Vector3(Mathf.Cos(radialDiv * c), 0f, Mathf.Sin(radialDiv * c)) * r * gradientDiv;
							// set placeholder color
							s_DiscMeshColors[i] = Color.magenta;
							// skip limits
							if (c == k_DiscMeshSubdivisions || r == k_GradientMeshSubdivisions)
							{
								continue;
							}
							// compute triangle indices
							triangles.Add(i);
							triangles.Add(i + k_GradientMeshSubdivisions + 1);
							triangles.Add(i + 1);
							triangles.Add(i + k_GradientMeshSubdivisions + 2);
							triangles.Add(i + 1);
							triangles.Add(i + k_GradientMeshSubdivisions + 1);
						}
					}
					// set mesh properties
					m_DiscMesh.vertices = s_DiscMeshNormalizedVertices;
					m_DiscMesh.colors = s_DiscMeshColors;
					m_DiscMesh.triangles = triangles.ToArray();
					m_DiscMesh.uv = uv;
					m_DiscMesh.RecalculateNormals();
					m_DiscMesh.hideFlags = HideFlags.DontSave;
				}
				return m_DiscMesh;
			}
		}

		/// <summary>
		/// A circle cap function.
		/// </summary>
		/// <param name="controlID">Control identifier.</param>
		/// <param name="position">Position.</param>
		/// <param name="rotation">Rotation.</param>
		/// <param name="size">Size.</param>
		private static void CircleCap(int controlID, Vector3 position, Quaternion rotation, float size)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			s_HandlesStartCapDrawParams[0] = position;
			s_HandlesStartCapDrawParams[1] = rotation;
			s_HandlesStartCapDrawParams[2] = size;
			s_HandlesStartCapDraw.Invoke(null, s_HandlesStartCapDrawParams);
			Handles.DrawWireDisc(position, rotation * s_CircleCapOffset * Vector3.forward, size);
		}

		/// <summary>
		/// Creates a label for the specified <paramref name="key"/>.
		/// </summary>
		/// <returns>A label for the specfied <paramref name="key"/>.</returns>
		/// <param name="key">Key.</param>
		/// <param name="shapeMode">Shape mode.</param>
		/// <param name="xLabel">X label.</param>
		/// <param name="yLabel">Y label.</param>
		/// <param name="minLabel">Minimum label.</param>
		/// <param name="maxLabel">Maximum label.</param>
		/// <param name="isFirst">If set to <see langword="true"/> then prefer <paramref name="minLabel"/>.</param>
		/// <param name="isLast">If set to <see langword="true"/> then prefer <paramref name="maxLabel"/>.</param>
		private static string CreateLabel(
			Keyframe key, ShapeMode shapeMode,
			string xLabel, string yLabel, string minLabel, string maxLabel,
			bool isFirst, bool isLast
		)
		{
			using (StringX.StringBuilderScope sb = new StringX.StringBuilderScope())
			{
				bool isGraph = shapeMode != ShapeMode.NoGraph;
				string text = isGraph ? yLabel : xLabel;;
				if (isFirst && !string.IsNullOrEmpty(minLabel))
				{
					text = minLabel;
				}
				if (isLast && !string.IsNullOrEmpty(maxLabel))
				{
					text = maxLabel;
				}
				sb.StringBuilder.AppendFormat("{0}{1}", text, text.Length > 0 ? ": " : "");
				if (!isGraph)
				{
					if (sb.StringBuilder.Length > 0)
					{
						sb.StringBuilder.AppendFormat("{0:0.###}", key.time);
					}
				}
				else
				{
					if (sb.StringBuilder.Length > 0)
					{
						sb.StringBuilder.AppendFormat("{0:0.###}", key.value);
					}
					if (xLabel.Length > 0)
					{
						sb.StringBuilder.AppendFormat(
							"{0}{1}: {2:0.###}", sb.StringBuilder.Length > 0 ? "\n" : "", xLabel, key.time
						);
					}
				}
				return sb.StringBuilder.ToString();
			}
		}

		/// <summary>
		/// Displays a falloff disc handle.
		/// </summary>
		/// <returns>The modified AnimationCurve.</returns>
		/// <param name="baseId">Base identifier. Each handle has its own unique hash based off this value.</param>
		/// <param name="curve">Curve.</param>
		/// <param name="center">Center.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="maxLabel">Max label.</param>
		/// <param name="minLabel">Minimum label.</param>
		/// <param name="xLabel">X label.</param>
		/// <param name="yLabel">Y label.</param>
		/// <param name="xMin">Domain lower bound.</param>
		/// <param name="xMax">Domain upper bound.</param>
		public static AnimationCurve Disc(
			int baseId, AnimationCurve curve, Vector3 center, Quaternion orientation,
			string maxLabel = "", string minLabel = "", string xLabel = "", string yLabel = "",
			float xMin = 0f, float xMax = Mathf.Infinity
		)
		{
			return Disc(
				baseId, curve, center, orientation, s_DefaultColorGradient,
				maxLabel, minLabel, xLabel, yLabel,
				xMin, xMax
			);
		}
		
		/// <summary>
		/// Displays a falloff disc handle.
		/// </summary>
		/// <returns>The modified AnimationCurve.</returns>
		/// <param name="baseId">Base identifier. Each handle has its own unique hash based off this value.</param>
		/// <param name="curve">Curve.</param>
		/// <param name="center">Center.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="colorGradient">Color gradient.</param>
		/// <param name="maxLabel">Max label.</param>
		/// <param name="minLabel">Minimum label.</param>
		/// <param name="xLabel">X label.</param>
		/// <param name="yLabel">Y label.</param>
		/// <param name="xMin">Domain lower bound.</param>
		/// <param name="xMax">Domain upper bound.</param>
		public static AnimationCurve Disc(
			int baseId, AnimationCurve curve, Vector3 center, Quaternion orientation, ColorGradient colorGradient,
			string maxLabel = "", string minLabel = "", string xLabel = "", string yLabel = "",
			float xMin = 0f, float xMax = Mathf.Infinity
		)
		{
			return DoFalloffHandle(
				baseId, curve, center, orientation, colorGradient,
				xMin, xMax, Mathf.NegativeInfinity, Mathf.Infinity,
				maxLabel, minLabel, xLabel, yLabel,
				Quaternion.identity, ShapeMode.NoGraph
			);
		}
		
		/// <summary>
		/// Displays a falloff disc graph handle.
		/// </summary>
		/// <returns>The modified AnimationCurve.</returns>
		/// <param name="baseId">Base identifier. Each handle has its own unique hash based off this value.</param>
		/// <param name="curve">Curve.</param>
		/// <param name="center">Center.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="maxLabel">Max label.</param>
		/// <param name="minLabel">Minimum label.</param>
		/// <param name="xLabel">X label.</param>
		/// <param name="yLabel">Y label.</param>
		/// <param name="xMin">Domain lower bound.</param>
		/// <param name="xMax">Domain upper bound.</param>
		/// <param name="yMin">Range lower bound.</param>
		/// <param name="yMax">Range upper bound.</param>
		public static AnimationCurve DiscGraph(
			int baseId, AnimationCurve curve,Vector3 center, Quaternion orientation,
			string maxLabel = "", string minLabel = "", string xLabel = "", string yLabel = "",
			float xMin = 0f, float xMax = Mathf.Infinity,
			float yMin = Mathf.NegativeInfinity, float yMax = Mathf.Infinity
		)
		{
			return DiscGraph(
				baseId, curve, center, orientation, s_DefaultColorGradient,
				maxLabel, minLabel, xLabel, yLabel, xMin, xMax, yMin, yMax
			);
		}
		
		/// <summary>
		/// Displays a falloff disc graph handle.
		/// </summary>
		/// <returns>The modified AnimationCurve.</returns>
		/// <param name="baseId">Base identifier. Each handle has its own unique hash based off this value.</param>
		/// <param name="curve">Curve.</param>
		/// <param name="center">Center.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="colorGradient">Color gradient.</param>
		/// <param name="maxLabel">Max label.</param>
		/// <param name="minLabel">Minimum label.</param>
		/// <param name="xLabel">X label.</param>
		/// <param name="yLabel">Y label.</param>
		/// <param name="xMin">Domain lower bound.</param>
		/// <param name="xMax">Domain upper bound.</param>
		/// <param name="yMin">Range lower bound.</param>
		/// <param name="yMax">Range upper bound.</param>
		public static AnimationCurve DiscGraph(
			int baseId, AnimationCurve curve, Vector3 center, Quaternion orientation, ColorGradient colorGradient,
			string maxLabel = "", string minLabel = "", string xLabel = "", string yLabel = "",
			float xMin = 0f, float xMax = Mathf.Infinity,
			float yMin = Mathf.NegativeInfinity, float yMax = Mathf.Infinity
		)
		{
			return DoFalloffHandle(
				baseId, curve, center, orientation, colorGradient,
				xMin, xMax, yMin, yMax,
				maxLabel, minLabel, xLabel, yLabel,
				Quaternion.identity, ShapeMode.Graph
			);
		}
		
		/// <summary>
		/// Displays a falloff handle.
		/// </summary>
		/// <returns>
		/// The modified AnimationCurve.
		/// </returns>
		/// <param name="baseId">Base identifier. Each handle has its own unique hash based off this value.</param>
		/// <param name="curve">Curve.</param>
		/// <param name="center">Center.</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="colorGradient">Color gradient.</param>
		/// <param name="xMin">Domain lower bound.</param>
		/// <param name="xMax">Domain upper bound.</param>
		/// <param name="yMin">Range lower bound.</param>
		/// <param name="yMax">Range upper bound.</param>
		/// <param name="maxLabel">Max label.</param>
		/// <param name="minLabel">Minimum label.</param>
		/// <param name="xLabel">X label.</param>
		/// <param name="yLabel">Y label.</param>
		/// <param name="graphOffset">Graph offset.</param>
		/// <param name="shapeMode">Shape mode.</param>
		private static AnimationCurve DoFalloffHandle(
			int baseId, AnimationCurve curve,
			Vector3 center, Quaternion orientation,
			ColorGradient colorGradient,
			float xMin, float xMax, float yMin, float yMax,
			string maxLabel, string minLabel, string xLabel, string yLabel,
			Quaternion graphOffset, ShapeMode shapeMode
		)
		{
			// store the old color
			Color oldColor = Handles.color;
			// store the old matrix
			Matrix4x4 oldMatrix = Handles.matrix;
			// set the handle matrix
			Handles.matrix *= Matrix4x4.TRS(center, orientation, Vector3.one);
			// duplicate curve
			curve = new AnimationCurve(curve.keys);
			// create keyframe handles
			float high, low;
			curve.GetRange(out high, out low, 0.01f);
			Color baseColor =
				(low > 0f ? colorGradient.MaxColor : colorGradient.MinColor) * SceneGUI.CurrentAlphaScalar;
			DoKeyframeHandles(
				baseId, curve, graphOffset, colorGradient, baseColor,
				high, low,
				xMin, xMax, yMin, yMax,
				minLabel, maxLabel, xLabel, yLabel,
				shapeMode
			);
			// fill the disc and graph
			FillDiscAndGraph(
				curve, graphOffset, colorGradient, baseColor, high, low, xMin, xMax, yMin, yMax, shapeMode
			);
			// reset handle matrix
			Handles.matrix = oldMatrix;
			// reset handle color
			Handles.color = oldColor;
			// return result
			return curve;
		}

		/// <summary>
		/// Displays the keyframe handles.
		/// </summary>
		/// <param name="baseId">Base identifier. Each handle has its own unique hash based off this value.</param>
		/// <param name="curve">Curve.</param>
		/// <param name="graphOffset">Graph offset.</param>
		/// <param name="colorGradient">Color gradient.</param>
		/// <param name="baseColor">The preferred color for a uniform curve.</param>
		/// <param name="high">The upper bound of the curve's current range.</param>
		/// <param name="low">The lower bound of the curve's current range.</param>
		/// <param name="xMin">Domain lower bound.</param>
		/// <param name="xMax">Domain upper bound.</param>
		/// <param name="yMin">Range lower bound.</param>
		/// <param name="yMax">Range upper bound.</param>
		/// <param name="minLabel">Minimum label.</param>
		/// <param name="maxLabel">Max label.</param>
		/// <param name="xLabel">X label.</param>
		/// <param name="yLabel">Y label.</param>
		/// <param name="shapeMode">Shape mode.</param>
		private static void DoKeyframeHandles(
			int baseId, AnimationCurve curve, Quaternion graphOffset, ColorGradient colorGradient, Color baseColor,
			float high, float low,
			float xMin, float xMax, float yMin, float yMax,
			string minLabel, string maxLabel, string xLabel, string yLabel,
			ShapeMode shapeMode
		)
		{
			// a new array of keys
			s_Keyframes.Clear();
			s_Keyframes.AddRange(curve.keys);
			if (s_KeyframesToDelete.ContainsKey(baseId))
			{
				if (s_Keyframes.Count > 1)
				{
					s_Keyframes.RemoveAt(s_KeyframesToDelete[baseId]);
					SceneGUI.RegisterChange();
				}
				s_KeyframesToDelete.Remove(baseId);
			}
			FalloffHandleData handle = GetHandle(baseId);
			if (s_NewKeyframes.ContainsKey(handle.Id))
			{
				Keyframe newKey = s_NewKeyframes[handle.Id];
				s_Keyframes.Add(newKey);
				AnimationCurve c = new AnimationCurve(s_Keyframes.ToArray());
				s_Keyframes.Clear();
				s_Keyframes.AddRange(c.keys);
				for (int i = 0; i < s_Keyframes.Count; ++i)
				{
					if (s_Keyframes[i].time == newKey.time)
					{
						s_LastClickedKeyframeHandleId = ObjectX.GenerateHashCode(baseId, s_KeyframeValueHash, i);
					}
				}
				SceneGUI.RegisterChange();
				s_NewKeyframes.Remove(handle.Id);
			}
			float scaleFactor = shapeMode == ShapeMode.NormalizedGraph ?
				s_Keyframes[s_Keyframes.Count - 1].time : 1f;
			// create a handle at each keyframe
			if (yMax != Mathf.Infinity && yMin != Mathf.NegativeInfinity)
			{
				high = yMax;
				low = yMin;
			}
			bool isMinMax = Mathf.Abs(high - low) < 0.000001f;
			float rangeDiv = 1f / (high - low);
			for (int i = 0; i < s_Keyframes.Count; ++i)
			{
				Keyframe key = s_Keyframes[i];
				float time = key.time;
				float val = key.value;
				// compute color for the handle based on the value at this time
				Color color = baseColor;
				if (!isMinMax)
				{
					color = colorGradient.Evaluate((s_Keyframes[i].value - low) * rangeDiv) *
						SceneGUI.CurrentAlphaScalar;
				}
				Handles.color = color;
				int id = ObjectX.GenerateHashCode(baseId, s_KeyframeTimeHash, i);
				// create concentric wire disc handles
				if (shapeMode == ShapeMode.NoGraph)
				{
					int wireHandleID = ObjectX.GenerateHashCode(id, s_WireDiscHash);
					time = DiscHandles.WireDisc(
						id,
						time,
						Vector3.zero,
						Quaternion.identity,
						s_LastClickedKeyframeHandleId != wireHandleID ?
							null : CreateLabel(
								key, shapeMode, xLabel, yLabel, minLabel, maxLabel, i == 0, i == curve.length - 1
							)
					);
					if (GUIUtility.hotControl == wireHandleID)
					{
						s_LastClickedKeyframeHandleId = wireHandleID;
					}
				}
				// create graph handles and labels
				else
				{
					// create keyframe handle
					id = ObjectX.GenerateHashCode(baseId, s_KeyframeValueHash, i);
					s_CircleCapOffset = Quaternion.Inverse(graphOffset);
					Vector3 handlePosition = graphOffset * new Vector3(0f, val * scaleFactor, time);
					if (s_LastClickedKeyframeHandleId == id)
					{
						Handles.Label(
							handlePosition,
							CreateLabel(
								key, shapeMode, xLabel, yLabel, minLabel, maxLabel, i == 0, i == curve.length - 1
							)
						);
					}
					Vector3 newHandlePosition = Handles.Slider2D(
						id,
						handlePosition,
						graphOffset * Vector3.right,
						graphOffset * Vector3.forward,
						graphOffset * Vector3.up,
						SceneGUI.GetFixedHandleSize(Vector3.zero, SceneGUI.DotHandleSize),
						Handles.DotCap,
						Vector2.zero
					);
					newHandlePosition = Vector3.Scale(
						new Vector3(1f, 1f / scaleFactor, 1f), Quaternion.Inverse(graphOffset) * newHandlePosition
					);
					time = newHandlePosition.z;
					val = newHandlePosition.y;
					// store keyframe handle id if it was clicked
					if (GUIUtility.hotControl == id)
					{
						s_LastClickedKeyframeHandleId = id;
						s_LastClickedKeyframeHandleIndex = i;
						GUIUtility.keyboardControl = id;
					}
					// if a keyframe handle was most recently clicked, display tangent handles
					if (s_LastClickedKeyframeHandleId == id)
					{
						Vector3 xAxis = graphOffset * Vector3.forward;
						Vector3 yAxis = graphOffset * Vector3.up * scaleFactor;
						handlePosition = xAxis * time + yAxis * val;
						float handleSize =
							HandleUtility.GetHandleSize(Handles.matrix.MultiplyPoint(handlePosition)) * 0.25f;
						Vector3 inTangentHandle =
							handlePosition - (xAxis + yAxis * key.inTangent).normalized * handleSize;
						Vector3 outTangentHandle =
							handlePosition + (xAxis + yAxis * key.outTangent).normalized * handleSize;
						Handles.DrawLine(handlePosition, inTangentHandle);
						Handles.DrawLine(handlePosition, outTangentHandle);
						int inHash = ObjectX.GenerateHashCode(id, s_InTangentHash);
						Vector3 direction = Vector3.zero;
						Vector3.OrthoNormalize(ref xAxis, ref yAxis, ref direction);
						Vector3 newInHandle = Handles.Slider2D(
							inHash,
							inTangentHandle,
							direction,
							xAxis,
							yAxis,
							handleSize * 0.1f,
							Handles.RectangleCap,
							Vector2.zero
						) - handlePosition;
						float newInTangent =
							Vector3.Dot(newInHandle, yAxis) / Vector3.Dot(newInHandle, xAxis) / scaleFactor;
						int outHash = ObjectX.GenerateHashCode(id, s_OutTangentHash);
						Vector3 newOutHandle = Handles.Slider2D(
							outHash,
							outTangentHandle,
							direction, xAxis,
							yAxis,
							handleSize * 0.1f,
							Handles.RectangleCap,
							Vector2.one
						) - handlePosition;
						float newOutTangent =
							Vector3.Dot(newOutHandle, yAxis) / Vector3.Dot(newOutHandle, xAxis) / scaleFactor;
						// apply edits to tangents
						bool uniteTangents = key.inTangent == key.outTangent && !Event.current.alt;
						if (GUIUtility.hotControl == inHash)
						{
							if (Event.current.clickCount == 2)
							{
								key.outTangent = key.inTangent;
								SceneGUI.RegisterChange();
							}
							else
							{
								if (Event.current.shift)
								{
									newInTangent = SnapTangent(newInTangent);
								}
								key.inTangent = newInTangent;
								if (uniteTangents)
								{
									key.outTangent = key.inTangent;
								}
							}
						}
						else if (GUIUtility.hotControl == outHash)
						{
							if (Event.current.clickCount == 2)
							{
								key.inTangent = key.outTangent;
								SceneGUI.RegisterChange();
							}
							else
							{
								if (Event.current.shift)
								{
									newOutTangent = SnapTangent(newOutTangent);
								}
								key.outTangent = newOutTangent;
								if (uniteTangents)
								{
									key.inTangent = key.outTangent;
								}
							}
						}
					}
				}
				// store the value to the copy of keyframes
				key.time = Mathf.Clamp(time, xMin, xMax);
				key.value = Mathf.Clamp(val, yMin, yMax);
				s_Keyframes[i] = key;
			}
			// delete selected keyframe if pressing delete or backspace key
			if (
				GUIUtility.keyboardControl == s_LastClickedKeyframeHandleId &&
				s_LastClickedKeyframeHandleIndex > -1 &&
				Event.current.type == EventType.KeyUp &&
				(Event.current.keyCode == KeyCode.Delete || Event.current.keyCode == KeyCode.Backspace)
			)
			{
				QueueDeleteKeyframe(new DeleteKeyframeMenuData(baseId, s_LastClickedKeyframeHandleIndex));
			}
			/*
			bool isKeySelected =
				GUIUtility.keyboardControl == s_LastClickedKeyframeHandleId && s_LastClickedKeyframeHandleIndex > -1;
			*/
			if (
				!Event.current.alt &&
				Event.current.type == EventType.MouseDown &&
				Event.current.button == 1 &&
				Camera.current.pixelRect.Contains(Event.current.mousePosition)
			)
			{
				GenericMenu menu = new GenericMenu();
				s_ActiveHandles[baseId].GraphMatrix = Handles.matrix * Matrix4x4.TRS(
					Vector3.zero,
					graphOffset * Quaternion.AngleAxis(-90f, Vector3.up),
					new Vector3(1f, -scaleFactor * high, 1f)
				);
				/*
				if (isKeySelected)
				{
					menu.AddItem(
						new GUIContent("Delete Key"),
						false,
						QueueDeleteKeyframe,
						new DeleteKeyframeData(baseId, s_LastClickedKeyframeHandleId)
					);
				}
				*/
				float nearestDistance = Mathf.Infinity;
				int nearestControl = 0;
				foreach (KeyValuePair<int, FalloffHandleData> kv in s_ActiveHandles)
				{
					float sqrMag = (kv.Value.ScreenPosition - Event.current.mousePosition).sqrMagnitude;
					if (sqrMag < nearestDistance)
					{
						nearestControl = kv.Key;
						nearestDistance = sqrMag;
					}
				}
				if (nearestControl != 0)
				{
					s_ActiveHandles[nearestControl].Curve = new AnimationCurve(s_Keyframes.ToArray());
					menu.AddItem(
						new GUIContent("Add Key"),
						false,
						QueueAddKeyframe,
						new AddKeyframeMenuData(
							s_ActiveHandles[nearestControl].Id, Event.current.mousePosition, xMin, xMax
						)
					);
					Rect rect = new Rect(0f, 0f, 95f, 25f);
					rect.position = Event.current.mousePosition;
					rect.position -= SceneGUI.SceneViewTabHeight * Vector2.up * 0.5f;
					menu.DropDown(rect);
				}
			}
			// cache last known screen position of handle as needed
			switch (Event.current.type)
			{
			case EventType.MouseUp: // a bit of a hack, but can be reasonably certain handles won't change without this
				s_ActiveHandles.Clear();
				break;
			default:
				handle.ScreenPosition =
					Camera.current.WorldToScreenPoint(Handles.matrix.GetPosition()) +
					SceneGUI.SceneViewTabHeight * Vector3.up * 2f;
				break;
			}
			// set the curve's updated keyframes
			curve.keys = s_Keyframes.ToArray();
		}

		/// <summary>
		/// Fills the disc and graph.
		/// </summary>
		/// <param name="curve">Curve.</param>
		/// <param name="graphOffset">Graph offset.</param>
		/// <param name="colorGradient">Color gradient.</param>
		/// <param name="baseColor">The preferred color for a uniform curve.</param>
		/// <param name="high">The upper bound of the curve's current range.</param>
		/// <param name="low">The lower bound of the curve's current range.</param>
		/// <param name="xMin">Domain lower bound.</param>
		/// <param name="xMax">Domain upper bound.</param>
		/// <param name="yMin">Range lower bound.</param>
		/// <param name="yMax">Range upper bound.</param>
		/// <param name="shapeMode">Shape mode.</param>
		private static void FillDiscAndGraph(
			AnimationCurve curve, Quaternion graphOffset, ColorGradient colorGradient, Color baseColor,
			float high, float low,
			float xMin, float xMax, float yMin, float yMax,
			ShapeMode shapeMode
		)
		{
			// early out if not repaint
			if (
				Event.current.type != EventType.Repaint ||
				FalloffHandles.DiscMesh == null || // NOTE: ensures meshes get drawn on first layout phase
				FalloffHandles.GraphMesh == null
			)
			{
				return;
			}
			// compute disc mesh colors and vertices
			float minTime = curve[0].time;
			float maxTime = curve[curve.keys.Length - 1].time;
			if (yMax != Mathf.Infinity && yMin != Mathf.NegativeInfinity)
			{
				high = yMax;
				low = yMin;
			}
			bool isMinMax = Mathf.Abs(high - low) < 0.000001f;
			float rangeDiv = (high == low) ? 0.0000001f : (1f / (high - low));
			float timeDiv = 1f / (k_GradientMeshSubdivisions - 1) * (maxTime - minTime);
			for (int r = 0; r <= k_GradientMeshSubdivisions; ++r)
			{
				float parameter = Mathf.Max(0f, minTime + (r - 1) * timeDiv);
				Color col = baseColor;
				if (!isMinMax)
				{
					col = colorGradient.Evaluate((curve.Evaluate(parameter) - low) * rangeDiv) *
						SceneGUI.CurrentAlphaScalar;
				}
				col.a *= SceneGUI.FillAlphaScalar * 2f;
				for (int c = 0; c <= k_DiscMeshSubdivisions; ++c)
				{
					int i = c * (k_GradientMeshSubdivisions + 1) + r;
					s_DiscMeshColors[i] = col;
					s_DiscMeshVertices[i] = s_DiscMeshNormalizedVertices[i].normalized * parameter;
				}
			}
			// set disc mesh properties
			FalloffHandles.DiscMesh.colors = s_DiscMeshColors;
			FalloffHandles.DiscMesh.vertices = s_DiscMeshVertices;
			// draw disc mesh
			Graphics.DrawMeshNow(FalloffHandles.DiscMesh, Handles.matrix);
			if (shapeMode == ShapeMode.NoGraph)
			{
				return;
			}
			// set graph matrix
			Handles.matrix *= Matrix4x4.TRS(
				Vector3.zero,
				graphOffset,
				shapeMode == ShapeMode.NormalizedGraph ? new Vector3(1f, maxTime, 1f) : Vector3.one
			);
			// compute graph mesh colors and vertices
			timeDiv = 1f / k_GraphMeshSubdivisionCount * maxTime;
			for (int i = 0; i <= k_GraphMeshSubdivisionCount; ++i)
			{
				s_GraphMeshVertices[i * 2].z = s_GraphMeshVertices[i * 2 + 1].z = i * timeDiv;
				s_GraphMeshVertices[i * 2 + 1].y = curve.Evaluate(s_GraphMeshVertices[i * 2].z);
				Color col = baseColor;
				if (!isMinMax)
				{
					col = colorGradient.Evaluate((s_GraphMeshVertices[i * 2 + 1].y - low) * rangeDiv) *
						SceneGUI.CurrentAlphaScalar;
				}
				if (i * 2 > 0)
				{
					Handles.color = col;
					Handles.DrawLine(s_GraphMeshVertices[i * 2 - 1], s_GraphMeshVertices[i * 2 + 1]);
					s_GraphLineVertices[i] = s_GraphMeshVertices[i * 2 - 1];
				}
				col.a *= SceneGUI.FillAlphaScalar * 2f;
				s_GraphMeshColors[i * 2] = s_GraphMeshColors[i * 2 + 1] = col;
			}
			// set graph mesh properties
			GraphMesh.colors = s_GraphMeshColors;
			GraphMesh.vertices = s_GraphMeshVertices;
			// draw graph mesh
			Graphics.DrawMeshNow(FalloffHandles.GraphMesh, Handles.matrix);
		}

		/// <summary>
		/// Gets the handle data for the specified base control ID.
		/// </summary>
		/// <returns>The handle data.</returns>
		/// <param name="id">Base control ID of the handle..</param>
		private static FalloffHandleData GetHandle(int id)
		{
			if (!s_ActiveHandles.ContainsKey(id))
			{
				s_ActiveHandles[id] = new FalloffHandleData(id);
			}
			return s_ActiveHandles[id];
		}

		/// <summary>
		/// Queues an add keyframe operation.
		/// </summary>
		/// <param name="menuData">Menu data.</param>
		private static void QueueAddKeyframe(object menuData)
		{
			AddKeyframeMenuData data = (AddKeyframeMenuData)menuData;
			Ray ray = Camera.current.ScreenPointToRay(data.MousePosition);
			FalloffHandleData handle = GetHandle(data.HandleId);
			Plane p = new Plane(
				inNormal: handle.GraphMatrix.MultiplyVector(Vector3.back),
				inPoint: handle.GraphMatrix.GetPosition()
			);
			float distance;
			p.Raycast(ray, out distance);
			Vector3 point = handle.GraphMatrix.inverse.MultiplyPoint(ray.GetPoint(distance));
			float inTangent, outTangent;
			float t = Mathf.Clamp(point.x, data.XMin, data.XMax);
			float value = handle.Curve.Evaluate(t, out inTangent, out outTangent);
			inTangent = outTangent = Mathf.Lerp(inTangent, outTangent, 0.5f);
			s_NewKeyframes[handle.Id] = new Keyframe(t, value, inTangent, outTangent);
		}

		/// <summary>
		/// Queues a delete keyframe operation.
		/// </summary>
		/// <param name="menuData">Menu data.</param>
		private static void QueueDeleteKeyframe(object menuData)
		{
			DeleteKeyframeMenuData data = (DeleteKeyframeMenuData)menuData;
			s_KeyframesToDelete[data.HandleId] = data.KeyframeIndex;
			s_LastClickedKeyframeHandleIndex = -1;
			s_LastClickedKeyframeHandleId = 0;
			GUIUtility.keyboardControl = 0;
			if (Event.current != null)
			{
				Event.current.Use();
			}
		}

		/// <summary>
		/// Snaps the tangent to the nearest snap angle.
		/// </summary>
		/// <returns>The tangent snapped to the nearest snap angle.</returns>
		/// <param name="tangent">Tangent.</param>
		private static float SnapTangent(float tangent)
		{
			if (tangent == Mathf.Infinity || tangent == Mathf.NegativeInfinity)
			{
				return tangent;
			}
			float inputAngle = Mathf.Atan(tangent);
			float snapAngle = s_TangentSnapAngles[0];
			float distanceToSnapAngle = Mathf.Infinity;
			foreach (float testAngle in s_TangentSnapAngles)
			{
				float distanceToTestAngle = Mathf.Abs(testAngle - inputAngle);
				if (distanceToTestAngle < distanceToSnapAngle)
				{
					snapAngle = testAngle;
					distanceToSnapAngle = distanceToTestAngle;
				}
			}
			return Mathf.Tan(snapAngle);
		}
		
		/// <summary>
		/// Displays a falloff sphere handle.
		/// </summary>
		/// <returns>The modified AnimationCurve.</returns>
		/// <param name="baseId">Base identifier. Each handle has its own unique hash based off this value.</param>
		/// <param name="curve">Curve.</param>
		/// <param name="center">Center.</param>
		/// <param name="maxLabel">Max label.</param>
		/// <param name="minLabel">Minimum label.</param>
		/// <param name="xLabel">X label.</param>
		/// <param name="yLabel">Y label.</param>
		/// <param name="xMin">Domain lower bound.</param>
		/// <param name="xMax">Domain upper bound.</param>
		public static AnimationCurve Sphere(
			int baseId, AnimationCurve curve, Vector3 center,
			string maxLabel = "", string minLabel = "", string xLabel = "", string yLabel = "",
			float xMin = 0f, float xMax = Mathf.Infinity
		)
		{
			return Sphere(baseId, curve, center, s_DefaultColorGradient, maxLabel, minLabel, xLabel, yLabel);
		}
		
		/// <summary>
		/// Displays a falloff sphere handle.
		/// </summary>
		/// <returns>The modified AnimationCurve.</returns>
		/// <param name="baseId">Base identifier. Each handle has its own unique hash based off this value.</param>
		/// <param name="curve">Curve.</param>
		/// <param name="center">Center.</param>
		/// <param name="colorGradient">Color gradient.</param>
		/// <param name="maxLabel">Max label.</param>
		/// <param name="minLabel">Minimum label.</param>
		/// <param name="xLabel">X label.</param>
		/// <param name="yLabel">Y label.</param>
		/// <param name="xMin">Domain lower bound.</param>
		/// <param name="xMax">Domain upper bound.</param>
		public static AnimationCurve Sphere(
			int baseId, AnimationCurve curve, Vector3 center, ColorGradient colorGradient,
			string maxLabel = "", string minLabel = "", string xLabel = "", string yLabel = "",
			float xMin = 0f, float xMax = Mathf.Infinity
		)
		{
			return DoFalloffHandle(
				baseId,
				curve,
				center,
				DiscHandles.GetScreenSpaceOrientationForDisc(center),
				colorGradient,
				xMin, xMax, Mathf.NegativeInfinity, Mathf.Infinity,
				maxLabel, minLabel, xLabel, yLabel,
				s_SphereGraphOffset,
				ShapeMode.NoGraph
			);
		}
		
		/// <summary>
		/// Displays a falloff sphere graph handle.
		/// </summary>
		/// <returns>The modified AnimationCurve.</returns>
		/// <param name="baseId">Base identifier. Each handle has its own unique hash based off this value.</param>
		/// <param name="curve">Curve.</param>
		/// <param name="center">Center.</param>
		/// <param name="maxLabel">Max label.</param>
		/// <param name="minLabel">Minimum label.</param>
		/// <param name="xLabel">X label.</param>
		/// <param name="yLabel">Y label.</param>
		/// <param name="xMin">Domain lower bound.</param>
		/// <param name="xMax">Domain upper bound.</param>
		/// <param name="yMin">Range lower bound.</param>
		/// <param name="yMax">Range upper bound.</param>
		/// <param name="normalizeValues">
		/// If set to <see langword="true"/> then scale the value handles by the size of the key handles.
		/// </param>
		public static AnimationCurve SphereGraph(
			int baseId, AnimationCurve curve, Vector3 center,
			string maxLabel = "", string minLabel = "", string xLabel = "", string yLabel = "",
			float xMin = 0f, float xMax = Mathf.Infinity,
			float yMin = Mathf.NegativeInfinity, float yMax = Mathf.Infinity,
			bool normalizeValues = false
		)
		{
			return SphereGraph(
				baseId, curve, center, s_DefaultColorGradient,
				maxLabel, minLabel, xLabel, yLabel,
				xMin, xMax, yMin, yMax
			);
		}
		
		/// <summary>
		/// Displays a falloff sphere graph handle.
		/// </summary>
		/// <returns>The modified AnimationCurve.</returns>
		/// <param name="baseId">Base identifier. Each handle has its own unique hash based off this value.</param>
		/// <param name="curve">Curve.</param>
		/// <param name="center">Center.</param>
		/// <param name="colorGradient">Color gradient.</param>
		/// <param name="maxLabel">Max label.</param>
		/// <param name="minLabel">Minimum label.</param>
		/// <param name="xLabel">X label.</param>
		/// <param name="yLabel">Y label.</param>
		/// <param name="xMin">Domain lower bound.</param>
		/// <param name="xMax">Domain upper bound.</param>
		/// <param name="yMin">Range lower bound.</param>
		/// <param name="yMax">Range upper bound.</param>
		/// <param name="normalizeValues">
		/// If set to <see langword="true"/> then scale the value handles by the size of the key handles.
		/// </param>
		public static AnimationCurve SphereGraph(
			int baseId, AnimationCurve curve, Vector3 center, ColorGradient colorGradient,
			string maxLabel = "", string minLabel = "", string xLabel = "", string yLabel = "",
			float xMin = 0f, float xMax = Mathf.Infinity,
			float yMin = Mathf.NegativeInfinity, float yMax = Mathf.Infinity,
			bool normalizeValues = false
		)
		{
			return DoFalloffHandle(
				baseId,
				curve,
				center,
				DiscHandles.GetScreenSpaceOrientationForDisc(center),
				colorGradient,
				xMin, xMax, yMin, yMax,
				maxLabel, minLabel, xLabel, yLabel,
				s_SphereGraphOffset,
				normalizeValues ? ShapeMode.NormalizedGraph : ShapeMode.Graph
			);
		}
	}
}