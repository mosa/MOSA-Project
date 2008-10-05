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
    /// <summary>
    /// 
    /// </summary>
    public class BoxInstruction : BoxingInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxInstruction"/> class.
        /// </summary>
        /// <param name="code">The opcode of the box instruction, which must be OpCode.Box.</param>
        public BoxInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Box == code);
            if (OpCode.Box != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Returns a formatted representation of the opcode.
        /// </summary>
        /// <returns>The code as a string value.</returns>
        public override string ToString()
        {
            return String.Format(@"{2} ; {0} = box({1})", this.Results[0], this.Operands[0], base.ToString());
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Box(this, arg);
        }

        #endregion // Methods
    }
}
