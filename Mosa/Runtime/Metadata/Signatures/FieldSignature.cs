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
    public sealed class FieldSignature : Signature
    {
        private CustomMod[] _customMods;
        private SigType _type;

        public FieldSignature()
        {
        }

        public CustomMod[] CustomMods
        {
            get { return _customMods; }
        }

        public SigType Type
        {
            get { return _type; }
        }


        protected override void ParseSignature(byte[] buffer, ref int index)
        {
            if (FIELD == buffer[index])
            {
                index++;
                _customMods = CustomMod.ParseCustomMods(buffer, ref index);
                _type = SigType.ParseTypeSignature(buffer, ref index);
            }
        }

        private const int FIELD = 0x06;
    }
}
