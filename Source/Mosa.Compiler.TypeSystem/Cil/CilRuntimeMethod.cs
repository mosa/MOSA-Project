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

		#endregion // Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CilRuntimeMethod"/> class.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <param name="name">The name.</param>
		/// <param name="signature">The signature.</param>
		/// <param name="token">The token.</param>
		/// <param name="declaringType">Type of the declaring.</param>
		/// <param name="methodAttributes">The method attributes.</param>
		/// <param name="methodImplAttributes">The method impl attributes.</param>
		/// <param name="rva">The rva.</param>
		public CilRuntimeMethod(ITypeModule module, string name, SigType returnType, bool hasThis, bool hasExplicitThis, SigType[] sigParameters, Token token, RuntimeType declaringType, MethodAttributes methodAttributes, MethodImplAttributes methodImplAttributes, uint rva) :
			base(module, token, declaringType)
		{
			base.Attributes = methodAttributes;
			base.ImplAttributes = methodImplAttributes;
			base.Rva = rva;
			this.Name = name;

			this.ReturnType = returnType;
			this.HasThis = hasThis;
			this.HasExplicitThis = hasExplicitThis;
			this.SigParameters = sigParameters;

			this.Parameters = new List<RuntimeParameter>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CilRuntimeMethod" /> class.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <param name="genericMethod">The generic method.</param>
		/// <param name="signature">The signature.</param>
		/// <param name="declaringType">Type of the declaring.</param>
		public CilRuntimeMethod(ITypeModule module, CilRuntimeMethod genericMethod, SigType returnType, bool hasThis, bool hasExplicitThis, SigType[] sigParameters, RuntimeType declaringType) :
			base(module, genericMethod.Token, declaringType)
		{
			this.Attributes = genericMethod.Attributes;
			this.ImplAttributes = genericMethod.ImplAttributes;
			this.Rva = genericMethod.Rva;
			this.Parameters = genericMethod.Parameters;
			base.Name = genericMethod.Name;

			this.ReturnType = returnType;
			this.HasThis = hasThis;
			this.HasExplicitThis = hasExplicitThis;
			this.SigParameters = sigParameters;
		}

		#endregion // Construction

	}
}
