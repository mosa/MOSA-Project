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
	public class LocalVariableSignature : Signature
	{
		/// <summary>
		/// Holds the signature types of all local variables in order of definition.
		/// </summary>
		private VariableSignature[] locals;

		/// <summary>
		/// A shared empty array for those signatures, who do not have local variables.
		/// </summary>
		private static VariableSignature[] Empty = new VariableSignature[0];

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalVariableSignature" /> class.
		/// </summary>
		/// <param name="reader">The reader.</param>
		public LocalVariableSignature(SignatureReader reader)
			: base(reader)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalVariableSignature" /> class.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <param name="token">The token.</param>
		public LocalVariableSignature(IMetadataProvider provider, HeapIndexToken token)
			: base(provider, token)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalVariableSignature" /> class.
		/// </summary>
		public LocalVariableSignature()
		{
			this.locals = LocalVariableSignature.Empty;
		}

		/// <summary>
		/// Gets the types.
		/// </summary>
		/// <value>The types.</value>
		public VariableSignature[] Locals
		{
			get { return this.locals; }
		}

		/// <summary>
		/// Parses the signature.
		/// </summary>
		/// <param name="reader">The reader.</param>
		protected override void ParseSignature(SignatureReader reader)
		{
			// Check signature identifier
			if (reader.ReadByte() != 0x07)
				throw new ArgumentException(@"Token doesn't represent a local variable signature.", @"token");

			// Retrieve the number of locals
			int count = reader.ReadCompressedInt32();
			if (count != 0)
			{
				this.locals = new VariableSignature[count];
				for (int i = 0; i < count; i++)
				{
					locals[i] = new VariableSignature(reader);
				}
			}
		}
	}
}