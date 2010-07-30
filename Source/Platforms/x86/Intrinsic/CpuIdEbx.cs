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

using Mosa.Runtime;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Platforms.x86.Intrinsic
{
    /// <summary>
    /// Representations the x86 CPUID instruction.
    /// </summary>
    public sealed class CpuIdEbx : IIntrinsicMethod
    {
	
        #region Methods

		/// <summary>
		/// Replaces the instrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
        public void ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem)
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
