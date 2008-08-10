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
using Mosa.Runtime.Metadata.Tables;
using System.Diagnostics;

namespace Mosa.Runtime.Metadata.Signatures
{
    public class LocalVariableSignature : Signature
    {
        private SigType[] _types;

        public SigType[] Types
        {
            get { return _types; }
        }

        protected override void ParseSignature(byte[] buffer, ref int index)
        {
            // Check signature identifier
            if (buffer[index++] != 0x07)
                throw new ArgumentException(@"Token doesn't represent a local variable signature.", @"token");

            // Retrieve the number of locals
            int count = Utilities.ReadCompressedInt32(buffer, ref index);
            if (0 != count)
            {
                _types = new SigType[count];
                for (int i = 0; i < count; i++)
                {
                    _types[i] = SigType.ParseTypeSignature(buffer, ref index);
                }
            }
        }

        public static LocalVariableSignature Parse(IMetadataProvider provider, TokenTypes token)
        {
            byte[] buffer;
            int index = 0;
            provider.Read(token, out buffer);
            LocalVariableSignature sig = new LocalVariableSignature();
            sig.ParseSignature(buffer, ref index);
            Debug.Assert(index == buffer.Length, @"Signature parser didn't complete.");
            return sig;
        }
    }
}
