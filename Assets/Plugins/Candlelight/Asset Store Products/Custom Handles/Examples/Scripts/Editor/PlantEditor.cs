// 
// PlantEditor.cs
// 
// Copyright (c) 2011-2015, Candlelight Interactive, LLC
// All rights reserved.
// 
// This file is licensed according to the terms of the Unity Asset Store EULA:
// http://download.unity3d.com/assetstore/customer-eula.pdf

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Candlelight.Examples.CustomHandles
{
	/// <summary>
	/// A custom editor for a <see cref="Plant"/> component.
	/// </summary>
	[CustomEditor(typeof(Plant)), CanEditMultipleObjects]
	public class PlantEditor : BaseEditor<Plant>
	{
		#region Shared Allocations
		private List<Plant.Leaf> s_Leaves = new List<Plant.Leaf>(8);
		#endregion

		/// <summary>
		/// Displays the handle preferences. They will be displayed in the preference menu and the top of the inspector.
		/// </summary>
		new protected static void DisplayHandlePreferences()
		{
			typeof(HelixMeshRibbonEditor).GetMethod(
				"DisplayHandlePreferences", ReflectionX.staticBindingFlags
			).Invoke(null, null);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="PlantEditor"/> implements scene GUI handles.
		/// </summary>
		/// <value><see langword="true"/> if implements scene GUI handles; otherwise, <see langword="false"/>.</value>
		protected override bool ImplementsSceneGUIHandles { get { return true; } }
		/// <summary>
		/// Gets a value indicating whether this <see cref="PlantEditor"/> implements scene GUI overlay.
		/// </summary>
		/// <value><see langword="true"/> if implements scene GUI overlay; otherwise, <see cref="false"/>.</value>
		protected override bool ImplementsSceneGUIOverlay { get { return false; } }

		/// <summary>
		/// Displays the scene GUI handles.
		/// </summary>
		protected override void DisplaySceneGUIHandles()
		{
			base.DisplaySceneGUIHandles();
			HashSet<Object> undoObjects = new HashSet<Object>();
			for (int i = 0; i < this.Target.GetLeaves(ref s_Leaves); ++i)
			{
				undoObjects.Add(s_Leaves[i].BackFace);
				undoObjects.Add(s_Leaves[i].FrontFace);
			}
			undoObjects.Remove(null);
			Helix newHelix = null;
			Matrix4x4 oldMatrix = Handles.matrix;
			for (int i = 0; i < s_Leaves.Count; ++i)
			{
				if (s_Leaves[i].FrontFace == null || s_Leaves[i].BackFace == null)
				{
					continue;
				}
				Handles.matrix = s_Leaves[i].FrontFace.transform.localToWorldMatrix;
				if (
					SceneGUI.BeginHandles(undoObjects.ToArray(), "Modify Plant Leaf") &&
					HelixMeshRibbonEditor.IsHandleEnabled
				)
				{
					newHelix = HelixHandles.MeshRibbon(
						s_Leaves[i].FrontFace, 
						Vector3.zero,
						Quaternion.identity,
						Vector3.one,
						HelixMeshRibbonEditor.HandleColor
					);
				}
				if (SceneGUI.EndHandles())
				{
					s_Leaves[i].FrontFace.Helix = newHelix;
					s_Leaves[i].BackFace.Helix = newHelix;
				}
			}
			Handles.matrix = oldMatrix;
		}
	}
}