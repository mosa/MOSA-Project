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
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class CallFilter : IIntrinsicMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		public void ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem, IList<RuntimeParameter> parameters)
		{
			RegisterOperand ebp = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EBP);
			RegisterOperand esp = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.ESP);
			RegisterOperand eax = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX);
			RegisterOperand ebx = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EBX);
			RegisterOperand ecx = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.ECX);
			RegisterOperand edx = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EDX);
			RegisterOperand esi = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.ESI);
			RegisterOperand edi = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EDI);

			context.SetInstruction(CPUx86.Instruction.PushInstruction, null, ebp);
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, ebp, esp);
			context.AppendInstruction(CPUx86.Instruction.PushInstruction, null, ebx);
			context.AppendInstruction(CPUx86.Instruction.PushInstruction, null, esi);
			context.AppendInstruction(CPUx86.Instruction.PushInstruction, null, edi);

			// Load register context
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, eax, new MemoryOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.ESP, new IntPtr(28)));
			// Load exception handler
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, ecx, new MemoryOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.ESP, new IntPtr(32)));
			// Save EBP
			context.AppendInstruction(CPUx86.Instruction.PushInstruction, null, ebp);

			// Restore register values
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, ebp, new MemoryOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX, new IntPtr(24)));
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, ebx, new MemoryOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX, new IntPtr(4)));
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, esi, new MemoryOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX, new IntPtr(16)));
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, edi, new MemoryOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX, new IntPtr(20)));

			// Align ESP
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, edx, esp);
			context.AppendInstruction(CPUx86.Instruction.AndInstruction, esp, new ConstantOperand(BuiltInSigType.UInt32, 0xFFFFFFF0u));
			context.AppendInstruction(CPUx86.Instruction.SubInstruction, esp, new ConstantOperand(BuiltInSigType.UInt32, 0x8u));

			// Save original ESP
			context.AppendInstruction(CPUx86.Instruction.PushInstruction, null, edx);

			// Call catch handler
			context.AppendInstruction(CPUx86.Instruction.CallInstruction, ecx);

			// Restore registers
			context.AppendInstruction(CPUx86.Instruction.PopInstruction, esp);
			context.AppendInstruction(CPUx86.Instruction.PopInstruction, ebp);
			context.AppendInstruction(CPUx86.Instruction.PopInstruction, esi);
			context.AppendInstruction(CPUx86.Instruction.PopInstruction, edi);
			context.AppendInstruction(CPUx86.Instruction.PopInstruction, ebx);
			context.AppendInstruction(CPUx86.Instruction.LeaveInstruction);
			context.AppendInstruction(CPUx86.Instruction.RetInstruction);
		}

		#endregion // Methods
	}
}
