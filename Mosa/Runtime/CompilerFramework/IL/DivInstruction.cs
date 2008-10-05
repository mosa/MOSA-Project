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
    public class DivInstruction : ArithmeticInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="DivInstruction"/> class.
        /// </summary>
        /// <param name="code">The opcode of the arithmetic instruction to create.</param>
        public DivInstruction(OpCode code) :
            base(code)
        {
            if (OpCode.Div != code)
                throw new ArgumentException(@"Opcode not supported.", @"code");
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
            return String.Format("{0} ; {1} = {2} / {3}", base.ToString(), this.Results[0], ops[0], ops[1]);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Div(this, arg);
        }

        #endregion // #region ArithmeticInstruction Overrides
    }
}
