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
	/// Represents a named argument of a custom attribute in the reflection-only context.
	/// </summary>
	[Serializable]
	public struct CustomAttributeNamedArgument
	{
		private string memberName;
		private CustomAttributeTypedArgument typedArgument;
		private bool isField;

		/// <summary>
		/// Gets a value that indicates whether the named argument is a field.
		/// </summary>
		public bool IsField
		{
			get { return this.isField; }
		}

		/// <summary>
		/// Gets the name of the attribute member that would be used to set the named argument.
		/// </summary>
		public string MemberName
		{
			get { return this.memberName; }
		}

		/// <summary>
		/// Gets a CustomAttributeTypedArgument structure that can be used to obtain the type and value of the current named argument.
		/// </summary>
		public CustomAttributeTypedArgument TypedValue
		{
			get { return this.typedArgument; }
		}
	}
}