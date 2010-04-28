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
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86.Intrinsic
{
    /// <summary>
    /// Representations the x86 move cr0 instruction.
    /// </summary>
    public sealed class GetControlRegister :  IIntrinsicMethod
    {

        #region Methods

        /// <summary>
        /// Replaces the instrinsic call site
        /// </summary>
        /// <param name="context">The context.</param>
        public void ReplaceIntrinsicCall(Context context)
        {
            Context loadContext = new Context(context.InstructionSet, context.Operand1.Definitions[0]);
            ConstantOperand op1 = loadContext.Operand1 as ConstantOperand;

            if (op1 == null)
                throw new InvalidOperationException();
            
            Operand result = context.Result;

			ControlRegister control;

			switch ((int)op1.Value) {
				case 0: control = ControlRegister.CR0; break;
				case 2: control = ControlRegister.CR2; break;
				case 3: control = ControlRegister.CR3; break;
				case 4: control = ControlRegister.CR4; break;
				default: throw new InvalidOperationException();
			}

			RegisterOperand imm = new RegisterOperand(new SigType(CilElementType.U4), GeneralPurposeRegister.EAX);

			context.SetInstruction(IR.Instruction.MoveInstruction, imm, new RegisterOperand(new SigType(CilElementType.U4), control));
			context.AppendInstruction(IR.Instruction.MoveInstruction, result, imm);
		}

        #endregion // Methods

    }
}
