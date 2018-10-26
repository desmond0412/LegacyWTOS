// 
// AwarenessEditor.cs
// 
// Copyright (c) 2011-2015, Candlelight Interactive, LLC
// All rights reserved.
// 
// This file is licensed according to the terms of the Unity Asset Store EULA:
// http://download.unity3d.com/assetstore/customer-eula.pdf

using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace Candlelight.Examples.CustomHandles
{
	/// <summary>
	/// A custom editor for the <see cref="Awareness"/> component.
	/// </summary>
	[CustomEditor(typeof(Awareness)), CanEditMultipleObjects]
	public class AwarenessEditor : BaseEditor<Awareness>
	{
		#region Preferences
		private static readonly EditorPreference<ColorGradient, AwarenessEditor> s_HearingHandleColorPreference =
			new EditorPreference<ColorGradient, AwarenessEditor>(
				"hearingHandleColor", new ColorGradient(new Color(0f, 0f, 0f, 0.5f), Color.white)
			);
		private static readonly EditorPreference<bool, AwarenessEditor> s_HearingHandleTogglePreference =
			EditorPreference<bool, AwarenessEditor>.ForToggle("hearingHandle", true);
		private static readonly EditorPreference<Color, AwarenessEditor> s_VisionHandleColorPreference =
			new EditorPreference<Color, AwarenessEditor>("visionHandleColor", Color.yellow);
		private static readonly EditorPreference<bool, AwarenessEditor> s_VisionHandleTogglePreference =
			EditorPreference<bool, AwarenessEditor>.ForToggle("visionHandle", true);
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
			EditorGUIX.DisplayHandlePropertyEditor(
				"Vision", s_VisionHandleTogglePreference, s_VisionHandleColorPreference
			);
			EditorGUIX.DisplayHandlePropertyEditor<AwarenessEditor>(
				"Hearing", s_HearingHandleTogglePreference, s_HearingHandleColorPreference
			);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="AwarenessEditor"/> implements scene GUI handles.
		/// </summary>
		/// <value><see langword="true"/> if implements scene GUI handles; otherwise, <see langword="false"/>.</value>
		protected override bool ImplementsSceneGUIHandles { get { return true; } }
		/// <summary>
		/// Gets a value indicating whether this <see cref="AwarenessEditor"/> implements scene GUI overlay.
		/// </summary>
		/// <value><see langword="true"/> if implements scene GUI overlay; otherwise, <see cref="false"/>.</value>
		protected override bool ImplementsSceneGUIOverlay { get { return false; } }

		/// <summary>
		/// Displays the scene GUI handles.
		/// </summary>
		protected override void DisplaySceneGUIHandles()
		{
			base.DisplaySceneGUIHandles();
			// hearing
			if (s_HearingHandleTogglePreference.CurrentValue)
			{
				AnimationCurve newHearing = null;
				Matrix4x4 oldMatrix = Handles.matrix;
				Handles.matrix = Matrix4x4.identity;
				if (SceneGUI.BeginHandles(this.Target, "Change Hearing Zone"))
				{
					newHearing = FalloffHandles.SphereGraph(
						target.GetHashCode(),
						this.Target.HearingFalloff,
						this.Target.transform.position,
						s_HearingHandleColorPreference.CurrentValue,
						"", "", "Distance", "Hearing Falloff"
					);
				}
				if (SceneGUI.EndHandles())
				{
					this.Target.HearingFalloff = newHearing;
				}
				Handles.matrix = oldMatrix;
			}
			// vision
			if (s_VisionHandleTogglePreference.CurrentValue)
			{
				float newAngle = this.Target.VisionAngle;
				float newDistance = this.Target.VisionDistance;
				if (SceneGUI.BeginHandles(this.Target, "Change Vision Zone"))
				{
					Color c = Handles.color;
					Handles.color = s_VisionHandleColorPreference.CurrentValue;
					Vector3 up = this.Target.transform.InverseTransformDirection(Vector3.up);
					newAngle = ArcHandles.SolidWedge(
						target.GetHashCode(),
						this.Target.VisionAngle,
						Vector3.zero,
						Quaternion.LookRotation(Vector3.forward - Vector3.Dot(Vector3.forward, up) * up, up),
						ref newDistance,
						string.Format("{0:#} Degrees", this.Target.VisionAngle),
						string.Format("Vision Distance: {0:#.###}", this.Target.VisionDistance)
					);
					Handles.color = c;
				}
				if (SceneGUI.EndHandles())
				{
					this.Target.VisionAngle = newAngle;
					this.Target.VisionDistance = newDistance;
				}
			}
		}
	}
}