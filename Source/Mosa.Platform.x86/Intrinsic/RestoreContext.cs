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
	public sealed class RestoreContext : IIntrinsicMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicMethod.ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem, IList<RuntimeParameter> parameters)
		{
			
			// Retrieve register context
			//context.SetInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX), new MemoryOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.ESP, new IntPtr(28)));

			// Restore registers (Note: EAX and EDX are NOT restored!)
			//context.AppendInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EDX), new MemoryOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX, new IntPtr(28)));
			//context.AppendInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EBX), new MemoryOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX, new IntPtr(4)));
			//context.AppendInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EDI), new MemoryOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX, new IntPtr(20)));
			//context.AppendInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.ESI), new MemoryOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX, new IntPtr(16)));
			//context.AppendInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.ESP), new MemoryOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX, new IntPtr(32)));
			//context.AppendInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EBP), new MemoryOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX, new IntPtr(24)));

			//uint ebp, uint esp, int eip

			RegisterOperand edx = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EDX);
			RegisterOperand ebp = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EBP);
			RegisterOperand esp = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.ESP);

			// Restore registers
			context.AppendInstruction(X86.Mov, new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.ESP), context.Operand1);


			// Jmp to EIP (stored in EDX)
			context.AppendInstruction(X86.Jmp, null, edx);
			//context.SetOperand(0, edx);
		}

		#endregion // Methods
	}
}
