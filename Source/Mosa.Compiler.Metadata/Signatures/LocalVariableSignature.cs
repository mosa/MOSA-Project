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

namespace Mosa.Compiler.Metadata.Signatures
{
	/// <summary>
	///
	/// </summary>
	public class LocalVariableSignature : Signature
	{

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
		/// Holds the signature types of all local variables in order of definition.
		/// </summary>
		/// <value>The types.</value>
		public VariableSignature[] Locals { get; private set; }

		/// <summary>
		/// Parses the signature.
		/// </summary>
		/// <param name="reader">The reader.</param>
		protected override void ParseSignature(SignatureReader reader)
		{
			// Check signature identifier
			if (reader.ReadByte() != 0x07)
				throw new ArgumentException("Token doesn't represent a local variable signature.", "token");

			// Retrieve the number of locals
			int count = reader.ReadCompressedInt32();
			if (count != 0)
			{
				Locals = new VariableSignature[count];
				for (int i = 0; i < count; i++)
				{
					Locals[i] = new VariableSignature(reader);
				}
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(base.ToString());
			sb.Append(' ');

			if (Locals.Length != 0)
			{
				sb.Append(" [ ");

				foreach (var local in Locals)
				{
					sb.Append(local.ToString());
					sb.Append(", ");
				}

				sb.Length = sb.Length - 2;

				sb.Append(" ]");
			}

			return sb.ToString();
		}
	}
}