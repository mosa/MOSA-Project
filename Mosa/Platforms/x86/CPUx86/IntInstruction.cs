/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Intermediate representation of the x86 int instruction.
    /// </summary>
	public sealed class IntInstruction : OneOperandInstruction
    {
        #region OneOperandInstruction Overrides

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="codeStream"></param>
        public override void Emit(Context ctx, System.IO.Stream codeStream)
        {
            byte interrupt = Convert.ToByte(((ConstantOperand)ctx.Operand1).Value);
            codeStream.Write(new byte[] { 0xCD, interrupt }, 0, 2);
        }

        /// <summary>
        /// Returns a string representation of the instruction.
        /// </summary>
        /// <returns>
        /// A string representation of the instruction in intermediate form.
        /// </returns>
        public override string ToString(Context context)
        {
            return String.Format(@"x86.int {0}", context.Operand1);
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Int(context);
		}

        #endregion // OneOperandInstruction Overrides
    }
}
