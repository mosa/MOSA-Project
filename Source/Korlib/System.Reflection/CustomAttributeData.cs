/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System.Collections.Generic;

namespace System.Reflection
{
	/// <summary>
	/// Provides access to custom attribute data for assemblies, modules, types, members and parameters that are loaded into the reflection-only context.
	/// </summary>
	[Serializable]
	public class CustomAttributeData
	{
		private Type attributeType;
		private IList<CustomAttributeTypedArgument> ctorArgs;
		private IList<CustomAttributeNamedArgument> namedArgs;

		/// <summary>
		/// The type of the attribute.
		/// </summary>
		public Type AttributeType
		{
			get { return this.attributeType; }
		}

		/// <summary>
		/// A collection of structures that represent the positional arguments specified for the custom attribute instance.
		/// </summary>
		public virtual IList<CustomAttributeTypedArgument> ConstructorArguments
		{
			get { return this.ctorArgs; }
		}

		/// <summary>
		/// A collection of structures that represent the named arguments specified for the custom attribute instance.
		/// </summary>
		public virtual IList<CustomAttributeNamedArgument> NamedArguments
		{
			get { return this.namedArgs; }
		}
	}
}