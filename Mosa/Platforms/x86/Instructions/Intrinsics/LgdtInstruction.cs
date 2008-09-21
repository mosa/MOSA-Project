/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using IL = Mosa.Runtime.CompilerFramework.IL;
using IR = Mosa.Runtime.CompilerFramework.IR;
using System.Diagnostics;

namespace Mosa.Platforms.x86.Instructions.Intrinsics
{
    /// <summary>
    /// Intermediate representation of the x86 lgdt instruction.
    /// </summary>
    sealed class LgdtInstruction : IR.OneOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LgdtInstruction"/> class.
        /// </summary>
        public LgdtInstruction() :
            base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LgdtInstruction"/> class.
        /// </summary>
        /// <param name="source">The source operand.</param>
        public LgdtInstruction(Operand source) :
            base(source)
        {
        }

        #endregion // Construction

        #region OneOperandInstruction Overrides

        /// <summary>
        /// Returns a string representation of the instruction.
        /// </summary>
        /// <returns>
        /// A string representation of the instruction in intermediate form.
        /// </returns>
        public override string ToString()
        {
            return String.Format(@"x86 lgdt {0} ", this.Operand0);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        protected override void Visit<ArgType>(IR.IIRVisitor<ArgType> visitor, ArgType arg)
        {
            IX86InstructionVisitor<ArgType> x86 = visitor as IX86InstructionVisitor<ArgType>;
            Debug.Assert(null != x86);
            if (null != x86)
                x86.Lgdt(this, arg);
            else
                visitor.Visit(this, arg);
        }

        #endregion // OneOperandInstruction Overrides
    }
}
