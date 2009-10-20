/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Intermediate representation of the x86 push instruction.
    /// </summary>
    public sealed class InvlpgInstruction : OneOperandInstruction
    {
        #region OneOperandInstruction Overrides
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        /// <param name="third"></param>
        /// <returns></returns>
        protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
        {
            return new OpCode(new byte[] { 0x0F, 0x01 });
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Invlpg(context);
		}

        #endregion // OneOperandInstruction Overrides
    }
}
