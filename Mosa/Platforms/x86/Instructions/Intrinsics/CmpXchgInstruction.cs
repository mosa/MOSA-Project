/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@mosa-project.org>)
 */

using System;
using System.Diagnostics;

using Mosa.Runtime.CompilerFramework;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86.Instructions.Intrinsics
{
    /// <summary>
    /// Intermediate representation of the x86 compare-exchange instruction.
    /// </summary>
    /// <remarks>
    /// This instruction compares the value of Operand0 and Operand1. If they are
    /// equal, Operand0 is set to the value of Operand2.
    /// </remarks>
    public sealed class CmpXchgInstruction : IR.ThreeOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CmpXchgInstruction"/> class.
        /// </summary>
        public CmpXchgInstruction()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmpXchgInstruction"/> class.
        /// </summary>
        /// <param name="destination">The destination operand.</param>
        /// <param name="comparator">The comparator value.</param>
        /// <param name="value">The value to store in <paramref name="destination"/>, if destination equals <paramref name="comparator"/>.</param>
        public CmpXchgInstruction(Operand destination, Operand comparator, Operand value) :
            base(destination, comparator, value)
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
            return String.Format(@"x86 cmpxchg {0}, {1}, {2} ; if ({0} == {1}) {0} = {2} ", this.Operand0, this.Operand1, this.Operand2);
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
                x86.CmpXchg(this, arg);
            else
                visitor.Visit(this, arg);
        }

        #endregion // ThreeOperandInstruction Overrides
    }
}
