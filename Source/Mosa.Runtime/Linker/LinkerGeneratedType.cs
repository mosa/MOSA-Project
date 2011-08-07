/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.TypeSystem;

namespace Mosa.Runtime.Linker
{
	/// <summary>
	/// Represents a compiler generated type.
	/// </summary>
	public sealed class LinkerGeneratedType : RuntimeType
	{
		private readonly List<RuntimeMethod> methods;

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LinkerGeneratedType"/> class.
		/// </summary>
		/// <param name="moduleTypeSystem">The module type system.</param>
		/// <param name="nameSpace">The name space.</param>
		/// <param name="name">The name.</param>
		/// <param name="baseType">Type of the base.</param>
		public LinkerGeneratedType(ITypeModule moduleTypeSystem, string nameSpace, string name, RuntimeType baseType) :
			base(moduleTypeSystem, Token.Zero, baseType)
		{
			if (nameSpace == null)
				throw new ArgumentNullException(@"namespace");
			if (name == null)
				throw new ArgumentNullException(@"name");

			base.Namespace = nameSpace;
			base.Name = name;

			this.methods = new List<RuntimeMethod>();
		}

		#endregion // Construction

		public void AddMethod(RuntimeMethod method)
		{
			this.methods.Add(method);
		}
	}
}
