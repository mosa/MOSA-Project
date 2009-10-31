/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Representations the x86 int instruction.
    /// </summary>
    public sealed class DebugInstruction : BaseInstruction
    {
        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="emitter"></param>
        public override void Emit(Context ctx, MachineCodeEmitter emitter)
        {
            emitter.WriteByte(0xCC);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="context">The context.</param>
        public override void Visit(IX86Visitor visitor, Context context)
        {
            visitor.Int(context);
        }

        #endregion // Methods
    }
}
