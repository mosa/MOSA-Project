/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;

namespace Mosa.Compiler.Metadata.Signatures
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class FieldSignature : VariableSignature
	{

		private const int Field = 0x06;

		/// <summary>
		/// Loads the signature.
		/// </summary>
		/// <param name="reader">The reader.</param>
		public FieldSignature(SignatureReader reader)
			: base(reader)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldSignature"/> class.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <param name="token">The token.</param>
		public FieldSignature(IMetadataProvider provider, HeapIndexToken token)
			: base(provider, token)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldSignature"/> class.
		/// </summary>
		/// <param name="signature">The signature.</param>
		public FieldSignature(FieldSignature signature)
			: base(signature)
		{
		}

		/// <summary>
		/// FieldSignature signature is indexed by the Field.Signature column
		/// </summary>
		/// <param name="reader">The reader.</param>
		protected override void ParseSignature(SignatureReader reader)
		{
			if (Field != reader.ReadByte())
			{
				throw new InvalidOperationException(@"Invalid method definition signature.");
			}

			base.ParseSignature(reader);
		}

	}
}
