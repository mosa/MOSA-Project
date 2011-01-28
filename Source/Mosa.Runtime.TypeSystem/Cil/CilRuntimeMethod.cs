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
using System.Text;
using System.Diagnostics;

using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.TypeSystem.Cil
{
	/// <summary>
	/// A CIL specialization of <see cref="RuntimeMethod"/>.
	/// </summary>
	sealed class CilRuntimeMethod : RuntimeMethod
	{
		#region Data Members

		/// <summary>
		/// The index of the method name.
		/// </summary>
		private TokenTypes nameIdx;

		/// <summary>
		/// Holds the blob location of the signature.
		/// </summary>
		private TokenTypes signatureBlobIdx;

		#endregion // Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CilRuntimeMethod"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="signature">The signature.</param>
		/// <param name="token">The token.</param>
		/// <param name="declaringType">Type of the declaring.</param>
		/// <param name="method">The method.</param>
		public CilRuntimeMethod(string name, MethodSignature signature, TokenTypes token, RuntimeType declaringType, MethodDefRow method) :
			base(token, declaringType)
		{
			this.nameIdx = method.NameStringIdx;
			this.signatureBlobIdx = method.SignatureBlobIdx;
			base.Attributes = method.Flags;
			base.ImplAttributes = method.ImplFlags;
			base.Rva = method.Rva;
			this.Name = name;
			this.Signature = signature;

			this.Parameters = new List<RuntimeParameter>();
		}

		#endregion // Construction

	}
}
