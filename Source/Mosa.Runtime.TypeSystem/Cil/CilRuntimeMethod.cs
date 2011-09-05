/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System.Collections.Generic;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.TypeSystem.Cil
{
	/// <summary>
	/// A CIL specialization of <see cref="RuntimeMethod"/>.
	/// </summary>
	sealed class CilRuntimeMethod : RuntimeMethod
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
		public CilRuntimeMethod(ITypeModule module, string name, MethodSignature signature, Token token, RuntimeType declaringType, MethodAttributes methodAttributes, MethodImplAttributes methodImplAttributes, uint rva) :
			base(module, token, declaringType)
		{
			base.Attributes = methodAttributes;
			base.ImplAttributes = methodImplAttributes;
			base.Rva = rva;
			this.Name = name;
			this.Signature = signature;

			this.Parameters = new List<RuntimeParameter>();
		}

		#endregion // Construction

	}
}
