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
        SigType[] _types;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodSpecSignature"/> class.
        /// </summary>
        public MethodSpecSignature()
        {
        }

        /// <summary>
        /// Gets the types.
        /// </summary>
        /// <value>The types.</value>
        public SigType[] Types { get { return _types; } }

        /// <summary>
        /// Parses the signature.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="index">The index.</param>
        protected override void ParseSignature(byte[] buffer, ref int index)
        {
            if (0x0A != buffer[index])
                throw new InvalidOperationException(@"Invalid signature.");
            index++;

            int genArgCount = Utilities.ReadCompressedInt32(buffer, ref index);
            _types = new SigType[genArgCount];
            for (int i = 0; i < genArgCount; i++)
                _types[i] = SigType.ParseTypeSignature(buffer, ref index);
        }
    }
}
