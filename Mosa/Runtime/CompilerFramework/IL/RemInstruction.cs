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
    public class RemInstruction : ArithmeticInstruction
    {
        #region Construction

        public RemInstruction(OpCode code) :
            base(code)
        {
            if (OpCode.Rem != code)
                throw new ArgumentException(@"Opcode not supported.", @"code");
        }

        #endregion // Construction

        #region ArithmeticInstruction Overrides

        public override string ToString()
        {
            Operand[] ops = this.Operands;
            return String.Format("{0} ; {1} = {2} % {3}", base.ToString(), this.Results[0], ops[0], ops[1]);
        }

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Rem(this);
        }

        #endregion // #region ArithmeticInstruction Overrides
    }
}
