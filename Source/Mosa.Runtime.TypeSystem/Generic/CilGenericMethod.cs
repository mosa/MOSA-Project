/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Text;
using System.Diagnostics;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.TypeSystem.Cil;

namespace Mosa.Runtime.TypeSystem.Generic
{

	internal class CilGenericMethod : RuntimeMethod
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CilGenericMethod"/> class.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <param name="genericMethod">The generic method.</param>
		/// <param name="signature">The signature.</param>
		/// <param name="declaringType">Type of the declaring.</param>
		public CilGenericMethod(ITypeModule module, CilRuntimeMethod genericMethod, MethodSignature signature, RuntimeType declaringType) :
			base(module, genericMethod.Token, declaringType)
		{
			this.Signature = signature;
			this.Attributes = genericMethod.Attributes;
			this.ImplAttributes = genericMethod.ImplAttributes;
			this.Rva = genericMethod.Rva;
			this.Parameters = genericMethod.Parameters;
			base.Name = genericMethod.Name;
		}

	}
}
