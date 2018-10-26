// 
// Vector3X.cs
// 
// Copyright (c) 2014-2015, Candlelight Interactive, LLC
// All rights reserved.
// 
// This file is licensed according to the terms of the Unity Asset Store EULA:
// http://download.unity3d.com/assetstore/customer-eula.pdf
// 
// This file contains a static class with extension methods for Vector3.

using UnityEngine;
using System.Collections.ObjectModel;

namespace Candlelight
{
	/// <summary>
	/// Extension methods for <see cref="UnityEngine.Vector3"/>.
	/// </summary>
	public static class Vector3X
	{
		/// <summary>
		/// The cardinal axes.
		/// </summary>
		private static readonly ReadOnlyCollection<Vector3> s_CardinalAxes = new ReadOnlyCollection<Vector3>(
			new [] { Vector3.right, Vector3.up, Vector3.forward, Vector3.left, Vector3.down, Vector3.back }
		);

		/// <summary>
		/// Gets the absolute value on each axis.
		/// </summary>
		/// <returns>
		/// A <see cref="UnityEngine.Vector3"/> whose values are the absolute value of the input
		/// <see cref="UnityEngine.Vector3"/> along each axis.
		/// </returns>
		/// <param name="v">A <see cref="UnityEngine.Vector3"/>.</param>
		public static Vector3 GetAbs(this Vector3 v)
		{
			return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
		}
		
		/// <summary>
		/// Gets the intersection with the specified plane.
		/// </summary>
		/// <returns><see langword="true"/> if there was an intersection; otherwise, <see langword="false"/>.</returns>
		/// <param name="origin">A point through which the line passes.</param>
		/// <param name="direction">The direction of the line.</param>
		/// <param name="p">The <see cref="UnityEngine.Plane"/> with which to intersect.</param>
		/// <param name="intersectionPoint">
		/// The intersection point, if the method returns <see langword="true"/>.
		/// </param>
		public static bool GetIntersectionOnPlane(
			ref Vector3 origin, ref Vector3 direction, ref Plane p, out Vector3 intersectionPoint
		)
		{
			Ray ray = new Ray(origin: origin, direction: direction);
			return GetIntersectionOnPlane(ref ray, ref p, out intersectionPoint);
		}
		
		/// <summary>
		/// Gets the intersection with the specified plane.
		/// </summary>
		/// <returns><see langword="true"/> if there was an intersection; otherwise, <see langword="false"/>.</returns>
		/// <param name="ray">The <see cref="UnityEngine.Ray"/> being tested.</param>
		/// <param name="p">The <see cref="UnityEngine.Plane"/> with which to intersect.</param>
		/// <param name="intersectionPoint">
		/// The intersection point, if the method returns <see langword="true"/>.
		/// </param>
		public static bool GetIntersectionOnPlane(ref Ray ray, ref Plane p, out Vector3 intersectionPoint)
		{
			float intersectionDistance;
			bool ret = p.Raycast(ray, out intersectionDistance);
			intersectionPoint = ray.GetPoint(intersectionDistance);
			return ret;
		}

		/// <summary>
		/// Gets the greatest of the values along the three axes.
		/// </summary>
		/// <returns>The greatest of the values along the three axes.</returns>
		/// <param name="v">A <see cref="UnityEngine.Vector3"/>.</param>
		public static float GetMaxValue(this Vector3 v)
		{
			return Mathf.Max(v.x, v.y, v.z);
		}

		/// <summary>
		/// Gets the smallest of the values along the three axes.
		/// </summary>
		/// <returns>The smallest of the values along three axes.</returns>
		/// <param name="v">A <see cref="UnityEngine.Vector3"/>.</param>
		public static float GetMinValue(this Vector3 v)
		{
			return Mathf.Min(v.x, v.y, v.z);
		}
		
