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
    public sealed class SZArray : SigType
    {
        private CustomMod[] _customMods;
        private SigType _type;

        public SZArray(CustomMod[] customMods, SigType type)
            : base(CilElementType.SZArray)
        {
            _customMods = customMods;
            _type = type;
        }

        public CustomMod[] CustomMods { get { return _customMods; } }
        public SigType ElementType { get { return _type; } }
    }
}
