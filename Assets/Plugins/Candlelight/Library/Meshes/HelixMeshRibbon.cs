// 
// HelixMeshRibbon.cs
// 
// Copyright (c) 2011-2015, Candlelight Interactive, LLC
// All rights reserved.
// 
// This file is licensed according to the terms of the Unity Asset Store EULA:
// http://download.unity3d.com/assetstore/customer-eula.pdf

using UnityEngine;

namespace Candlelight
{
	/// <summary>
	/// A component that creates a mesh ribbon from <see cref="Helix"/> data and automatically adds the mesh to a
	/// <see cref="UnityEngine.MeshFilter"/>. you can optionally enable auto-updating, which lets you adjust the helix
	/// properties in real-time to deform the mesh.
	/// </summary>
	[AddComponentMenu("Mesh/Helix Mesh Ribbon")]
	public class HelixMeshRibbon : MonoBehaviour
	{
		/// <summary>
		/// The previous divisions.
		/// </summary>
		private int m_PreviousDivisions = 32;
		/// <summary>
		/// The previous helix.
		/// </summary>
		private Helix m_PreviousHelix;
		/// <summary>
		/// The previous V-scale.
		/// </summary>
		private float m_PreviousVScale = 0.5f;

		#region Backing Fields
		[SerializeField]
		private MeshFilter m_MeshFilter;
		[SerializeField]
		private Helix m_Helix = new Helix(4f, 0f, 0f);
		[SerializeField, PropertyBackingField]
		private int m_Divisions = 32;
		[SerializeField]
		private float m_VScale = 0.5f;
		[SerializeField]
		private bool m_AutoVScale = true;
		[SerializeField]
		private bool m_AutoUpdate = true;
		#endregion

		/// <summary>
		/// Gets or sets a value specifying whether or not the ribbon should try to update in real-time.
		/// </summary>
		/// <value>A value specifying whether or not the ribbon should try to update in real-time.</value>
		public bool AutoUpdate
		{
			get { return m_AutoUpdate; }
			set { m_AutoUpdate = value; }
		}
		/// <summary>
		/// Gets or sets a value specifying whether or not to automatically normalize the UV scale.
		/// </summary>
		/// <value>A value indicating whether or not to automatically normalize the UV scale.</value>
		public bool AutoVScale
		{
			get { return m_AutoVScale; }
			set { m_AutoVScale = value; }
		}
		/// <summary>
		/// Gets or sets the number of divisions in the ribbon.
		/// </summary>
		/// <value>The number of divisions.</value>
		public int Divisions
		{
			get { return m_Divisions; }
			set { m_Divisions = Mathf.Clamp(value, 1, 32766); } // mesh cannot exceed 65535 vertices
		}
		/// <summary>
		/// Gets or sets the helix.
		/// </summary>
		/// <value>The helix.</value>
		public Helix Helix
		{
			get { return m_Helix; }
			set { m_Helix = value; }
		}
		/// <summary>
		/// Gets or sets the mesh filter.
		/// </summary>
		/// <value>The mesh filter.</value>
		public MeshFilter MeshFilter
		{
			get { return m_MeshFilter; }
			set { m_MeshFilter = value; }
		}
		/// <summary>
		/// Gets or sets the v-scale for the ribbon in its UV layout.
		/// </summary>
		/// <value>The v-scale for the ribbon.</value>
		public float VScale
		{
			get { return m_VScale; }
			set { m_VScale = value; }
		}

		/// <summary>
		/// Try to determine the best vScale based on the ratio of the helix length to the average width.
		/// </summary>
		private void AutoComputeVScale()
		{
			float averageWidth = 0f;
			AnimationCurve width = Helix.Width;
			for (int i=0; i<width.length; ++i)
			{
				averageWidth += width.keys[i].value;
			}
			averageWidth /= width.length;
			VScale = 2f * averageWidth / Helix.Length;
		}
		
