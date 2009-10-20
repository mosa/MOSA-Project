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
    /// Intermediate representation of the x86 int instruction.
    /// </summary>
	public sealed class IntInstruction : OneOperandInstruction
    {
        #region OneOperandInstruction Overrides

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="emitter">The emitter.</param>
        public override void Emit(Context ctx, MachineCodeEmitter emitter)
        {
            byte interrupt = Convert.ToByte(((ConstantOperand)ctx.Operand1).Value);
			emitter.Write(new byte[] { 0xCD, interrupt }, 0, 2);
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
