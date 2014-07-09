/*
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
	public abstract class MethodInfo : MethodBase
	{
		/// <summary>
		/// Gets a <see cref="System.Reflection.MemberTypes">MemberTypes</see> value indicating that this member is a method.
		/// </summary>
		public override MemberTypes MemberType
		{
			get { return MemberTypes.Method; }
		}

		/// <summary>
		/// Gets a ParameterInfo object that contains information about the return type of the method, such as whether the return type has custom modifiers.
		/// </summary>
		public virtual ParameterInfo ReturnParameter
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets the return type of this method.
		/// </summary>
		public virtual Type ReturnType
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets the custom attributes for the return type.
		/// </summary>
		public abstract ICustomAttributeProvider ReturnTypeCustomAttributes { get; }

		/// <summary>
		/// Initializes a new instance of the MethodInfo class.
		/// </summary>
		protected MethodInfo()
		{
		}

		/// <summary>
		/// Creates a delegate of the specified type from this method.
		/// </summary>
		/// <param name="delegateType">The type of the delegate to create.</param>
		/// <returns>The delegate for this method.</returns>
		public virtual Delegate CreateDelegate(Type delegateType)
		{
			// TODO
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a delegate of the specified type with the specified target from this method.
		/// </summary>
		/// <param name="delegateType">The type of the delegate to create.</param>
		/// <param name="target">The object targeted by the delegate.</param>
		/// <returns>The delegate for this method.</returns>
		public virtual Delegate CreateDelegate(Type delegateType, object target)
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
		/// When overridden in a derived class, returns the MethodInfo object for the method on the direct or indirect base class in which the method represented by this instance was first declared.
		/// </summary>
		/// <returns>A MethodInfo object for the first implementation of this method.</returns>
		public abstract MethodInfo GetBaseDefinition();

		/// <summary>
		/// Returns an array of Type objects that represent the type arguments of a generic method or the type parameters of a generic method definition.
		/// </summary>
		/// <returns>An array of Type objects that represent the type arguments of a generic method or the type parameters of a generic method definition. Returns an empty array if the current method is not a generic method.</returns>
		public override Type[] GetGenericArguments()
		{
			// TODO
			throw new NotSupportedException();
		}

		/// <summary>
		/// Returns a MethodInfo object that represents a generic method definition from which the current method can be constructed.
		/// </summary>
		/// <returns>A MethodInfo object representing a generic method definition from which the current method can be constructed.</returns>
		public virtual MethodInfo GetGenericMethodDefinition()
		{
			// TODO
			throw new NotSupportedException();
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
		/// Substitutes the elements of an array of types for the type parameters of the current generic method definition, and returns a MethodInfo object representing the resulting constructed method.
		/// </summary>
		/// <param name="typeArguments">An array of types to be substituted for the type parameters of the current generic method definition.</param>
		/// <returns>A MethodInfo object that represents the constructed method formed by substituting the elements of typeArguments for the type parameters of the current generic method definition.</returns>
		public virtual MethodInfo MakeGenericMethod(params Type[] typeArguments)
		{
			// TODO
			throw new NotSupportedException();
		}

		/// <summary>
		/// Indicates whether two MethodInfo objects are equal.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>True if left is equal to right; otherwise, False.</returns>
		public static bool operator ==(MethodInfo left, MethodInfo right)
		{
			if (object.ReferenceEquals(left, right))
				return true;

			if ((object)left == null || (object)right == null)
				return false;

			return left.Equals(right);
		}

		/// <summary>
		/// Indicates whether two MethodInfo objects are not equal.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>True if left is not equal to right; otherwise, False.</returns>
		public static bool operator !=(MethodInfo left, MethodInfo right)
		{
			return !(left == right);
		}
	}
}