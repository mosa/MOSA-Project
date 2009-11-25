/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using IR = Mosa.Runtime.CompilerFramework.IR;

using Mosa.Runtime.CompilerFramework;


namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Representations the x86 CPUID instruction.
    /// </summary>
    public sealed class CpuIdEbxInstruction : TwoOperandInstruction, IIntrinsicInstruction
    {

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="emitter"></param>
        public override void Emit(Context ctx, MachineCodeEmitter emitter)
        {
            emitter.Emit(new OpCode(new byte[] { 0x0F, 0xA2 }), null, null);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="context">The context.</param>
        public override void Visit(IX86Visitor visitor, Context context)
        {
            visitor.CpuIdEax(context);
        }

        /// <summary>
        /// Replaces the instrinsic call site
        /// </summary>
        /// <param name="context">The context.</param>
        public void ReplaceIntrinsicCall(Context context)
        {
            Operand result = context.Result;
            Operand operand = context.Operand1;
            RegisterOperand eax = new RegisterOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I4), GeneralPurposeRegister.EAX);
            RegisterOperand ecx = new RegisterOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I4), GeneralPurposeRegister.ECX);
            RegisterOperand reg = new RegisterOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I4), GeneralPurposeRegister.EBX);
            context.SetInstruction(CPUx86.Instruction.MovInstruction, eax, operand);
            context.AppendInstruction(CPUx86.Instruction.XorInstruction, ecx, ecx);
            context.AppendInstruction(CPUx86.Instruction.CpuIdEbxInstruction);
            context.AppendInstruction(CPUx86.Instruction.MovInstruction, result, reg);
        }

        #endregion // Methods
    }
}
