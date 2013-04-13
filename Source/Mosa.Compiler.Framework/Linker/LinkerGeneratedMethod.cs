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
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework.Linker
{
	/// <summary>
	/// Represents compiler generated methods.
	/// </summary>
	public sealed class LinkerGeneratedMethod : RuntimeMethod
	{
		#region Data Members

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LinkerGeneratedMethod"/> class.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="name">The name of the method.</param>
		/// <param name="declaringType">Type of the declaring.</param>
		public LinkerGeneratedMethod(ITypeModule typeSystem, string name, RuntimeType declaringType, SigType returnType, bool hasThis, bool hasExplicitThis, SigType[] sigParameters) :
			base(typeSystem, Token.Zero, declaringType)
		{
			if (name == null)
				throw new ArgumentNullException(@"name");

			base.Name = name;

			this.ReturnType = returnType;
			this.HasThis = hasThis;
			this.HasExplicitThis = hasExplicitThis;
			this.SigParameters = sigParameters;

			this.Parameters = new List<RuntimeParameter>();
		}

		#endregion Construction
	}
}