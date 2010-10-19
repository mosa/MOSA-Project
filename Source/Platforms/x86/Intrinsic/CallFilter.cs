/*
 * (c) 2010 MOSA - The Managed Operating System Alliance
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
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Vm;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using CIL = Mosa.Runtime.CompilerFramework.CIL;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86.Intrinsic
{
	public sealed class CallFilter : IIntrinsicMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the instrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		public void ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem)
		{
			SigType u4 = new SigType(Runtime.Metadata.CilElementType.U4);

			RegisterOperand ebp = new RegisterOperand(u4, GeneralPurposeRegister.EBP);
			RegisterOperand esp = new RegisterOperand(u4, GeneralPurposeRegister.ESP);
			RegisterOperand eax = new RegisterOperand(u4, GeneralPurposeRegister.EAX);
			RegisterOperand ebx = new RegisterOperand(u4, GeneralPurposeRegister.EBX);
			RegisterOperand ecx = new RegisterOperand(u4, GeneralPurposeRegister.ECX);
			RegisterOperand esi = new RegisterOperand(u4, GeneralPurposeRegister.ESI);
			RegisterOperand edi = new RegisterOperand(u4, GeneralPurposeRegister.EDI);

			context.SetInstruction(CPUx86.Instruction.PushInstruction, null, ebp);
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, ebp, esp);
			context.AppendInstruction(CPUx86.Instruction.PushInstruction, null, ebx);
			context.AppendInstruction(CPUx86.Instruction.PushInstruction, null, esi);
			context.AppendInstruction(CPUx86.Instruction.PushInstruction, null, edi);

			context.AppendInstruction(CPUx86.Instruction.MovInstruction, eax, new MemoryOperand(u4, GeneralPurposeRegister.ESP, new IntPtr(28)));
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, ecx, new MemoryOperand(u4, GeneralPurposeRegister.ESP, new IntPtr(32)));
			context.AppendInstruction(CPUx86.Instruction.PushInstruction, null, ebp);

			context.AppendInstruction(CPUx86.Instruction.MovInstruction, ebp, new MemoryOperand(u4, GeneralPurposeRegister.EAX, new IntPtr(24)));
		}

		#endregion // Methods
	}
}
