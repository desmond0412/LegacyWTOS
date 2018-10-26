// 
// CharacterMoveToEditor.cs
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
	/// A custom editor for the <see cref="CharacterMoveTo"/> component.
	/// </summary>
	[CustomEditor(typeof(CharacterMoveTo)), CanEditMultipleObjects]
	public class CharacterMoveToEditor : BaseEditor<CharacterMoveTo>
	{
		#region Preferences
		private static EditorPreference<ColorGradient, CharacterMoveToEditor> s_HandleColorPreference =
			new EditorPreference<ColorGradient, CharacterMoveToEditor>(
				"handleColor", new ColorGradient(Color.red, Color.yellow)
			);
		private static EditorPreference<bool, CharacterMoveToEditor> s_HandleTogglePreference =
			EditorPreference<bool, CharacterMoveToEditor>.ForToggle("handle", true);
		#endregion

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
			EditorGUIX.DisplayHandlePropertyEditor("Speed", s_HandleTogglePreference, s_HandleColorPreference);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="CharacterMoveToEditor"/> implements scene GUI handles.
		/// </summary>
		/// <value><see langword="true"/> if implements scene GUI handles; otherwise, <see langword="false"/>.</value>
		protected override bool ImplementsSceneGUIHandles { get { return true; } }
		/// <summary>
		/// Gets a value indicating whether this <see cref="CharacterMoveToEditor"/> implements scene GUI overlay.
		/// </summary>
		/// <value><see langword="true"/> if implements scene GUI overlay; otherwise, <see cref="false"/>.</value>
		protected override bool ImplementsSceneGUIOverlay { get { return false; } }

		/// <summary>
		/// Displays the scene GUI handles.
		/// </summary>
		protected override void DisplaySceneGUIHandles()
		{
			base.DisplaySceneGUIHandles();
			AnimationCurve newSpeedCurve = null;
			if (
				SceneGUI.BeginHandles(this.Target, "Change Movement Speed Curve") &&
				s_HandleTogglePreference.CurrentValue
			)
			{
				Vector3 up = this.Target.transform.InverseTransformDirection(Vector3.up);
				newSpeedCurve = FalloffHandles.DiscGraph(
					target.GetHashCode(),
					this.Target.SpeedCurve,
					Vector3.zero,
					Quaternion.LookRotation(Vector3.forward - Vector3.Dot(Vector3.forward, up) * up, up),
					s_HandleColorPreference.CurrentValue,
					"Run Speed", "Walk Speed", "Distance", "Speed"
				);
			}
			if (SceneGUI.EndHandles())
			{
				this.Target.SpeedCurve = newSpeedCurve;
			}
		}
	}
}