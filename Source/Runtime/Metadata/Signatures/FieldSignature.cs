/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Runtime.Metadata.Signatures
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class FieldSignature : VariableSignature
	{

        /// <summary>
        /// Parses the signature.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reader">The reader.</param>
		protected override void ParseSignature(ISignatureContext context, SignatureReader reader)
		{
			if (Field != reader.PeekByte())
				return;

            reader.SkipByte();

			base.ParseSignature(context, reader);
		}

		/// <summary>
		/// 
		/// </summary>
		private const int Field = 0x06;
	}
}
