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
    public class RefSigType : SigType
    {
        private SigType _type;

        public RefSigType(SigType type)
            : base(CilElementType.ByRef)
        {
            _type = type;
        }

        public SigType ElementType { get { return _type; } }
    }
}
