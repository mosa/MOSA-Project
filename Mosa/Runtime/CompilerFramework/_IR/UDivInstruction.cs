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

namespace Mosa.Runtime.CompilerFramework.IR
{
    /// <summary>
    /// Intermediate representation of the unsigned division operation.
    /// </summary>
    public class UDivInstruction : ThreeOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="UDivInstruction"/> class.
        /// </summary>
        public UDivInstruction()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UDivInstruction"/> class.
        /// </summary>
        /// <param name="result">The result operand.</param>
        /// <param name="op1">The dividend operand.</param>
        /// <param name="op2">The divisor operand.</param>
        public UDivInstruction(Operand result, Operand op1, Operand op2) :
            base(result, op1, op2)
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
            return String.Format(@"IR udiv {0}, {1}, {2} ; {0} = {1} / {2}", this.Operand0, this.Operand1, this.Operand2);
        }

        /// <summary>
        /// Abstract visitor method for intermediate representation visitors.
        /// </summary>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        protected override void Visit<ArgType>(IIRVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Visit(this, arg);
        }

        #endregion // ThreeOperandInstruction Overrides
    }
}
