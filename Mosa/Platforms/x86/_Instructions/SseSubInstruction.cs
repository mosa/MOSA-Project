/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86.Instructions
{
    /// <summary>
    /// Intermediate representation of an SSE based subtraction instruction.
    /// </summary>
    sealed class SseSubInstruction : IR.TwoOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="SseSubInstruction"/> class.
        /// </summary>
        public SseSubInstruction()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SseSubInstruction"/> class.
        /// </summary>
        /// <param name="destination">The destination operand.</param>
        /// <param name="source">The second operand.</param>
        public SseSubInstruction(Operand destination, Operand source) :
            base(destination, source)
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
        public override string  ToString()
        {
            return String.Format(@"x86 subsd {0}, {1} ; {0} -= {1}", this.Operand0, this.Operand1);
        }

        /// <summary>
        /// Visits the specified visitor.
        /// </summary>
        /// <typeparam name="ArgType">The type of the rg type.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <param name="arg">The arg.</param>
        protected override void Visit<ArgType>(IR.IIRVisitor<ArgType> visitor, ArgType arg)
        {
            IX86InstructionVisitor<ArgType> x86 = visitor as IX86InstructionVisitor<ArgType>;
            Debug.Assert(null != x86);
            if (null != x86)
                x86.SseSub(this, arg);
            else
                visitor.Visit(this, arg);
        }

        #endregion // TwoOperandInstruction Overrides
    }
}
