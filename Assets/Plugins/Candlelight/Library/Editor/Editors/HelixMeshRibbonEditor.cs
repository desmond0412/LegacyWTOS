// 
// HelixMeshRibbonEditor.cs
// 
// Copyright (c) 2011-2015, Candlelight Interactive, LLC
// All rights reserved.
// 
// This file is licensed according to the terms of the Unity Asset Store EULA:
// http://download.unity3d.com/assetstore/customer-eula.pdf

using UnityEditor;
using UnityEngine;

namespace Candlelight.Examples.CustomHandles
{
	/// <summary>
	/// A custom editor for the <see cref="HelixMeshRibbon"/> component. See <see cref="HelixHandles"/> for other notes
	/// and limitations.
	/// </summary>
	[CustomEditor(typeof(HelixMeshRibbon)), CanEditMultipleObjects]
	public class HelixMeshRibbonEditor : BaseEditor<HelixMeshRibbon>
	{
		#region Preferences
		private static readonly EditorPreference<Color, HelixMeshRibbonEditor> s_HandleColorPreference =
			new EditorPreference<Color, HelixMeshRibbonEditor>("handleColor", Color.yellow);
		private static readonly EditorPreference<bool, HelixMeshRibbonEditor> s_HandleTogglePreference =
			EditorPreference<bool, HelixMeshRibbonEditor>.ForToggle("handle", true);
		#endregion

		/// <summary>
		/// Gets the color of the handle.
		/// </summary>
		/// <value>The color of the handle.</value>
		public static Color HandleColor { get { return s_HandleColorPreference.CurrentValue; } }
		/// <summary>
		/// Gets a value indicating whether this <see cref="HelixMeshRibbonEditor"/> implements scene GUI handles.
		/// </summary>
		/// <value><see langword="true"/> if implements scene GUI handles; otherwise, <see langword="false"/>.</value>
		protected override bool ImplementsSceneGUIHandles { get { return true; } }
		/// <summary>
		/// Gets a value indicating whether this <see cref="HelixMeshRibbonEditor"/> implements scene GUI overlay.
		/// </summary>
		/// <value><see langword="true"/> if implements scene GUI overlay; otherwise, <see cref="false"/>.</value>
		protected override bool ImplementsSceneGUIOverlay { get { return false; } }
		/// <summary>
		/// Gets a value indicating whether the handle is enabled.
		/// </summary>
		/// <value><see langword="true"/> if the handle is enabled; otherwise, <see langword="false"/>.</value>
		public static bool IsHandleEnabled { get { return s_HandleTogglePreference.CurrentValue; } }
		/// <summary>
		/// Gets the product category. Replace this property in a subclass to specify a location in the preference menu.
		/// </summary>
		/// <value>The product category.</value>
		new protected static AssetStoreProduct ProductCategory { get { return AssetStoreProduct.CustomHandles; } }

		/// <summary>
		/// Displays the handle preferences. They will be displayed in the preference menu and the top of the inspector.
		/// </summary>
		new protected static void DisplayHandlePreferences()
		{
			EditorGUIX.DisplayHandlePropertyEditor("Helix", s_HandleTogglePreference, s_HandleColorPreference);
		}

		/// <summary>
		/// Displays the scene GUI handles.
		/// </summary>
		protected override void DisplaySceneGUIHandles()
		{
			base.DisplaySceneGUIHandles();
			Helix newHelix = null;
			if (SceneGUI.BeginHandles(target, "Change Helix Mesh Ribbon") && s_HandleTogglePreference.CurrentValue)
			{
				newHelix = HelixHandles.MeshRibbon(
					this.Target,
					Vector3.zero,
					Quaternion.identity,
					Vector3.one,
					s_HandleColorPreference.CurrentValue
				);
			}
			if (SceneGUI.EndHandles())
			{
				this.Target.Helix = newHelix;
			}
		}
	}
}