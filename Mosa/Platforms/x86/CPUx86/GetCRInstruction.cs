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
    public sealed class GetCRInstruction : TwoOperandInstruction, IIntrinsicInstruction
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

            Operand result = context.Result;
            Operand operand1 = context.Operand1;

            context.SetInstruction(Instruction.MoveCRToRegInstruction, new RegisterOperand(result.Type, GeneralPurposeRegister.EAX), operand1);
            context.AppendInstruction(IR.Instruction.MoveInstruction, result, new RegisterOperand(result.Type, GeneralPurposeRegister.EAX));
        }

        #endregion // Methods
    }
}
