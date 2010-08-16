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

using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.Metadata.Runtime
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
		/// Holds the signature token of the field.
		/// </summary>
		private TokenTypes signature;

		#endregion // Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CilRuntimeField"/> class.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <param name="field">The field.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="rva">The rva.</param>
		/// <param name="declaringType">Type of the declaring.</param>
		public CilRuntimeField(IMetadataModule module, ref FieldRow field, ulong offset, ulong rva, RuntimeType declaringType, ITypeSystem typeSystem) :
			base(module, declaringType, typeSystem)
		{
			this.nameIdx = field.NameStringIdx;
			this.signature = field.SignatureBlobIdx;
			base.Attributes = field.Flags;
			base.RVA = rva;
			//base.Offset = offset; ?
		}

		public CilRuntimeField(RuntimeField genericField, IMetadataModule module, FieldSignature signature, ITypeSystem typeSystem) :
			base(module, genericField.DeclaringType, typeSystem)
		{
			this.Name = genericField.Name;
			this.Attributes = genericField.Attributes;
			this.RVA = genericField.RVA;
			this.Signature = signature;
			
			this.SetAttributes(genericField.CustomAttributes);
		}


		#endregion // Construction

		#region RuntimeField Overrides

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		public override bool Equals(RuntimeField other)
		{
			CilRuntimeField crf = other as CilRuntimeField;
			return (crf != null && 
					this.nameIdx == crf.nameIdx && 
					this.signature == crf.signature && 
					base.Equals(other) == true);
		}

		/// <summary>
		/// Gets the type of the field.
		/// </summary>
		/// <returns>The type of the field.</returns>
		protected override FieldSignature GetSignature()
		{
			FieldSignature fsig = new FieldSignature();
			fsig.LoadSignature(this.DeclaringType, this.Module.Metadata, this.signature);
			return fsig;
		}

		/// <summary>
		/// Called to retrieve the name of the type.
		/// </summary>
		/// <returns>The name of the type.</returns>
		protected override string GetName()
		{
			string name = this.Module.Metadata.ReadString(this.nameIdx);
			Debug.Assert(name != null, @"Failed to retrieve CilRuntimeMethod name.");
			return name;
		}

		#endregion // RuntimeField Overrides
	}
}
