/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System.Collections.Generic;

using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.TypeSystem.Cil
{
	/// <summary>
	/// A CIL specialization of <see cref="RuntimeMethod"/>.
	/// </summary>
	sealed public class CilRuntimeMethod : RuntimeMethod
	{
		#region Data Members

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CilRuntimeMethod" /> class.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <param name="name">The name.</param>
		/// <param name="returnType">Type of the return.</param>
		/// <param name="hasThis">if set to <c>true</c> [has this].</param>
		/// <param name="hasExplicitThis">if set to <c>true</c> [has explicit this].</param>
		/// <param name="sigParameters">The sig parameters.</param>
		/// <param name="token">The token.</param>
		/// <param name="declaringType">Type of the declaring.</param>
		/// <param name="methodAttributes">The method attributes.</param>
		/// <param name="methodImplAttributes">The method impl attributes.</param>
		/// <param name="rva">The rva.</param>
		public CilRuntimeMethod(ITypeModule module, string name, SigType returnType, bool hasThis, bool hasExplicitThis, SigType[] sigParameters, Token token, RuntimeType declaringType, MethodAttributes methodAttributes, MethodImplAttributes methodImplAttributes, uint rva) :
			base(module, name, token, declaringType, null, sigParameters)
		{
			base.Attributes = methodAttributes;
			base.ImplAttributes = methodImplAttributes;
			base.Rva = rva;

			this.ReturnType = returnType;
			this.HasThis = hasThis;
			this.HasExplicitThis = hasExplicitThis;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CilRuntimeMethod" /> class.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <param name="genericMethod">The generic method.</param>
		/// <param name="returnType">Type of the return.</param>
		/// <param name="hasThis">if set to <c>true</c> [has this].</param>
		/// <param name="hasExplicitThis">if set to <c>true</c> [has explicit this].</param>
		/// <param name="sigParameters">The sig parameters.</param>
		/// <param name="declaringType">Type of the declaring.</param>
		public CilRuntimeMethod(ITypeModule module, CilRuntimeMethod genericMethod, SigType returnType, bool hasThis, bool hasExplicitThis, SigType[] sigParameters, RuntimeType declaringType) :
			base(module, genericMethod.Name, genericMethod.Token, declaringType, genericMethod.Parameters, sigParameters)
		{
			this.Attributes = genericMethod.Attributes;
			this.ImplAttributes = genericMethod.ImplAttributes;
			this.Rva = genericMethod.Rva;

			this.ReturnType = returnType;
			this.HasThis = hasThis;
			this.HasExplicitThis = hasExplicitThis;
		}

		#endregion Construction
	}
}