/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Representations the x86 move cr0 instruction.
    /// </summary>
    public sealed class MoveRegToCRInstruction : TwoOperandInstruction
    {
		#region Data Members

		private static readonly byte[] MOVCR = new byte[] { 0x0F, 0x22 };

		#endregion

        #region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="emitter">The emitter.</param>
        protected override void Emit(Context ctx, MachineCodeEmitter emitter)
        {
			emitter.Emit(MOVCR, Convert.ToByte((ctx.Result as ConstantOperand).Value), ctx.Operand1, null);
        }
        #endregion // Methods
    }
}
