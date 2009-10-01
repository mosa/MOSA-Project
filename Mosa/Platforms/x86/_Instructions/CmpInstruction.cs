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
using Mosa.Runtime.CompilerFramework.IR;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.Instructions
{
    /// <summary>
    /// Intermediate representation of the x86 cmp instruction.
    /// </summary>
    public sealed class CmpInstruction : TwoOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CmpInstruction"/> class.
        /// </summary>
        public CmpInstruction()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmpInstruction"/> class.
        /// </summary>
        /// <param name="op0">The first operand to compare.</param>
        /// <param name="op1">The second operand to compare.</param>
        public CmpInstruction(Operand op0, Operand op1) :
            base(op0, op1)
        {
        }

        #endregion // Construction

        #region TwoOperandInstruction Overrides

        /// <summary>
        /// Returns a string representation of the instruction.
        /// </summary>
        /// <returns>
        /// A string representation of the instruction in intermediate form.
        /// </returns>
        public override string ToString()
        {
            return String.Format(@"x86 cmp {0}, {1}", this.Operand0, this.Operand1);
        }

        /// <summary>
        /// Visits the specified visitor.
        /// </summary>
        /// <typeparam name="ArgType">The type of the rg type.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <param name="arg">The arg.</param>
        protected override void Visit<ArgType>(IIRVisitor<ArgType> visitor, ArgType arg)
        {
            IX86InstructionVisitor<ArgType> x86 = visitor as IX86InstructionVisitor<ArgType>;
            if (null != x86)
                x86.Cmp(this, arg);
            else
                visitor.Visit(this, arg);            
        }

        #endregion // TwoOperandInstruction Overrides
    }
}
