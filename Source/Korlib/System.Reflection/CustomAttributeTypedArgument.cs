/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

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
	}
}