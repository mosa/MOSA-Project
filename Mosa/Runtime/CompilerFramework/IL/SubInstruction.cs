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
    public class SubInstruction : ArithmeticInstruction
    {
        #region Construction

        public SubInstruction(OpCode code) :
            base(code)
        {
            if (OpCode.Sub != code)
                throw new ArgumentException(@"Opcode not supported.", @"code");
        }

        public SubInstruction(OpCode code, Operand destination, Operand source) :
            base(code, destination, source)
        {
            if (OpCode.Sub != code)
                throw new ArgumentException(@"Opcode not supported.", @"code");
        }
        
        #endregion // Construction

        #region ArithmeticInstruction Overrides

        public override string ToString()
        {
            Operand[] ops = this.Operands;
            return String.Format("{0} ; {1} = {2} - {3}", base.ToString(), this.Results[0], ops[0], ops[1]);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Sub(this, arg);
        }

        #endregion // #region ArithmeticInstruction Overrides
    }
}
