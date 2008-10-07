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
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86.Instructions
{
    /// <summary>
    /// Intermediate representation of the x86 shld instruction.
    /// </summary>
    public class ShldInstruction : IR.ThreeOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ShldInstruction"/> class.
        /// </summary>
        public ShldInstruction()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShldInstruction"/> class.
        /// </summary>
        /// <param name="dst">The destination operand.</param>
        /// <param name="src">The source operand.</param>
        /// <param name="count">The count.</param>
        public ShldInstruction(Operand dst, Operand src, Operand count) :
            base(dst, src, count)
        {
        }

        #endregion // Construction

        #region ThreeOperandInstruction Overrides

        /// <summary>
        /// Returns a string representation of the instruction.
        /// </summary>
        /// <returns>
        /// A string representation of the instruction in intermediate form.
        /// </returns>
        public override string ToString()
        {
            return String.Format(@"x86 shld {0}, {1}, {2}", this.Operand0, this.Operand1, this.Operand2);
        }

        /// <summary>
        /// Visits the specified visitor.
        /// </summary>
        /// <typeparam name="ArgType">The type of the rg type.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <param name="arg">The arg.</param>
        protected override void Visit<ArgType>(IR.IIRVisitor<ArgType> visitor, ArgType arg)
        {
            IX86InstructionVisitor<ArgType> x86v = visitor as IX86InstructionVisitor<ArgType>;
            if (null != x86v)
                x86v.Shld(this, arg);
            else
                visitor.Visit(this, arg);
        }

        #endregion // ThreeOperandInstruction Overrides
    }
}
