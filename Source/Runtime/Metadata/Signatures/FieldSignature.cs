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
        /// <param name="buffer">The buffer.</param>
        /// <param name="index">The index.</param>
        protected override void ParseSignature(ISignatureContext context, byte[] buffer, ref int index)
        {
            if (Field != buffer[index]) 
                return;

            index++;

            base.ParseSignature(context, buffer, ref index);
        }

        /// <summary>
        /// 
        /// </summary>
        private const int Field = 0x06;
    }
}
