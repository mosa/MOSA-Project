/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Representations the x86 int instruction.
    /// </summary>
	public sealed class IntInstruction : OneOperandInstruction
    {
        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        /// <param name="third"></param>
        /// <returns></returns>
        protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
        {
            return new OpCode(new byte[] { 0xCD });
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

        #endregion // Methods
    }
}
