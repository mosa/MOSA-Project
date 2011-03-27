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

namespace Mosa.Runtime.Metadata.Signatures
{
	/// <summary>
	/// 
	/// </summary>
	public class MethodSpecSignature : Signature
	{
		/// <summary>
		/// 
		/// </summary>
		private SigType[] types;

		/// <summary>
		/// Initializes a new instance of the <see cref="VariableSignature"/> class.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <param name="token">The token.</param>
		public MethodSpecSignature(IMetadataProvider provider, HeapIndexToken token)
			: base(provider, token)
		{
		}

		/// <summary>
		/// Gets the types.
		/// </summary>
		/// <value>The types.</value>
		public SigType[] Types
		{
			get { return types; }
		}

		/// <summary>
		/// Parses the signature.
		/// </summary>
		/// <param name="reader">The reader.</param>
		protected override void ParseSignature(SignatureReader reader)
		{
			if (reader.ReadByte() != 0x0A)
				throw new InvalidOperationException(@"Invalid signature.");

			int genArgCount = reader.ReadCompressedInt32();
			types = new SigType[genArgCount];
			for (int i = 0; i < genArgCount; i++)
				types[i] = SigType.ParseTypeSignature(reader);
		}

	}
}
