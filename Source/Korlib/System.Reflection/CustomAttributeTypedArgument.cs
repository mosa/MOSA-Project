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
	/// Represents an argument of a custom attribute in the reflection-only context, or an element of an array argument.
	/// </summary>
	[Serializable]
	public struct CustomAttributeTypedArgument
	{
		private Type argumentType;
		private object value;

		/// <summary>
		/// Initializes a new instance of the CustomAttributeTypedArgument class with the specified value.
		/// </summary>
		/// <param name="value">The value of the custom attribute argument.</param>
		public CustomAttributeTypedArgument(object value)
		{
			if (value == null)
				throw new ArgumentNullException("value");

			this.argumentType = value.GetType();
			this.value = value;
		}

		/// <summary>
		/// Initializes a new instance of the CustomAttributeTypedArgument class with the specified type and value.
		/// </summary>
		/// <param name="argumentType">The type of the custom attribute argument.</param>
		/// <param name="value">The value of the custom attribute argument.</param>
		public CustomAttributeTypedArgument(Type argumentType, object value)
		{
			if (argumentType == null)
				throw new ArgumentNullException("argumentType");

			this.argumentType = argumentType;
			this.value = value;
		}

		/// <summary>
		/// Gets the type of the argument or of the array argument element.
		/// </summary>
		public Type ArgumentType
		{
			get { return this.argumentType; }
		}

		/// <summary>
		/// Gets the value of the argument for a simple argument or for an element of an array argument; gets a collection of values for an array argument.
		/// </summary>
		public object Value
		{
			get { return this.value; }
		}

		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <param name="obj">The object to compare with the current instance.</param>
		/// <returns>True if obj and this instance are the same type and represent the same value; otherwise, False.</returns>
		public override bool Equals(object obj)
		{
			if (!(obj is CustomAttributeTypedArgument))
				return false;

			CustomAttributeTypedArgument other = (CustomAttributeTypedArgument)obj;
			return other.argumentType == argumentType
				&& (value != null ? value.Equals(other.value) : (object)other.value == null);
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
		public override int GetHashCode()
		{
			// TODO
			return base.GetHashCode();
		}

		/// <summary>
		/// Returns a string consisting of the equal sign, and a string representation of the argument value.
		/// </summary>
		/// <returns>A string consisting of the equal sign, and a string representation of the argument value.</returns>
		public override string ToString()
		{
			string val = value != null ? value.ToString() : String.Empty;
			if (argumentType == typeof(string))
				val = "\"" + val + "\"";
			if (argumentType == typeof(Type))
				val = "typeof (" + val + ")";
			if (argumentType.IsEnum)
				val = "(" + argumentType.Name + ")" + val;

			return " = " + val;
		}
	}
}