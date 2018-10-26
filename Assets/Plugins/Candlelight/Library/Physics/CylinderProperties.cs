// 
// CylinderProperties.cs
// 
// Copyright (c) 2013-2015, Candlelight Interactive, LLC
// All rights reserved.
// 
// This file is licensed according to the terms of the Unity Asset Store EULA:
// http://download.unity3d.com/assetstore/customer-eula.pdf

namespace Candlelight
{
	/// <summary>
	/// A struct for describing cylinder properties.
	/// </summary>
	public struct CylinderProperties : System.IEquatable<CylinderProperties>
	{
		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <value>The height.</value>
		public float Height { get; private set; }
		/// <summary>
		/// Gets the radius.
		/// </summary>
		/// <value>The radius.</value>
		public float Radius { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="CylinderProperties"/> struct.
		/// </summary>
		/// <param name="height">Height.</param>
		/// <param name="radius">Radius.</param>
		public CylinderProperties(float height, float radius) : this()
		{
			this.Height = height;
			this.Radius = radius;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="CylinderProperties"/>.
		/// </summary>
		/// <param name="obj">
		/// The <see cref="System.Object"/> to compare with the current <see cref="CylinderProperties"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="CylinderProperties"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public override bool Equals(object obj)
		{
			return ObjectX.Equals(ref this, obj);
		}

		/// <summary>
		/// Determines whether the specified <see cref="CylinderProperties"/> is equal to the current
		///  <see cref="CylinderProperties"/>.
		/// </summary>
		/// <param name="other">
		/// The <see cref="CylinderProperties"/> to compare with the current <see cref="CylinderProperties"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the specified <see cref="CylinderProperties"/> is equal to the current
		/// <see cref="CylinderProperties"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public bool Equals(CylinderProperties other)
		{
			return GetHashCode() == other.GetHashCode();
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="CylinderProperties"/> object.
		/// </summary>
		/// <returns>
		/// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
		/// hash table.
		/// </returns>
		public override int GetHashCode()
		{
			return ObjectX.GenerateHashCode(this.Height.GetHashCode(), this.Radius.GetHashCode());
		}
	}
}