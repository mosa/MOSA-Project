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
    /// <summary>
    /// 
    /// </summary>
    public struct MethodHeader
    {
        /// <summary>
        /// Header flags 
        /// </summary>
        public MethodFlags flags;
        /// <summary>
        /// Maximum stack size 
        /// </summary>
        public ushort maxStack;
        /// <summary>
        /// Size of the code in bytes 
        /// </summary>
        public uint codeSize;
        /// <summary>
        /// Local variable signature token 
        /// </summary>
        public TokenTypes localsSignature;
    }
}