		/// <summary>
		/// Gets the specified point reflected across a plane.
		/// </summary>
		/// <returns>The point reflected across a plane.</returns>
		/// <param name="p">Point to reflect.</param>
		/// <param name="n">Normal of the plane across which to reflect the point.</param>
		public static Vector3 GetPointReflectedAcrossPlane(this Vector3 p, Vector3 n)
		{
			n.Normalize();
			return p - 2f * (Vector3.Dot(p, n) * n);
		}
		
		/// <summary>
		/// Gets the nearest cardinal axis.
		/// </summary>
		/// <returns>The cardinal axis nearest to the specified test vector.</returns>
		/// <param name="testVector">Test vector.</param>
		public static Vector3 GetNearestCardinalAxis(this Vector3 testVector)
		{
			testVector.Normalize();
			Vector3 nearest = Vector3.forward;
			foreach (Vector3 v in s_CardinalAxes)
			{
				if (Vector3.Dot(testVector, v) > Vector3.Dot(testVector, nearest))
				{
					nearest = v;
				}
			}
			return nearest;
		}

		/// <summary>
		/// Gets the projection of the specified <see cref="UnityEngine.Vector3"/> on the specified
		/// <see cref="UnityEngine.Plane"/>.
		/// </summary>
		/// <returns>
		/// The projection of the specified <see cref="UnityEngine.Vector3"/> on the specified
		/// <see cref="UnityEngine.Plane"/>.
		/// </returns>
		/// <param name="v">A <see cref="UnityEngine.Vector3"/>.</param>
		/// <param name="p">A <see cref="UnityEngine.Plane"/>.</param>
		public static Vector3 GetProjectionOnPlane(this Vector3 v, ref Plane p)
		{
			return v - Vector3.Dot(v + p.normal * p.distance, p.normal) * p.normal;
		}

		/// <summary>
		/// Multiplies <paramref name="v1"/> by the inverse of <paramref name="v2"/>.
		/// </summary>
		/// <returns>The inverse times other.</returns>
		/// <param name="v1">The first <see cref="UnityEngine.Vector3"/>.</param>
		/// <param name="v2">The second <see cref="UnityEngine.Vector3"/>.</param>
		public static Vector3 MultiplyInverse(this Vector3 v1, Vector3 v2)
		{
			return new Vector3(v1.x + 1f / v2.x, v1.y + 1f / v2.y, v1.z + 1f / v2.z);
		}

		/// <summary>
		/// Normalizes the specified Euler angles to the range (-180, 180].
		/// </summary>
		/// <param name="eulerAngles">Euler angles.</param>
		public static void NormalizeAngles(ref Vector3 eulerAngles)
		{
			while (eulerAngles.x <= -180f)
			{
				eulerAngles.x += 360f;
			}
			while (eulerAngles.x > 180f)
			{
				eulerAngles.x -= 360f;
			}
			while (eulerAngles.y <= -180f)
			{
				eulerAngles.y += 360f;
			}
			while (eulerAngles.y > 180f)
			{
				eulerAngles.y -= 360f;
			}
			while (eulerAngles.z <= -180f)
			{
				eulerAngles.z += 360f;
			}
			while (eulerAngles.z > 180f)
			{
				eulerAngles.z -= 360f;
			}
		}
		
		/// <summary>
		/// Converts an <see cref="UnityEngine.Input"/> position to <see cref="UnityEngine.GUI"/> position.
		/// </summary>
		/// <returns>The <see cref="UnityEngine.GUI"/> position.</returns>
		/// <param name="inputPosition">The <see cref="UnityEngine.Input"/> position.</param>
		public static Vector3 ToGUIPosition(this Vector3 inputPosition)
		{
			inputPosition.y = Screen.height - inputPosition.y;
			return inputPosition;
		}

		/// <summary>
		/// Converts a <see cref="UnityEngine.GUI"/> position to a <see cref="UnityEngine.Input"/> position.
		/// </summary>
		/// <returns>The <see cref="UnityEngine.Input"/> position.</returns>
		/// <param name="guiPosition"><see cref="UnityEngine.GUI"/> position.</param>
		public static Vector3 ToMousePosition(this Vector3 guiPosition)
		{
			return ToGUIPosition(guiPosition);
		}
	}
}