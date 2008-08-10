/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Metadata.Signatures
{
    public class MethodSpecSignature : Signature
    {
        SigType[] _types;

        public MethodSpecSignature()
        {
        }

        public SigType[] Types { get { return _types; } }

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
