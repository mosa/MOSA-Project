/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Representations the x86 move cr0 instruction.
    /// </summary>
	public sealed class SetCRInstruction : TwoOperandInstruction, IIntrinsicInstruction
    {
        #region Methods

		/// <summary>
		/// Replaces the instrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		public void ReplaceIntrinsicCall(Context context)
		{
            if (!(context.Operand1 is ConstantOperand))
                return;

		    Operand operand1 = context.Operand1;
		    Operand operand2 = context.Operand2;
            context.SetInstruction(IR.Instruction.MoveInstruction, new RegisterOperand(operand2.Type, GeneralPurposeRegister.EAX), operand2);
            context.AppendInstruction(Instruction.MoveRegToCRInstruction, operand1, new RegisterOperand(operand2.Type, GeneralPurposeRegister.EAX));
		}

        #endregion // Methods
    }
}
