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
	/// <summary>
	/// Represents a named argument of a custom attribute in the reflection-only context.
	/// </summary>
	[Serializable]
	public struct CustomAttributeNamedArgument
	{
		private CustomAttributeTypedArgument typedArgument;
		private MemberInfo memberInfo;

		/// <summary>
		/// Initializes a new instance of the CustomAttributeNamedArgument class, which represents the specified field or property of the custom attribute, and specifies the value of the field or property.
		/// </summary>
		/// <param name="memberInfo">A field or property of the custom attribute. The new CustomAttributeNamedArgument object represents this member and its value.</param>
		/// <param name="value">The value of the field or property of the custom attribute.</param>
		public CustomAttributeNamedArgument(MemberInfo memberInfo, object value)
		{
			if (memberInfo == null)
				throw new ArgumentNullException("memberInfo");

			if (!(memberInfo.MemberType == MemberTypes.Field || memberInfo.MemberType == MemberTypes.Property))
				throw new ArgumentException("memberInfo is not a field or property of the custom attribute.");

			this.memberInfo = memberInfo;
			this.typedArgument = new CustomAttributeTypedArgument(value);
		}

		/// <summary>
		/// Initializes a new instance of the CustomAttributeNamedArgument class, which represents the specified field or property of the custom attribute, and specifies a CustomAttributeTypedArgument object that describes the type and value of the field or property.
		/// </summary>
		/// <param name="memberInfo">A field or property of the custom attribute. The new CustomAttributeNamedArgument object represents this member and its value.</param>
		/// <param name="typedArgument">An object that describes the type and value of the field or property.</param>
		public CustomAttributeNamedArgument(MemberInfo memberInfo, CustomAttributeTypedArgument typedArgument)
		{
			if (memberInfo == null)
				throw new ArgumentNullException("memberInfo");

			this.memberInfo = memberInfo;
			this.typedArgument = typedArgument;
		}

		/// <summary>
		/// Gets a value that indicates whether the named argument is a field.
		/// </summary>
		public bool IsField
		{
			get { return memberInfo.MemberType == MemberTypes.Field; }
		}

		/// <summary>
		/// Gets the attribute member that would be used to set the named argumen
		/// </summary>
		public MemberInfo MemberInfo
		{
			get { return this.memberInfo; }
		}

		/// <summary>
		/// Gets the name of the attribute member that would be used to set the named argument.
		/// </summary>
		public string MemberName
		{
			get { return memberInfo.Name; }
		}

		/// <summary>
		/// Gets a CustomAttributeTypedArgument structure that can be used to obtain the type and value of the current named argument.
		/// </summary>
		public CustomAttributeTypedArgument TypedValue
		{
			get { return this.typedArgument; }
		}

		/// <summary>
		/// Returns a value that indicates whether this instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">An object to compare with this instance, or null.</param>
		/// <returns>True if obj equals the type and value of this instance; otherwise, False.</returns>
		public override bool Equals(object obj)
		{
			if (!(obj is CustomAttributeNamedArgument))
				return false;

			CustomAttributeNamedArgument other = (CustomAttributeNamedArgument)obj;
			return other.memberInfo == memberInfo 
				&& typedArgument.Equals(other.typedArgument);
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
		/// Returns a string that consists of the argument name, the equal sign, and a string representation of the argument value.
		/// </summary>
		/// <returns>A string that consists of the argument name, the equal sign, and a string representation of the argument value.</returns>
		public override string ToString()
		{
			return memberInfo.Name + typedArgument.ToString();
		}

		/// <summary>
		/// Tests whether two CustomAttributeNamedArgument structures are equivalent.
		/// </summary>
		/// <param name="left">The structure to the left of the equality operator.</param>
		/// <param name="right">The structure to the right of the equality operator.</param>
		/// <returns>True if the two CustomAttributeNamedArgument structures are equal; otherwise, False.</returns>
		public static bool operator ==(CustomAttributeNamedArgument left, CustomAttributeNamedArgument right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Tests whether two CustomAttributeNamedArgument structures are different.
		/// </summary>
		/// <param name="left">The structure to the left of the inequality operator.</param>
		/// <param name="right">The structure to the right of the inequality operator.</param>
		/// <returns>True if the two CustomAttributeNamedArgument structures are different; otherwise, False.</returns>
		public static bool operator !=(CustomAttributeNamedArgument left, CustomAttributeNamedArgument right)
		{
			return !left.Equals(right);
		}
	}
}