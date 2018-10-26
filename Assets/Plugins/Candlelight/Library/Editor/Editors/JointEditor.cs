// 
// JointEditor.cs
// 
// Copyright (c) 2015-2016, Candlelight Interactive, LLC
// All rights reserved.
// 
// This file is licensed according to the terms of the Unity Asset Store EULA:
// http://download.unity3d.com/assetstore/customer-eula.pdf
// 
// This file contains a base class for custom editors of Joints.

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Candlelight.Physics
{
#if !UNITY_4_6 && !UNITY_4_7
	/// <summary>
	/// A class to manually override Unity's built-in custom editor for HingeJoint.
	/// </summary>
#if USE_CANDLELIGHT_JOINT_EDITORS
	[CustomEditor(typeof(HingeJoint)), CanEditMultipleObjects, InitializeOnLoad]
#endif
	internal class HingeJointEditor : JointEditor
	{
		/// <summary>
		/// Initializes the <see cref="HingeJointEditor"/> class.
		/// </summary>
		static HingeJointEditor()
		{
			s_BuiltinType =
				ReflectionX.AllTypes.Where(t => t.FullName == "UnityEditor.HingeJointEditor").FirstOrDefault();
			if (s_BuiltinType == null)
			{
				Debug.LogError("Unable to locate builtin editor for HingeJoint.");
			}
		}

		/// <summary>
		/// The type of Unity's built-in editor.
		/// </summary>
		private static readonly System.Type s_BuiltinType;

		/// <summary>
		/// An instance of Unity's built-in editor.
		/// </summary>
		private Editor m_BultinEditor;

		/// <summary>
		/// Create an instance of the built-in editor if it could be found.
		/// </summary>
		protected override void OnEnable()
		{
			base.OnEnable();
			if (s_BuiltinType != null)
			{
				m_BultinEditor = CreateEditor(this.targets, s_BuiltinType);
			}
		}

		/// <summary>
		/// Display the built-in inspector if it could be found.
		/// </summary>
		public override void OnInspectorGUI()
		{
			if (this.target == null)
			{
				return;
			}
			if (this.ImplementsSceneGUIOverlay || this.ImplementsSceneGUIHandles)
			{
				if (EditorGUIX.BeginSceneGUIControlsArea())
				{
					DisplaySceneGUIHandlePreferences();
				}
				EditorGUIX.EndSceneGUIControlsArea();
			}
			if (m_BultinEditor == null)
			{
				EditorGUILayout.HelpBox("Unable to locate built-in editor to draw inspector.", MessageType.Error);
				DrawDefaultInspector();
			}
			else
			{
				m_BultinEditor.OnInspectorGUI();
			}
		}
	}
#endif

	/// <summary>
	/// A custom editor for <see cref="UnityEngine.Joint"/> objects.
	/// </summary>
#if USE_CANDLELIGHT_JOINT_EDITORS
	[CustomEditor(typeof(Joint), true), CanEditMultipleObjects, InitializeOnLoad]
#endif
	public class JointEditor : BaseEditor<Joint>
	{
		/// <summary>
		/// An enum to describe joint type.
		/// </summary>
		private enum JointType { Character, Configurable, Hinge, Spring, Other }

		/// <summary>
		/// Initializes the <see cref="JointEditor"/> class.
		/// </summary>
		static JointEditor()
		{
			UnityFeatureDefineSymbols.SetSymbolForAllBuildTargets(
				s_OverrideBuiltinEditorSymbol, g => s_OverrideBuiltinEditors.CurrentValue
			);
#if USE_CANDLELIGHT_JOINT_EDITORS
			List<CustomEditor> attrs = new List<CustomEditor>(2);
			foreach (
				System.Type type in ReflectionX.AllTypes.Where(
					t => t.Assembly != typeof(Editor).Assembly &&
					t != typeof(JointEditor)
#if UNITY_5
					&& t != typeof(HingeJointEditor)
#endif
				)
			)
			{
				type.GetCustomAttributes(attrs);
				foreach (CustomEditor attr in attrs)
				{
					System.Type inspectedType = attr.GetFieldValue<System.Type>("m_InspectedType");
					if (typeof(Joint).IsAssignableFrom(inspectedType))
					{
						Debug.LogError(
							string.Format(
								"{0} implements a custom editor for {1}, which interferes with that defined in {2}. " +
								"To use that defined in {2}, remove the {3} attribute from {0}. " +
								"Please also contact the plugin vendor to request that they include an option to " +
								"disable their custom editor to avoid such conflicts.",
								type, inspectedType, typeof(JointEditor), typeof(CustomEditor)
							)
						);
					}
				}
			}
#endif
		}

		#region Base Hash Codes
		private static readonly int s_AnchorHandleHash = 511;
		#endregion
		#region Labels
		private static readonly GUIContent s_RotationMotionLabel = new GUIContent("Rotation");
		private static readonly GUIContent s_TranslationMotionLabel = new GUIContent("Translation");
		#endregion
		#region Preferences
		private static readonly EditorPreference<bool, JointEditor> s_AnchorHandlePreference =
			EditorPreference<bool, JointEditor>.ForToggle("anchorHandle", false);
		private static readonly EditorPreference<bool, JointEditor> s_AngularLimitHandlePreference =
			EditorPreference<bool, JointEditor>.ForToggle("angularLimitHandle", true);
		private static readonly EditorPreference<float, JointEditor> s_AngularLimitHandleSizePreference =
			new EditorPreference<float, JointEditor>("angularHandleSize", 0.25f);
		private static readonly EditorPreference<bool, JointEditor> s_AxisHandlePreference =
			EditorPreference<bool, JointEditor>.ForToggle("axisHandle", false);
		private static readonly EditorPreference<bool, JointEditor> s_LinearLimitHandlePreference =
			EditorPreference<bool, JointEditor>.ForToggle("linearLimitHandle", true);
		private static readonly EditorPreference<ConfigurableJointMotion, JointEditor> s_MultiTranslationMotionPreference =
			new EditorPreference<ConfigurableJointMotion, JointEditor>(
				"multiTranslationMotion", ConfigurableJointMotion.Locked
			);
		private static readonly EditorPreference<ConfigurableJointMotion, JointEditor> s_MultiRotationMotionPreference =
			new EditorPreference<ConfigurableJointMotion, JointEditor>(
				"multiRotationMotion",  ConfigurableJointMotion.Limited
			);
		private static readonly EditorPreference<bool, JointEditor> s_OverrideBuiltinEditors =
			new EditorPreference<bool, JointEditor>("overrideBuiltinEditors", true);
		#endregion
		
		/// <summary>
		/// The offset to get from the bindpose orientation to the axis orientation so that the color rings on the
		/// handles match colors on the gizmo axes.
		/// </summary>
		private static readonly Quaternion s_BindposeAxisHandleOffset = Quaternion.AngleAxis(-90f, Vector3.up);
		/// <summary>
		/// A cache of local bindposes for any joints existing in the scene before playing.
		/// </summary>
		private static readonly Dictionary<Joint, Matrix4x4> s_Bindposes = new Dictionary<Joint, Matrix4x4>();
		/// <summary>
		/// The wrap values for the joint frame calculation message.
		/// </summary>
		private static readonly float[] s_JointCalculationMessageWrapValues = new [] { 1482f, 830f, 618f, 507f, 443f };
		/// <summary>
		/// The define symbol to use when overriding the built-in editors is enabled.
		/// </summary>
		private static readonly string s_OverrideBuiltinEditorSymbol = "USE_CANDLELIGHT_JOINT_EDITORS";
		/// <summary>
		/// Gets the primary axis handle direction.
		/// </summary>
		/// <value>The primary axis handle direction.</value>
		private static readonly Vector3 s_PrimaryAxisHandleDirection = Vector3.right;
		/// <summary>
		/// Gets the secondary axis handle direction.
		/// </summary>
		/// <value>The secondary axis handle direction.</value>
		private static readonly Vector3 s_SecondaryAxisHandleDirection = Vector3.up;
		
		/// <summary>
		/// Gets or sets the size of the angular limit handles.
		/// </summary>
		/// <value>The size of the angular limit handles.</value>
		public static float AngularLimitHandleSize
		{
			get { return s_AngularLimitHandleSizePreference.CurrentValue; }
			set { s_AngularLimitHandleSizePreference.CurrentValue = value; }
		}
		/// <summary>
		/// Gets or sets a value indicating whether anchor handles are enabled.
		/// </summary>
		/// <value><see langword="true"/> if anchor handles are enabled; otherwise, <see langword="false"/>.</value>
		public static bool AreAnchorHandlesEnabled
		{
			get { return s_AnchorHandlePreference.CurrentValue; }
			set { s_AnchorHandlePreference.CurrentValue = value; }
		}
		/// <summary>
		/// Gets or sets a value indicating whether angular limit handles are enabled.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if angular limit handles are enabled; otherwise, <see langword="false"/>.
		/// </value>
		public static bool AreAngularLimitHandlesEnabled
		{
			get { return s_AngularLimitHandlePreference.CurrentValue; }
			set { s_AngularLimitHandlePreference.CurrentValue = value; }
		}
		/// <summary>
		/// Gets or sets a value indicating whether axis handles are enabled.
		/// </summary>
		/// <value><see langword="true"/> if axis handles are enabled; otherwise, <see langword="false"/>.</value>
		public static bool AreAxisHandlesEnabled
		{
			get { return s_AxisHandlePreference.CurrentValue; }
			set { s_AxisHandlePreference.CurrentValue = value; }
		}
		/// <summary>
		/// Gets or sets a value indicating whether linear limit handles are enabled.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if linear limit handles are enabled; otherwise, <see langword="false"/>.
		/// </value>
		public static bool AreLinearLimitHandlesEnabled
		{
			get { return s_LinearLimitHandlePreference.CurrentValue; }
			set { s_LinearLimitHandlePreference.CurrentValue = value; }
		}
		
		/// <summary>
		/// Gets the product category. Replace this property in a subclass to specify a location in the preference menu.
		/// </summary>
		/// <value>The product category.</value>
		new protected static AssetStoreProduct ProductCategory
		{
			get
			{
				if (
					ReflectionX.AllTypes.FirstOrDefault(
						t => t.FullName == "Candlelight.Physics.RagdollAnimator"
					) != null
				)
				{
					return AssetStoreProduct.RagdollWorkshop;
				}
				return AssetStoreProduct.CustomHandles;
			}
		}
		
		/// <summary>
		/// Caches all bindposes in the scene.
		/// </summary>
		public static void CacheAllBindposesInScene()
		{
			s_Bindposes.Clear();
			foreach (Joint joint in Object.FindObjectsOfType(typeof(Joint)))
			{
				s_Bindposes.Add(joint, joint.GetBindposeLocalMatrix());
			}
		}

		/// <summary>
		/// Displays the anchor handle property editor.
		/// </summary>
		public static void DisplayAnchorHandlePropertyEditor()
		{
			EditorGUI.BeginChangeCheck();
			{
				s_AnchorHandlePreference.CurrentValue =
					EditorGUIX.DisplayOnOffToggle("Anchor Handle", s_AnchorHandlePreference.CurrentValue);
			}
			if (EditorGUI.EndChangeCheck())
			{
				SceneView.RepaintAll();
			}
		}
		
		/// <summary>
		/// Displays the anchor handles.
		/// </summary>
		/// <returns>
		/// <see langword="true"/>, if anchor handles was displayed, <see langword="false"/> otherwise.
		/// </returns>
		/// <param name="joint">Joint.</param>
		/// <param name="bindpose">Bindpose.</param>
		/// <returns><see langword="true"/> if the handles changed; otherwise, <see langword="false"/>.</returns>
		public static bool DisplayAnchorHandles(Joint joint, Matrix4x4 bindpose)
		{
			Vector3 anchor = joint.anchor;
			if (SceneGUI.BeginHandles(joint, "Change Anchor"))
			{
				Matrix4x4 oldMatrix = Handles.matrix;
				Handles.matrix = Matrix4x4.TRS(joint.transform.position, joint.transform.rotation, Vector3.one);
				anchor = TransformHandles.Translation(
					ObjectX.GenerateHashCode(joint.GetHashCode(), s_AnchorHandleHash), anchor, Quaternion.identity, 0.5f
				);
				Handles.matrix = oldMatrix;
			}
			if (SceneGUI.EndHandles())
			{
				joint.anchor = anchor;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Displays the angular limit handle property editor.
		/// </summary>
		public static void DisplayAngularLimitHandlePropertyEditor()
		{
			EditorGUIX.DisplayHandlePropertyEditor(
				"Angular Limit", s_AngularLimitHandlePreference, s_AngularLimitHandleSizePreference
			);
		}

		/// <summary>
		/// Displays the angular limit handles.
		/// </summary>
		/// <param name="joint">Joint.</param>
		/// <param name="bindpose">Bindpose.</param>
		/// <returns><see langword="true"/> if the handles changed; otherwise, <see langword="false"/>.</returns>
		public static bool DisplayAngularLimitHandles(Joint joint, Matrix4x4 bindpose)
		{
			if (joint == null)
			{
				return false;
			}
			Matrix4x4 oldMatrix = Handles.matrix;
			Handles.matrix = Matrix4x4.identity;
			bool result = false;
			if (joint is ConfigurableJoint)
			{
				JointAngularLimits newAngularLimits = new JointAngularLimits(joint as ConfigurableJoint);
				if (SceneGUI.BeginHandles(joint, "Change Angular Limits"))
				{
					newAngularLimits = JointHandles.AngularLimit(
						joint as ConfigurableJoint, bindpose, AngularLimitHandleSize
					);
				}
				if (SceneGUI.EndHandles())
				{
					newAngularLimits.ApplyToJoint(joint as ConfigurableJoint);
					result = true;
				}
			}
			else if (joint is CharacterJoint)
			{
				JointAngularLimits newAngularLimits = new JointAngularLimits(joint as CharacterJoint);
				if (SceneGUI.BeginHandles(joint, "Change Angular Limits"))
				{
					newAngularLimits = JointHandles.AngularLimit(
						joint as CharacterJoint, bindpose, AngularLimitHandleSize
					);
				}
				if (SceneGUI.EndHandles())
				{
					newAngularLimits.ApplyToJoint(joint as CharacterJoint);
					result = true;
				}
			}
			else if (joint is HingeJoint)
			{
				JointLimits newLimits = (joint as HingeJoint).limits;
				if (SceneGUI.BeginHandles(joint, "Change Angular Limits"))
				{
					newLimits = JointHandles.AngularLimit(
						joint as HingeJoint, bindpose, AngularLimitHandleSize
					);
				}
				if (SceneGUI.EndHandles())
				{
					(joint as HingeJoint).limits = newLimits;
					result = true;
				}
			}
			Handles.matrix = oldMatrix;
			return result;
		}
		
		/// <summary>
		/// Displays the axis handle property editor.
		/// </summary>
		public static void DisplayAxisHandlePropertyEditor()
		{
			EditorGUI.BeginChangeCheck();
			{
				s_AxisHandlePreference.CurrentValue =
					EditorGUIX.DisplayOnOffToggle("Axis Handle", s_AxisHandlePreference.CurrentValue);
			}
			if (EditorGUI.EndChangeCheck())
			{
				SceneView.RepaintAll();
			}
		}

		/// <summary>
		/// Displays the axis handles.
		/// </summary>
		/// <param name="joint">Joint.</param>
		/// <param name="bindpose">Bindpose.</param>
		/// <returns><see langword="true"/> if the handles changed; otherwise, <see langword="false"/>.</returns>
		public static bool DisplayAxisHandles(Joint joint, Matrix4x4 bindpose)
		{
			bool isChar = joint is CharacterJoint;
			bool isConf = joint is ConfigurableJoint;
			bool isHinge = joint is HingeJoint;
			if (!isChar && !isConf && !isHinge)
			{
				return false;
			}
			bool result = false;
			Matrix4x4 oldMatrix = Handles.matrix;
			Handles.matrix = (
				joint.connectedBody == null ? Matrix4x4.identity : joint.connectedBody.transform.localToWorldMatrix
			) * bindpose;
			Vector3 ax1 = Vector3.forward;
			Vector3 ax2 = Vector3.up;
			if (isChar)
			{
				ax1 = (joint as CharacterJoint).axis;
				ax2 = (joint as CharacterJoint).swingAxis;
			}
			else if (isConf)
			{
				ax1 = (joint as ConfigurableJoint).axis;
				ax2 = (joint as ConfigurableJoint).secondaryAxis;
			}
			else if (isHinge)
			{
				ax1 = (joint as HingeJoint).axis;
			}
			Quaternion axisOrientation = Quaternion.LookRotation(ax1, ax2) * s_BindposeAxisHandleOffset;
			if (SceneGUI.BeginHandles(joint, "Change Axes"))
			{
				axisOrientation = TransformHandles.Rotation(axisOrientation, Vector3.zero);
			}
			if (SceneGUI.EndHandles())
			{
				if (isChar)
				{
					(joint as CharacterJoint).axis = axisOrientation * s_PrimaryAxisHandleDirection;
					(joint as CharacterJoint).swingAxis = axisOrientation * s_SecondaryAxisHandleDirection;
				}
				else if (isConf)
				{
					(joint as ConfigurableJoint).axis = axisOrientation * s_PrimaryAxisHandleDirection;
					(joint as ConfigurableJoint).secondaryAxis = axisOrientation * s_SecondaryAxisHandleDirection;
				}
				else if (isHinge)
				{
					(joint as HingeJoint).axis = axisOrientation * s_PrimaryAxisHandleDirection;
				}
				result = true;
			}
			Handles.matrix = oldMatrix;
			return result;
		}

		/// <summary>
		/// Displays the handle preferences. They will be displayed in the preference menu and the top of the inspector.
		/// </summary>
		new protected static void DisplayHandlePreferences()
		{
			EditorGUI.BeginChangeCheck();
			{
				s_OverrideBuiltinEditors.CurrentValue = EditorGUIX.DisplayOnOffToggle(
					"Override Builtin Joint Editors", s_OverrideBuiltinEditors.CurrentValue
				);
			}
			if (EditorGUI.EndChangeCheck())
			{
				UnityFeatureDefineSymbols.SetSymbolForAllBuildTargets(
					s_OverrideBuiltinEditorSymbol, g => s_OverrideBuiltinEditors.CurrentValue
				);
			}
			DisplaySceneGUIHandlePreferences();
		}

		/// <summary>
		/// Displays the joint frame calculation notification message.
		/// </summary>
		public static void DisplayJointFrameCalculationNotification()
		{
			SceneGUI.DisplayNotification(
				"One or more selected joints did not exist before play mode was entered. Handle orientation cannot be determined.",
				s_JointCalculationMessageWrapValues
			);
		}
		
		/// <summary>
		/// Displays the linear limit handle property editor.
		/// </summary>
		public static void DisplayLinearLimitHandlePropertyEditor()
		{
			EditorGUI.BeginChangeCheck();
			{
				s_LinearLimitHandlePreference.CurrentValue =
					EditorGUIX.DisplayOnOffToggle("Linear Limit Handle", s_LinearLimitHandlePreference.CurrentValue);
			}
			if (EditorGUI.EndChangeCheck())
			{
				SceneView.RepaintAll();
			}
		}

		/// <summary>
		/// Displays the linear limit handles.
		/// </summary>
		/// <param name="joint">Joint.</param>
		/// <param name="bindpose">Bindpose.</param>
		/// <returns><see langword="true"/> if the handles changed; otherwise, <see langword="false"/>.</returns>
		public static bool DisplayLinearLimitHandles(Joint joint, Matrix4x4 bindpose)
		{
			if (joint is ConfigurableJoint)
			{
				ConfigurableJoint thisJoint = joint as ConfigurableJoint;
				float newLinearLimit = 0f;
				if (SceneGUI.BeginHandles(thisJoint, "Change Linear Limits"))
				{
					newLinearLimit = JointHandles.LinearLimit(thisJoint, bindpose);
				}
				if (SceneGUI.EndHandles())
				{
					SoftJointLimit limit = thisJoint.linearLimit;
					limit.limit = newLinearLimit;
					thisJoint.linearLimit = limit;
					return true;
				}
			}
			else if (joint is SpringJoint)
			{
				SpringJoint thisJoint = joint as SpringJoint;
				Vector2 newLinearLimits = new Vector2(thisJoint.minDistance, thisJoint.maxDistance);
				if (SceneGUI.BeginHandles(thisJoint, "Change Linear Limits"))
				{
					newLinearLimits = JointHandles.SpringLimit(thisJoint, bindpose);
				}
				if (SceneGUI.EndHandles())
				{
					thisJoint.minDistance = newLinearLimits.x;
					thisJoint.maxDistance = newLinearLimits.y;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Displays the handle preferences for scene GUI handles.
		/// </summary>
		protected static void DisplaySceneGUIHandlePreferences()
		{
			DisplayAngularLimitHandlePropertyEditor();
			DisplayLinearLimitHandlePropertyEditor();
			DisplayAnchorHandlePropertyEditor();
		}

		/// <summary>
		/// Gets the cached bindpose of the specified joint.
		/// </summary>
		/// <returns>The cached bindpose.</returns>
		/// <param name="joint">Joint.</param>
		public static Matrix4x4 GetCachedBindpose(Joint joint)
		{
			return s_Bindposes[joint];
		}
		
		/// <summary>
		/// Initializes the class. Override this method to perform any special functions when the class is loaded.
		/// </summary>
		new protected static void InitializeClass()
		{
			CacheAllBindposesInScene();
		}

		/// <summary>
		/// Determines if the bindpose for the specified joint is cached.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if bindpose is cached for the specified joint; otherwise, <see langword="false"/>.
		/// </returns>
		/// <param name="joint">Joint.</param>
		public static bool IsBindposeCached(Joint joint)
		{
			return s_Bindposes.ContainsKey(joint);
		}

		/// <summary>
		/// Updates the cached bindpose for the specified joint. This method should only be called if the application is
		/// not currently playing.
		/// </summary>
		/// <param name="joint">Joint.</param>
		public static void UpdateCachedBindpose(Joint joint)
		{
			s_Bindposes[joint] = joint.GetBindposeLocalMatrix();
		}

		/// <summary>
		/// An array of the inspected targets used to prevent errors related to accessing targets property from within
		/// OnSceneGUI().
		/// </summary>
		private Object[] m_CachedTargets = new Object[0];
		/// <summary>
		/// The type of the <see cref="UnityEngine.Joint"/> being inspected.
		/// </summary>
		private JointType m_JointType = JointType.Other;
		
		/// <summary>
		/// Gets the handle matrix.
		/// </summary>
		/// <value>The handle matrix.</value>
		protected override Matrix4x4 HandleMatrix { get { return Matrix4x4.identity; } }
		/// <summary>
		/// Gets a value indicating whether this <see cref="JointEditor"/> implements anchor properties.
		/// </summary>
		/// <value><see langword="true"/> if implements anchor properties; otherwise, <see langword="false"/>.</value>
		private bool ImplementsAnchor
		{
			get { return m_JointType != JointType.Other; }
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="JointEditor"/> implements angular limits.
		/// </summary>
		/// <value><see langword="true"/> if implements angular limits; otherwise, <see langword="false"/>.</value>
		private bool ImplementsAngularLimits
		{
			get
			{
				return m_JointType == JointType.Character ||
					m_JointType == JointType.Configurable ||
					m_JointType == JointType.Hinge;
			}
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="JointEditor"/> implements linear limits.
		/// </summary>
		/// <value><see langword="true"/> if implements linear limits; otherwise, <see langword="false"/>.</value>
		private bool ImplementsLinearLimits
		{
			get { return m_JointType == JointType.Configurable || m_JointType == JointType.Spring; }
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="JointEditor"/> implements scene GUI
		/// handles.
		/// </summary>
		/// <value><see langword="true"/> if implements scene GUI handles; otherwise, <see langword="false"/>.</value>
		protected override bool ImplementsSceneGUIHandles { get { return true; } }
		/// <summary>
		/// Gets a value indicating whether this <see cref="JointEditor"/> implements scene GUI
		/// overlay.
		/// </summary>
		/// <value><see langword="true"/> if implements scene GUI overlay; otherwise, <see langword="false"/>.</value>
		protected override bool ImplementsSceneGUIOverlay { get { return true; } }

		/// <summary>
		/// Displays a button for setting all axes of motion on an array of targets.
		/// </summary>
		/// <param name="preference">Preference storing the motion type value.</param>
		/// <param name="label">Label for the control.</param>
		/// <param name="targets">An array of configurable joints.</param>
		/// <param name="applyMethod">Method to invoke if preference value is to be applied to all targets.</param>
		private void DisplayMultiJointMotionButton(
			EditorPreference<ConfigurableJointMotion, JointEditor> preference,
			GUIContent label,
			System.Action<ConfigurableJoint, ConfigurableJointMotion> applyMethod
		)
		{
			Rect controlPosition, buttonPosition;
			EditorGUIX.GetRectsForControlWithInlineButton(
				EditorGUILayout.GetControlRect(), out controlPosition, out buttonPosition, 40f, 80f
			);
			preference.CurrentValue =
				(ConfigurableJointMotion)EditorGUIX.DisplayField<System.Enum>(
					controlPosition, label, preference.CurrentValue, EditorGUI.EnumPopup
				);
			if (EditorGUIX.DisplayButton(buttonPosition, "Set All"))
			{
				Undo.RecordObjects(m_CachedTargets, string.Format("Set All {0} Motion", label.text));
				foreach (ConfigurableJoint j in m_CachedTargets)
				{
					if (j == null)
					{
						continue;
					}
					applyMethod(j, preference.CurrentValue);
				}
				EditorUtilityX.SetDirty(m_CachedTargets);
			}
		}

		/// <summary>
		/// Displays the scene GUI controls. This group appears after handle toggles.
		/// </summary>
		protected override void DisplaySceneGUIControls()
		{
			base.DisplaySceneGUIControls();
			this.SerializedTarget.ApplyModifiedProperties();
			this.SerializedTarget.Update();
			switch (m_JointType)
			{
			case JointType.Configurable:
				DisplayMultiJointMotionButton(
					s_MultiTranslationMotionPreference, s_TranslationMotionLabel, JointX.SetAllTranslationMotion
				);
				++EditorGUI.indentLevel;
				DisplaySceneGUIPropertyField("m_XMotion");
				DisplaySceneGUIPropertyField("m_YMotion");
				DisplaySceneGUIPropertyField("m_ZMotion");
				--EditorGUI.indentLevel;
				DisplayMultiJointMotionButton(
					s_MultiRotationMotionPreference, s_RotationMotionLabel, JointX.SetAllAngularMotion
				);
				++EditorGUI.indentLevel;
				DisplaySceneGUIPropertyField("m_AngularXMotion");
				DisplaySceneGUIPropertyField("m_AngularYMotion");
				DisplaySceneGUIPropertyField("m_AngularZMotion");
				--EditorGUI.indentLevel;
				break;
			case JointType.Hinge:
				DisplaySceneGUIPropertyField("m_UseLimits");
				break;
			}
		}

		/// <summary>
		/// Displays the scene GUI handles.
		/// </summary>
		protected override void DisplaySceneGUIHandles()
		{
			base.DisplaySceneGUIHandles();
			// update bind pose orientation if application is not playing
			if (!EditorApplication.isPlaying)
			{
				UpdateCachedBindpose(this.Target);
			}
			else if (!IsBindposeCached(this.Target))
			{
				bool implementsAnyHandles =
					this.ImplementsAngularLimits || this.ImplementsLinearLimits || this.ImplementsAnchor;
				if (this.Target == this.FirstTarget && implementsAnyHandles)
				{
					DisplayJointFrameCalculationNotification();
				}
				return;
			}
			if (this.ImplementsAngularLimits)
			{
				if (AreAngularLimitHandlesEnabled)
				{
					DisplayAngularLimitHandles(this.Target, GetCachedBindpose(this.Target));
				}
				if (AreAxisHandlesEnabled)
				{
					DisplayAxisHandles(this.Target, GetCachedBindpose(this.Target));
				}
			}
			if (this.ImplementsLinearLimits && AreLinearLimitHandlesEnabled)
			{
				DisplayLinearLimitHandles(this.Target, GetCachedBindpose(this.Target));
			}
			if (this.ImplementsAnchor && AreAnchorHandlesEnabled)
			{
				DisplayAnchorHandles(this.Target, GetCachedBindpose(this.Target));
			}
		}

		/// <summary>
		/// Displays the scene GUI handle toggles. This group appears at the top of the scene GUI overlay.
		/// </summary>
		protected override void DisplaySceneGUIHandleToggles()
		{
			base.DisplaySceneGUIHandleToggles();
			if (this.ImplementsLinearLimits)
			{
				AreLinearLimitHandlesEnabled =
					EditorGUIX.DisplayOnOffToggle("Linear Limit Handle", AreLinearLimitHandlesEnabled);
			}
			if (this.ImplementsAngularLimits)
			{
				AreAngularLimitHandlesEnabled = EditorGUIX.DisplayOnOffToggle(
					"Angular Limit Handle", AreAngularLimitHandlesEnabled
				);
				AreAxisHandlesEnabled =
					EditorGUIX.DisplayOnOffToggle("Axis Handle", AreAxisHandlesEnabled);
			}
			if (this.ImplementsAnchor)
			{
				AreAnchorHandlesEnabled =
					EditorGUIX.DisplayOnOffToggle("Anchor Handle", AreAnchorHandlesEnabled);
			}
		}

		/// <summary>
		/// Raises the enable event.
		/// </summary>
		protected override void OnEnable()
		{
			base.OnEnable();
			if (this.target is CharacterJoint)
			{
				m_JointType = JointType.Character;
			}
			else if (this.target is ConfigurableJoint)
			{
				m_JointType = JointType.Configurable;
			}
			else if (this.target is HingeJoint)
			{
				m_JointType = JointType.Hinge;
			}
			else if (this.target is SpringJoint)
			{
				m_JointType = JointType.Spring;
			}
			else
			{
				m_JointType = JointType.Other;
			}
			if (!EditorApplication.isPlaying)
			{
				CacheAllBindposesInScene();
			}
			m_CachedTargets = GetCachedTargets();
		}

		/// <summary>
		/// Raises the inspector GUI event.
		/// </summary>
		public override void OnInspectorGUI()
		{
			if (this.target == null)
			{
				return;
			}
			if (m_JointType == JointType.Configurable)
			{
				if (EditorGUIX.BeginSceneGUIControlsArea())
				{
					DisplaySceneGUIHandlePreferences();
				}
				EditorGUIX.EndSceneGUIControlsArea();
				DisplayMultiJointMotionButton(
					s_MultiTranslationMotionPreference, s_TranslationMotionLabel, JointX.SetAllTranslationMotion
				);
				DisplayMultiJointMotionButton(
					s_MultiRotationMotionPreference, s_RotationMotionLabel, JointX.SetAllAngularMotion
				);
				EditorGUILayout.Space();
			}
			DisplayInspector();
		}
	}
}