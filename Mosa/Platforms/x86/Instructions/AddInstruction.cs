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
using System.Diagnostics;

namespace Mosa.Platforms.x86.Instructions
{
    sealed class AddInstruction : IR.TwoOperandInstruction
    {
        #region Construction

        public AddInstruction()
        {
        }

        public AddInstruction(Operand destination, Operand source) :
            base(destination, source)
        {
        }

        #endregion // Construction

        public override string ToString()
        {
            return String.Format(@"x86 add {0}, {1} ; {0} += {1}", this.Operand0, this.Operand1);
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
                x86.Add(this, arg);
            else
                base.Visit((IInstructionVisitor<ArgType>)visitor, arg);
        }
    }
}
