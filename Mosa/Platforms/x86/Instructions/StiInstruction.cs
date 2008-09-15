/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (<mailto:phil@thinkedge.com>)
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using IR = Mosa.Runtime.CompilerFramework.IR;

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.Instructions
{
    /// <summary>
    /// Intrinsic instruction implementation for the x86 sti instruction.
    /// </summary>
    public sealed class StiInstruction : Instruction
    {
        #region Construction
        /// <summary>
        /// 
        /// </summary>
        public StiInstruction()
        {
        }

        #endregion // Construction

        #region StiInstruction Overrides

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public override void Visit<ArgType>(IInstructionVisitor<ArgType> visitor, ArgType arg)
        {
            IX86InstructionVisitor<ArgType> x86visitor = visitor as IX86InstructionVisitor<ArgType>;
            Debug.Assert(null != x86visitor);
            if (null != x86visitor)
                x86visitor.Sti(this, arg);
        }

        #endregion // StiInstruction Overrides
    }
}
