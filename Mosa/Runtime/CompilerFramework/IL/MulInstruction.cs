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

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// 
    /// </summary>
    public class MulInstruction : ArithmeticInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="MulInstruction"/> class.
        /// </summary>
        /// <param name="code">The opcode of the arithmetic instruction to create.</param>
        public MulInstruction(OpCode code) :
            base(code)
        {
            if (OpCode.Mul != code)
                throw new ArgumentException(@"Opcode not supported.", @"code");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MulInstruction"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="op1">The first operand.</param>
        /// <param name="op2">The second operand.</param>
        public MulInstruction(OpCode code, Operand destination, Operand op1, Operand op2) :
            base(code, destination, op1, op2)
        {
        }

        #endregion // Construction

        #region ArithmeticInstruction Overrides

        /// <summary>
        /// Returns a formatted representation of the opcode.
        /// </summary>
        /// <returns>The code as a string value.</returns>
        public override string ToString()
        {
            Operand[] ops = this.Operands;
            return String.Format("{0} ; {1} = {2} * {3}", base.ToString(), this.Results[0], ops[0], ops[1]);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Mul(this, arg);
        }

        #endregion // #region ArithmeticInstruction Overrides
    }
}
