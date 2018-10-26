// 
// Plant.cs
// 
// Copyright (c) 2011-2015, Candlelight Interactive, LLC
// All rights reserved.
// 
// This file is licensed according to the terms of the Unity Asset Store EULA:
// http://download.unity3d.com/assetstore/customer-eula.pdf

using UnityEngine;
using System.Collections.Generic;

namespace Candlelight.Examples.CustomHandles
{
	/// <summary>
	/// An example usage for <see cref="HelixMeshRibbon"/>.
	/// </summary>
	public class Plant : MonoBehaviour
	{
		/// <summary>
		/// A class to describe a plant leaf.
		/// </summary>
		[System.Serializable]
		public struct Leaf
		{
			#region Backing Fields
			[SerializeField, UnityEngine.Serialization.FormerlySerializedAs("frontface")]
			private HelixMeshRibbon m_FrontFace;
			[SerializeField, UnityEngine.Serialization.FormerlySerializedAs("backface")]
			private HelixMeshRibbon m_BackFace;
			#endregion

			/// <summary>
			/// Gets the back face.
			/// </summary>
			/// <value>The back face.</value>
			public HelixMeshRibbon BackFace { get { return m_BackFace; } }
			/// <summary>
			/// Gets the front face.
			/// </summary>
			/// <value>The front face.</value>
			public HelixMeshRibbon FrontFace { get { return m_FrontFace; } }

			/// <summary>
			/// Initializes a new instance of the <see cref="PlantLeaf"/> struct.
			/// </summary>
			/// <param name="front">Front.</param>
			/// <param name="back">Back.</param>
			public Leaf(HelixMeshRibbon front, HelixMeshRibbon back)
			{
				m_BackFace = back;
				m_FrontFace = front;
			}
		}

		#region Backing Fields
		[SerializeField, UnityEngine.Serialization.FormerlySerializedAs("leaves")]
		private List<Leaf> m_Leaves = new List<Leaf>();
		#endregion

		/// <summary>
		/// Gets the leaves.
		/// </summary>
		/// <returns>The number of leaves.</returns>
		/// <param name="leaves">Leaves.</param>
		public int GetLeaves(ref List<Leaf> leaves)
		{
			leaves = leaves ?? new List<Leaf>();
			leaves.Clear();
			leaves.AddRange(m_Leaves);
			return leaves.Count;
		}

		/// <summary>
		/// Sets the leaves.
		/// </summary>
		/// <param name="value">Value.</param>
		public void SetLeaves(IEnumerable<Leaf> value)
		{
			value = value ?? new Leaf[0];
			m_Leaves.Clear();
			m_Leaves.AddRange(value);
		}
	}
}