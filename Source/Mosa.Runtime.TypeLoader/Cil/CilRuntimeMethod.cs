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

using Mosa.Runtime.TypeLoader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.TypeLoader.Cil
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
		/// <param name="metadataProvider">The metadata provider.</param>
		/// <param name="token">The token.</param>
		/// <param name="method">The method.</param>
		/// <param name="declaringType">Type of the declaring.</param>
		public CilRuntimeMethod(IMetadataProvider metadataProvider, int token, MethodDefRow method, RuntimeType declaringType) :
			base((int)token, declaringType)
		{
			this.nameIdx = method.NameStringIdx;
			this.signatureBlobIdx = method.SignatureBlobIdx;
			base.Attributes = method.Flags;
			base.ImplAttributes = method.ImplFlags;
			base.Rva = method.Rva;

			this.Name = metadataProvider.ReadString(this.nameIdx);
			Debug.Assert(this.Name != null, @"Failed to retrieve CilRuntimeMethod name.");

			this.Signature = new MethodSignature(metadataProvider, signatureBlobIdx);
			this.Parameters = new List<RuntimeParameter>();
		}

		#endregion // Construction

		#region RuntimeMethod Overrides

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		public override bool Equals(RuntimeMethod other)
		{
			CilRuntimeMethod crm = other as CilRuntimeMethod;
			return (crm != null && this.nameIdx == crm.nameIdx && this.signatureBlobIdx == crm.signatureBlobIdx && base.Equals(other) == true);
		}

		#endregion // RuntimeMethod Overrides
	}
}
