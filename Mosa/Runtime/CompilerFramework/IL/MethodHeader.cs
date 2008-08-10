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
using Mosa.Runtime.Metadata;

namespace Mosa.Runtime.CompilerFramework.IL
{
    public struct MethodHeader
    {
        // Header flags
        public MethodFlags flags;
        // Maximum stack size
        public ushort maxStack;
        // Size of the code in bytes
        public uint codeSize;
        // Local variable signature token
        public TokenTypes localsSignature;
    }
}
