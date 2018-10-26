// 
// LinearHandles.cs
// 
// Copyright (c) 2011-2015, Candlelight Interactive, LLC
// All rights reserved.
// 
// This file is licensed according to the terms of the Unity Asset Store EULA:
// http://download.unity3d.com/assetstore/customer-eula.pdf
// 
// This file contains a static class with functions for drawing linear handles.

using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Reflection;

namespace Candlelight
{
	/// <summary>
	/// Linear handles.
	/// </summary>
	[InitializeOnLoad]
	public static class LinearHandles
	{
		/// <summary>
		/// Initializes the <see cref="LinearHandles"/> class.
		/// </summary>
		static LinearHandles()
		{
			if (s_Do1DSlider == null)
			{
				Debug.LogError("Slider1D.Do() method could not be found.");
			}
		}

		/// <summary>
		/// Display mode.
		/// </summary>
		internal enum DisplayMode { Dot, Line, Arrow, Cone };
		/// <summary>
		/// The method for doing a 1D slider. It is required because Handles.Slider() has no API point that allows
		/// specification of a control ID.
		/// </summary>
		private static readonly MethodInfo s_Do1DSlider =
			typeof(Handles).Assembly.GetTypes().FirstOrDefault(t => t.Name == "Slider1D").GetMethod(
				"Do",
				ReflectionX.staticBindingFlags,
				null,
				new System.Type[]
				{
					typeof(int),
					typeof(Vector3),
					typeof(Vector3),
					typeof(float),
					typeof(Handles.DrawCapFunction),
					typeof(float)
				},
				null
			);
		/// <summary>
		/// A reusable parameter list when invoking the Slider1Do.Do() method.
		/// </summary>
		private static readonly object[] s_Do1DSliderParams = new object[6];
		/// <summary>
		/// Required minimum handle change.
		/// </summary>
		/// <remarks>
		/// This value represents the smallest change in a handle's 3D position that will trigger a value change. If
		/// this number is 0, then many linear handles must transform along a cardinal axis or risk continually
		/// changing a prefab value.
		/// </remarks>
		private static readonly float s_RequiredMinHandleChange = 0.00001f;

		/// <summary>
		/// Displays an arrow slider.
		/// </summary>
		/// <returns>The new value.</returns>
		/// <param name="id">Control identifier.</param>
		/// <param name="val">Value.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="label">Label.</param>
		/// <param name="capScale">A scale factor for the slider cap.</param>
		public static float Arrow(
			int id, float val, Vector3 origin, Vector3 direction, string label = "", float capScale = 1f
		)
		{
			return Slider(id, val, origin, direction, label, capScale, DisplayMode.Arrow, null);
		}

		/// <summary>
		/// Displays a cone slider.
		/// </summary>
		/// <returns>The new value.</returns>
		/// <param name="id">Control identifier.</param>
		/// <param name="val">Value.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="label">Label.</param>
		/// <param name="capScale">A scale factor for the slider cap.</param>
		public static float Cone(
			int id, float val, Vector3 origin, Vector3 direction, string label = "", float capScale = 1f
		)
		{
			return Slider(id, val, origin, direction, label, capScale, DisplayMode.Cone, null);
		}
		
		/// <summary>
		/// Displays a dot slider.
		/// </summary>
		/// <returns>The new value.</returns>
		/// <param name="id">Control identifier.</param>
		/// <param name="val">Value.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="label">Label.</param>
		/// <param name="color">Color.</param>
		/// <param name="capScale">A scale factor for the slider cap.</param>
		public static float Dot(
			int id, float val, Vector3 origin, Vector3 direction, string label = "", float capScale = 1f
		)
		{
			return Slider(id, val, origin, direction, label, capScale, DisplayMode.Dot, null);
		}
		
		/// <summary>
		/// Displays a line slider.
		/// </summary>
		/// <returns>The new value.</returns>
		/// <param name="id">Control identifier</param>
		/// <param name="val">Value.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="label">Label.</param>
		/// <param name="color">Color.</param>
		/// <param name="capScale">A scale factor for the slider cap.</param>
		/// <param name="capFunction">Cap function to use. If <see langword="null"/> then defaults to dot.</param>
		public static float Line(
			int id, float val, Vector3 origin, Vector3 direction,
			string label = "", float capScale = 1f, Handles.DrawCapFunction capFunction = null
		)
		{
			return Slider(id, val, origin, direction, label, capScale, DisplayMode.Line, capFunction);
		}
		
