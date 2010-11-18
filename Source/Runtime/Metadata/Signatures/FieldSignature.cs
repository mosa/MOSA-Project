/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;

namespace Mosa.Runtime.Metadata.Signatures
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class FieldSignature : VariableSignature
	{

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
		public FieldSignature(IMetadataProvider provider, TokenTypes token)
			: base(provider, token)
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

		public void ApplyConcreteType(SigType[] genericArguments)
		{
			if (this.Type is VarSigType)
			{
				this.Type = genericArguments[(Type as VarSigType).Index];
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private const int Field = 0x06;
	}
}
