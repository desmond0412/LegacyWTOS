// 
// Matrix4x4X.cs
// 
// Copyright (c) 2014-2015, Candlelight Interactive, LLC
// All rights reserved.
// 
// This file is licensed according to the terms of the Unity Asset Store EULA:
// http://download.unity3d.com/assetstore/customer-eula.pdf

using UnityEngine;

namespace Candlelight
{
	/// <summary>
	/// <see cref="UnityEngine.Matrix4x4"/> extension methods.
	/// </summary>
	public static class Matrix4x4X
	{
		/// <summary>
		/// Gets the position component of the matrix.
		/// </summary>
		/// <returns>The position component of the matrix.</returns>
		/// <param name="matrix">A <see cref="UnityEngine.Matrix4x4"/>.</param>
		public static Vector3 GetPosition(this Matrix4x4 matrix)
		{
			return matrix.MultiplyPoint3x4(Vector3.zero);
		}

		/// <summary>
		/// Gets the rotation component of the matrix.
		/// </summary>
		/// <returns>The rotation component of the matrix.</returns>
		/// <param name="matrix">A <see cref="UnityEngine.Matrix4x4"/>.</param>
		public static Quaternion GetRotation(this Matrix4x4 matrix)
		{
			return Quaternion.LookRotation(matrix.MultiplyVector(Vector3.forward), matrix.MultiplyVector(Vector3.up));
		}

		/// <summary>
		/// Gets the scale component of the matrix.
		/// </summary>
		/// <returns>The scale component of the matrix.</returns>
		/// <param name="matrix">A <see cref="UnityEngine.Matrix4x4"/>.</param>
		public static Vector3 GetScale(this Matrix4x4 matrix)
		{
			return new Vector3(
				new Vector3(matrix.m00, matrix.m10, matrix.m20).magnitude,
				new Vector3(matrix.m01, matrix.m11, matrix.m21).magnitude,
				new Vector3(matrix.m02, matrix.m12, matrix.m22).magnitude
			);
		}
	}
}