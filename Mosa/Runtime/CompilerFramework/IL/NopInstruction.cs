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
using System.Diagnostics;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// 
    /// </summary>
    public class NopInstruction : ILInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="NopInstruction"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public NopInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Nop == code, @"Wrong opcode for NopInstruction.");
            if (OpCode.Nop != code)
                throw new ArgumentException(@"Wrong opcode.", @"code");
        }

        #endregion // Construction

        #region ILInstruction Overrides

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Nop(this, arg);
        }

        #endregion // ILInstruction Overrides
    }
}
