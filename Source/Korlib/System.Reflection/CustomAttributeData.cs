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
	/// Provides access to custom attribute data for assemblies, modules, types, members and parameters that are loaded into the reflection-only context.
	/// </summary>
	[Serializable]
	public class CustomAttributeData
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CustomAttributeData">CustomAttributeData</see> class.
		/// </summary>
		protected CustomAttributeData() { }

		/// <summary>
		/// The type of the attribute.
		/// </summary>
		public Type AttributeType { get; }

		/// <summary>
		/// An object that represents the constructor that would have initialized the custom attribute represented by the current instance of the CustomAttributeData class.
		/// </summary>
		public virtual ConstructorInfo Constructor { get; }

		/// <summary>
		/// A collection of structures that represent the positional arguments specified for the custom attribute instance.
		/// </summary>
		public virtual IList<CustomAttributeTypedArgument> ConstructorArguments { get; }

		/// <summary>
		/// A collection of structures that represent the named arguments specified for the custom attribute instance.
		/// </summary>
		public virtual IList<CustomAttributeNamedArgument> NamedArguments { get; }

		/// <summary>
		/// Returns a list of <see cref="CustomAttributeData">CustomAttributeData</see> objects representing data about the attributes that have been applied to the target assembly.
		/// </summary>
		/// <param name="target">The assembly whose custom attribute data is to be retrieved.</param>
		/// <returns>A list of objects that represent data about the attributes that have been applied to the target assembly.</returns>
		public static IList<CustomAttributeData> GetCustomAttributes(Assembly target)
		{
			if (target == null)
				throw new ArgumentNullException("target");
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns a list of <see cref="CustomAttributeData">CustomAttributeData</see> objects representing data about the attributes that have been applied to the target member.
		/// </summary>
		/// <param name="target">The member whose attribute data is to be retrieved.</param>
		/// <returns>A list of objects that represent data about the attributes that have been applied to the target member.</returns>
		public static IList<CustomAttributeData> GetCustomAttributes(MemberInfo target)
		{
			if (target == null)
				throw new ArgumentNullException("target");
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns a list of <see cref="CustomAttributeData">CustomAttributeData</see> objects representing data about the attributes that have been applied to the target parameter.
		/// </summary>
		/// <param name="target">The parameter whose attribute data is to be retrieved.</param>
		/// <returns>A list of objects that represent data about the attributes that have been applied to the target parameter.</returns>
		public static IList<CustomAttributeData> GetCustomAttributes(ParameterInfo target)
		{
			if (target == null)
				throw new ArgumentNullException("target");
			throw new NotImplementedException();
		}
	}
}