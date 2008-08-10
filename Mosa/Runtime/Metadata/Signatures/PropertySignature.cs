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
    public class PropertySignature : Signature
    {
        protected override void ParseSignature(byte[] buffer, ref int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
