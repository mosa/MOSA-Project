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
	public class MethodSpecSignature : Signature
	{
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
		public SigType[] Types { get; private set; }

		/// <summary>
		/// Parses the signature.
		/// </summary>
		/// <param name="reader">The reader.</param>
		protected override void ParseSignature(SignatureReader reader)
		{
			if (reader.ReadByte() != 0x0A)
				throw new InvalidOperationException(@"Invalid signature.");

			int genArgCount = reader.ReadCompressedInt32();
			Types = new SigType[genArgCount];
			for (int i = 0; i < genArgCount; i++)
				Types[i] = SigType.ParseTypeSignature(reader);
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(base.ToString() + " ");

			if (Types.Length != 0)
			{
				sb.Append(" [ ");

				foreach (var type in Types)
				{
					sb.Append(type.ToString());
					sb.Append(", ");
				}

				sb.Length = sb.Length - 2;

				sb.Append(" ]");
			}

			return sb.ToString();
		}
	}
}