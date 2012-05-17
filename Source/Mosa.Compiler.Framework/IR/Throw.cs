/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Compiler.Framework.IR
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Throw : OneOperandInstruction
    {
        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="context">The context.</param>
        public override void Visit(IIRVisitor visitor, Context context)
        {
            visitor.Throw(context);
        }
    }
}
