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
    public class TypeSpecSignature : Signature
    {
        private SigType _type;

        public SigType Type
        {
            get { return _type; }
        }

        protected override void ParseSignature(byte[] buffer, ref int index)
        {
            _type = SigType.ParseTypeSignature(buffer, ref index);
        }
    }
}