		/// <summary>
		/// Displays a slider.
		/// </summary>
		/// <returns>The new value.</returns>
		/// <param name="id">Control identifier.</param>
		/// <param name="val">Value.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="label">Label.</param>
		/// <param name="capScale">A scale factor for the slider cap.</param>
		/// <param name="displayMode">Display mode.</param>
		/// <param name="capFunction">Default cap function (used for line handles).</param>
		private static float Slider(
			int id,
			float val,
			Vector3 origin,
			Vector3 direction,
			string label,
			float capScale,
			DisplayMode displayMode,
			Handles.DrawCapFunction capFunction
		)
		{
			// set handle matrix
			Matrix4x4 oldMatrix = Handles.matrix;
			Handles.matrix *= Matrix4x4.TRS(origin, Quaternion.identity, Vector3.one);
			// normalize direction
			direction.Normalize();
			// draw a label if requested
			if (!string.IsNullOrEmpty(label))
			{
				Handles.Label(direction * val, label);
			}
			// perform special setup based on display mode
			Handles.DrawCapFunction cap = capFunction == null ? Handles.DotCap : capFunction;
			switch (displayMode)
			{
			case DisplayMode.Arrow:
				cap = Handles.ArrowCap;
				break;
			case DisplayMode.Cone:
				cap = Handles.ConeCap;
				break;
			case DisplayMode.Dot:
				cap = Handles.DotCap;
				capScale *= SceneGUI.DotHandleSize;
				break;
			case DisplayMode.Line:
				Handles.DrawLine(Vector3.zero, direction * val);
				capScale *= SceneGUI.DotHandleSize;
				break;
			}
			// create a slider
			Vector3 initialPosition = direction * val;
			s_Do1DSliderParams[0] = id;
			s_Do1DSliderParams[1] = initialPosition;
			s_Do1DSliderParams[2] = direction;
			s_Do1DSliderParams[3] = SceneGUI.GetFixedHandleSize(initialPosition, capScale);
			s_Do1DSliderParams[4] = cap;
			s_Do1DSliderParams[5] = 1f; // TODO: do something with snap
			Vector3 vDelta = (Vector3)s_Do1DSlider.Invoke(null, s_Do1DSliderParams) - initialPosition;
			float delta = vDelta.magnitude;
			if (delta > s_RequiredMinHandleChange)
			{
				val += delta * Mathf.Sign(Vector3.Dot(vDelta, direction));
			}
			// reset handle matrix
			Handles.matrix = oldMatrix;
			// return results
			return val;
		}

		#region Obsolete
		[System.Obsolete]
		public const string noLabel = "";
		[System.Obsolete("Use Candlelight.LinearHandles.Arrow(int id, float val, Vector3 origin, Vector3 direction, string label, float capScale)")]
		public static float Arrow(
			float val, Vector3 origin, Vector3 direction, string label = "", float capScale = 1f, int id = 0
		)
		{
			return Arrow(id, val, origin, direction, label, capScale);
		}
		[System.Obsolete("Use Candlelight.LinearHandles.Cone(int id, float val, Vector3 origin, Vector3 direction, string label, float capScale)")]
		public static float Cone(
			float val, Vector3 origin, Vector3 direction, string label = "", float capScale = 1f, int id = 0
		)
		{
			return Cone(id, val, origin, direction, label, capScale);
		}
		[System.Obsolete("Use Candlelight.LinearHandles.Dot(int id, float val, Vector3 origin, Vector3 direction, string label, float capScale)")]
		public static float Dot(
			float val, Vector3 origin, Vector3 direction, string label = "", float capScale = 1f, int id = 0
		)
		{
			return Dot(id, val, origin, direction, label, capScale);
		}
		[System.Obsolete("Use Candlelight.LinearHandles.Line(int id, float val, Vector3 origin, Vector3 direction, string label, float capScale)")]
		public static float Line(
			float val, Vector3 origin, Vector3 direction, string label = "", float capScale = 1f, int id = 0
		)
		{
			return Line(id, val, origin, direction, label, capScale);
		}
		#endregion
	}
}