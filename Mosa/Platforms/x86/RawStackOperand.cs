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

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Platforms.x86
{
    public sealed class RawStackOperand : StackOperand
    {
        public RawStackOperand(SigType type, Register register, int offset)
            : base(type, register, offset)
        {
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
