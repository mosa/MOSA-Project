/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *
 */

using System;

using Mosa.Runtime.CompilerFramework;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86.Instructions
{
    /// <summary>
    /// Intermediate representation for the x86 cvtss2sd instruction.
    /// </summary>
    public class Cvtss2sdInstruction : IR.TwoOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="Cvtss2sdInstruction"/> class.
        /// </summary>
        public Cvtss2sdInstruction()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cvtss2sdInstruction"/> class.
        /// </summary>
        /// <param name="result">The result operand.</param>
        /// <param name="op1">The source operand.</param>
        public Cvtss2sdInstruction(Operand result, Operand op1) :
            base(result, op1)
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
            return String.Format("x86 cvtss2sd {0}, {1} ; {0} = (double){1}", this.Operand0, this.Operand1);
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
            if (x86v != null)
            {
                x86v.Cvtss2sd(this, arg);
            }
            else
            {
                visitor.Visit(this, arg);
            }
        }

        #endregion // TwoOperandInstruction Overrides
    }
}