		/// <summary>
		/// Creates the mesh ribbon.
		/// </summary>
		private void CreateMeshRibbon()
		{
			// validate data
			Divisions = Mathf.Max(Divisions, 1);
			// compute vertex positions, normals, and uv coordinates
			Vector3[] vertices = new Vector3[2 * (Divisions + 1)];
			Vector3[] normals = new Vector3[vertices.Length];
			Vector2[] uv = new Vector2[vertices.Length];
			// cache divisions
			float oneHalfLengthOverDivisions = 0.5f * Helix.Length / Divisions;
			float oneHalfActualUVScale = 0.5f / VScale / Divisions;
			for (int i=0; i<vertices.Length; i+=2)
			{			
				// compute vertex positions
				float parameter = (float)(i) * oneHalfLengthOverDivisions;
				vertices[i] = Helix.Evaluate(parameter);
				vertices[i+1] = Helix.EvaluateOpposite(parameter);
				// compute uvs
				uv[i] = new Vector2(1f, (float)(i) * oneHalfActualUVScale);
				uv[i+1] = new Vector2(0f, uv[i].y);
			}
			// compute triangles
			int[] triangles = new int[Divisions*6];
			bool isEvenTriangle = true;
			int stepCount = 3;
			for (int i=0; i<triangles.Length; i+=stepCount)
			{
				int start = i / stepCount;
				if (isEvenTriangle)
				{
					triangles[i] = start;
					triangles[i+1] = start + 1;
					triangles[i+2] = start + 2;
				}
				else
				{
					triangles[i] = start + 2;
					triangles[i+1] = start + 1;
					triangles[i+2] = start;
				}
				isEvenTriangle = !isEvenTriangle;
			}
			// create the mesh
			MeshFilter.mesh.Clear();
			MeshFilter.mesh.vertices = vertices;
			MeshFilter.mesh.normals = normals;
			MeshFilter.mesh.triangles = triangles;
			MeshFilter.mesh.uv = uv;
			MeshFilter.mesh.RecalculateNormals();
			MeshFilter.mesh.RecalculateBounds();
			// scale uvs if required
			if (AutoVScale)
			{
				RecalculateUVCoordinates();
			}
		}
		
		/// <summary>
		/// Recompute uv coordinates, as when the vScale value changes.
		/// </summary>
		private void RecalculateUVCoordinates()
		{
			if (AutoVScale)
			{
				AutoComputeVScale();
			}
			Vector2[] uv = MeshFilter.mesh.uv;
			float oneHalfActualUVScale = 0.5f / VScale / Divisions;
			for (int i=0; i<uv.Length; i+=2)
			{
				uv[i].y = uv[i+1].y = (float)(i) * oneHalfActualUVScale;
			}
			MeshFilter.mesh.uv = uv;
		}
		
		/// <summary>
		/// Recompute vertex positions, as when the helix changes.
		/// </summary>
		private void RecalculateVertexPositions()
		{
			Vector3[] vertices = MeshFilter.mesh.vertices;
			float oneHalfLengthOverDivisions = 0.5f * Helix.Length / Divisions;
			for (int i=0; i<vertices.Length; i+=2)
			{
				float parameter = (float)(i) * oneHalfLengthOverDivisions;
				vertices[i] = Helix.Evaluate(parameter);
				vertices[i+1] = Helix.EvaluateOpposite(parameter);
			}
			MeshFilter.mesh.vertices = vertices;
			if (AutoVScale)
			{
				RecalculateUVCoordinates();
			}
		}

		/// <summary>
		/// Initialize
		/// </summary>
		void Start()
		{
			// create the mesh
			if (MeshFilter == null)
			{
				MeshFilter = gameObject.AddComponent<MeshFilter>();
			}
			if (MeshFilter.mesh == null)
			{
				MeshFilter.mesh = new Mesh();
				MeshFilter.mesh.name = name + " Helix Mesh Ribbon";
			}
			CreateMeshRibbon();
			if (AutoVScale)
			{
				AutoComputeVScale();
			}
			// store the old values
			m_PreviousHelix = Helix;
			m_PreviousDivisions = Divisions;
			m_PreviousVScale = VScale;
		}
		
		/// <summary>
		/// Update the parameters if anything changes
		/// </summary>
		void Update()
		{
			// early out if no updating is requested
			if (!AutoUpdate)
			{
				return;
			}
			// create a new mesh if the divisions have changed
			if (m_PreviousDivisions!=Divisions)
			{
				CreateMeshRibbon();
			}
			
			// recompute vertex positions if helix has changed
			if (!Helix.AreEquivalent(m_PreviousHelix, Helix))
			{
				RecalculateVertexPositions();
			}
			// compute uvs if the scale has changed
			if (!AutoVScale && (m_PreviousVScale != VScale))
			{
				RecalculateUVCoordinates();
			}
			// store old values
			m_PreviousHelix = new Helix(m_PreviousHelix);
			m_PreviousDivisions = Divisions;
			m_PreviousVScale = VScale;
		}
	}
}