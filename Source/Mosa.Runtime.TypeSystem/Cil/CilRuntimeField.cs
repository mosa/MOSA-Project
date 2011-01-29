/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Diagnostics;

using Mosa.Runtime;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.TypeSystem;

namespace Mosa.Runtime.TypeSystem.Cil
{
	/// <summary>
	/// A CIL specialization of <see cref="RuntimeField"/>.
	/// </summary>
	sealed class CilRuntimeField : RuntimeField
	{
		#region Data Members

		/// <summary>
		/// Holds the name index of the RuntimeField.
		/// </summary>
		private TokenTypes nameIdx;

		/// <summary>
		/// Holds the blob location of the signature.
		/// </summary>
		private TokenTypes signatureBlobIdx;

		#endregion // Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CilRuntimeField"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="signature">The signature.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="rva">The rva.</param>
		/// <param name="declaringType">Type of the declaring.</param>
		/// <param name="field">The field.</param>
		public CilRuntimeField(string name, FieldSignature signature, uint offset, uint rva, RuntimeType declaringType, FieldRow field) :
			base(declaringType)
		{
			this.Name = name;
			this.Signature = signature; 
			base.Attributes = field.Flags;
			base.RVA = rva;
			this.nameIdx = field.NameStringIdx;
			this.signatureBlobIdx = field.SignatureBlobIdx;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CilRuntimeField"/> class.
		/// </summary>
		/// <param name="metadataProvider">The metadata provider.</param>
		/// <param name="genericField">The generic field.</param>
		/// <param name="signature">The signature.</param>
		public CilRuntimeField(IMetadataProvider metadataProvider, RuntimeField genericField, FieldSignature signature) :
			base(genericField.DeclaringType)
		{
			this.Name = genericField.Name;
			this.Attributes = genericField.Attributes;
			this.RVA = genericField.RVA;
			this.Signature = signature;
			//TODO:
			//this.SetAttributes(genericField.CustomAttributes);

			this.Signature = new FieldSignature(metadataProvider, this.signatureBlobIdx);
			this.Name = metadataProvider.ReadString(this.nameIdx);
		}

		#endregion // Construction

	}
}
