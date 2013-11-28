/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Metadata;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework.Linker
{
	/// <summary>
	/// Represents a compiler generated type.
	/// </summary>
	public sealed class LinkerGeneratedType : RuntimeType
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LinkerGeneratedType" /> class.
		/// </summary>
		/// <param name="moduleTypeSystem">The module type system.</param>
		/// <param name="nameSpace">The name space.</param>
		/// <param name="name">The name.</param>
		/// <param name="baseType">Type of the base.</param>
		public LinkerGeneratedType(ITypeModule moduleTypeSystem, string nameSpace, string name, RuntimeType baseType) :
			base(moduleTypeSystem, Token.Zero, name, baseType, nameSpace)
		{
		}

		#endregion Construction

		public void AddMethod(RuntimeMethod method)
		{
			Methods.Add(method);
		}
	}
}