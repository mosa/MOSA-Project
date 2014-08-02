﻿/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Reflection
{
	[Serializable]
	public abstract class ConstructorInfo : MethodBase
	{
		/// <summary>
		/// Represents the name of the class constructor method as it is stored in metadata. This name is always ".ctor". This field is read-only.
		/// </summary>
		public static readonly string ConstructorName = ".ctor";

		/// <summary>
		/// Represents the name of the type constructor method as it is stored in metadata. This name is always ".cctor". This field is read-only.
		/// </summary>
		public static readonly string TypeConstructorName = ".cctor";

		/// <summary>
		/// Gets a <see cref="System.Reflection.MemberTypes">MemberTypes</see> value indicating that this member is a constructor.
		/// </summary>
		public override MemberTypes MemberType
		{
			get { return MemberTypes.Constructor; }
		}

		/// <summary>
		/// Invokes the constructor reflected by the instance that has the specified parameters, providing default values for the parameters not commonly used.
		/// </summary>
		/// <param name="parameters">An array of values that matches the number, order and type (under the constraints of the default binder) of the parameters for this constructor. If this constructor takes no parameters, then use either an array with zero elements or null, as in Object[] parameters = new Object[0]. Any object in this array that is not explicitly initialized with a value will contain the default value for that object type. For reference-type elements, this value is null. For value-type elements, this value is 0, 0.0, or false, depending on the specific element type.</param>
		/// <returns>An instance of the class associated with the constructor.</returns>
		public object Invoke(object[] parameters)
		{
			// TODO
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns a value that indicates whether this instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">An object to compare with this instance, or null.</param>
		/// <returns>True if obj equals the type and value of this instance; otherwise, False.</returns>
		public override bool Equals(object obj)
		{
			// TODO
			return base.Equals(obj);
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			// TODO
			return base.GetHashCode();
		}

		/// <summary>
		/// Indicates whether two ConstructorInfo objects are equal.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>True if left is equal to right; otherwise, False.</returns>
		public static bool operator ==(ConstructorInfo left, ConstructorInfo right)
		{
			if (object.ReferenceEquals(left, right))
				return true;

			if ((object)left == null || (object)right == null)
				return false;

			return left.Equals(right);
		}

		/// <summary>
		/// Indicates whether two ConstructorInfo objects are not equal.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>True if left is not equal to right; otherwise, False.</returns>
		public static bool operator !=(ConstructorInfo left, ConstructorInfo right)
		{
			return !(left == right);
		}
	}
}