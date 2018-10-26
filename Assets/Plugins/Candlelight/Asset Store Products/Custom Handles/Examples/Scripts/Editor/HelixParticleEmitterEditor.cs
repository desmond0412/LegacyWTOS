// 
// HelixParticleEmitterEditor.cs
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
	/// A custom editor for the <see cref="HelixParticleEmitter"/> component. See <see cref="HelixHandles"/> for other
	/// notes and limitations.
	/// </summary>
	[CustomEditor(typeof(HelixParticleEmitter)), CanEditMultipleObjects]
	public class HelixParticleEmitterEditor : BaseEditor<HelixParticleEmitter>
	{
		#region Preferences
		private static EditorPreference<Color, HelixParticleEmitterEditor> s_HandleColorPreference =
			new EditorPreference<Color, HelixParticleEmitterEditor>("handleColor", Color.yellow);
		private static EditorPreference<bool, HelixParticleEmitterEditor> s_HandleTogglePreference =
			EditorPreference<bool, HelixParticleEmitterEditor>.ForToggle("handle", true);
		#endregion

		/// <summary>
		/// Gets the product category. Replace this property in a subclass to specify a location in the preference menu.
		/// </summary>
		/// <value>The product category.</value>
		new protected static AssetStoreProduct ProductCategory { get { return AssetStoreProduct.CustomHandles; } }

		/// <summary>
		/// A convenience menu item parallel to GameObject -> Create Other -> Particle System.
		/// </summary>
		[MenuItem("GameObject/Create Other/Helix Particle System")]
		public static void CreateHelixParticleSystem()
		{
			// create a particle system to get its material
			EditorApplication.ExecuteMenuItem("GameObject/Create Other/Particle System");
			Material m = Selection.gameObjects[0].GetComponent<Renderer>().sharedMaterial;
			GameObject.DestroyImmediate(Selection.gameObjects[0]);
			// create an empty GameObject and add the components for the helix particle system
			EditorApplication.ExecuteMenuItem("GameObject/Create Empty");
			GameObject go = Selection.gameObjects[0];
			go.name = "Helix Particle System";
			go.AddComponent<HelixParticleEmitter>();
			go.AddComponent<ParticleAnimator>();
			go.AddComponent<ParticleRenderer>();
			go.GetComponent<Renderer>().sharedMaterial = m;
		}

		/// <summary>
		/// Displays the handle preferences. They will be displayed in the preference menu and the top of the inspector.
		/// </summary>
		new protected static void DisplayHandlePreferences()
		{
			EditorGUIX.DisplayHandlePropertyEditor("Helix", s_HandleTogglePreference, s_HandleColorPreference);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="HelixParticleEmitterEditor"/> implements scene GUI handles.
		/// </summary>
		/// <value><see langword="true"/> if implements scene GUI handles; otherwise, <see langword="false"/>.</value>
		protected override bool ImplementsSceneGUIHandles { get { return true; } }
		/// <summary>
		/// Gets a value indicating whether this <see cref="HelixParticleEmitterEditor"/> implements scene GUI overlay.
		/// </summary>
		/// <value><see langword="true"/> if implements scene GUI overlay; otherwise, <see cref="false"/>.</value>
		protected override bool ImplementsSceneGUIOverlay { get { return false; } }
		
		/// <summary>
		/// Displays the viewport handle.
		/// </summary>
		protected override void DisplaySceneGUIHandles()
		{
			base.DisplaySceneGUIHandles();
			Helix newHelix = null;
			if (
				SceneGUI.BeginHandles(this.Target, "Change Helix Particle Emitter") &&
				s_HandleTogglePreference.CurrentValue
			)
			{
				newHelix = HelixHandles.WireHelix(
					target.GetHashCode(),
					this.Target.Helix,
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