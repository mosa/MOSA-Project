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

using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.Vm;

namespace Mosa.Tools.Compiler
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
		public LinkerGeneratedType(IModuleTypeSystem moduleTypeSystem, string nameSpace, string name) :
			base(moduleTypeSystem, 0)
		{
			if (nameSpace == null)
				throw new ArgumentNullException(@"namespace");
			if (name == null)
				throw new ArgumentNullException(@"name");

			this.methods = new List<RuntimeMethod>();

			base.Namespace = nameSpace;
			base.Name = name;
			base.Methods = this.methods;
			base.Fields = new List<RuntimeField>();
		}

		#endregion // Construction

		#region RuntimeType Overrides

		/// <summary>
		/// Gets the base type.
		/// </summary>
		/// <returns>The base type.</returns>
		protected override RuntimeType GetBaseType()
		{
			// Compiler generated types don't have a base type.
			return null;
		}

		/// <summary>
		/// Called to retrieve the name of the type.
		/// </summary>
		/// <returns>The name of the type.</returns>
		protected override string GetName()
		{
			return this.Name;
		}

		/// <summary>
		/// Called to retrieve the namespace of the type.
		/// </summary>
		/// <returns>The namespace of the type.</returns>
		protected override string GetNamespace()
		{
			return this.Namespace;
		}

		protected override IList<RuntimeType> LoadInterfaces()
		{
			return NoInterfaces;
		}

		#endregion // RuntimeType Overrides

		public void AddMethod(RuntimeMethod method)
		{
			this.methods.Add(method);
		}
	}
}
