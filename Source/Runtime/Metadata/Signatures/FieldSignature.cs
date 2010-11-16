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

		/// <summary>
		/// 
		/// </summary>
		private const int Field = 0x06;
	}
}
