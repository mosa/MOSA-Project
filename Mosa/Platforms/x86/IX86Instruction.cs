/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// Represents an x86 instruction to the x86 code generator.
    /// </summary>
    public interface IX86Instruction
    {
        /// <summary>
        /// Used by the x86 code generator to emit code.
        /// </summary>
        /// <param name="stream">The stream to emit code into.</param>
        void Emit(Stream stream);
    }
}
