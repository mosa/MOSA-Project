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
using IR2 = Mosa.Runtime.CompilerFramework.IR2;

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Intermediate representation of an SSE based subtraction instruction.
    /// </summary>
    public sealed class SseSubInstruction : TwoOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="SseSubInstruction"/> class.
        /// </summary>
        public SseSubInstruction()
        {
        }

        #endregion // Construction

        #region TwoOperandInstruction Overrides

		/// <summary>
		/// Returns a string representation of the instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		/// A string representation of the instruction in intermediate form.
		/// </returns>
        public override string ToString(Context context)
        {
            return String.Format(@"x86 subsd {0}, {1} ; {0} -= {1}", context.Operand1, context.Operand2);
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.SseSub(context);
		}

        #endregion // TwoOperandInstruction Overrides
    }
}
