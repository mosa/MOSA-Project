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
    public sealed class Var : SigType
    {
        private int _index;

        public Var(int index)
            : base(CilElementType.Var)
        {
            _index = index;
        }

        public int Index { get { return _index; } }
    }
}
