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
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework.IL
{
    public class UnboxInstruction : BoxingInstruction
    {
        #region Construction

        public UnboxInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Unbox == code);
            if (OpCode.Unbox != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Unbox(this, arg);
        }

        public override string ToString()
        {
            return String.Format(@"{0} = unbox({1})", this.Results[0], this.Operands[0]);
        }

        #endregion // Methods
    }
}
