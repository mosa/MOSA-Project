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
		public void ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem, IList<RuntimeParameter> parameters)
		{			
			// Retrieve register context
			context.SetInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX), new MemoryOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.ESP, new IntPtr(28)));

			// Restore registers (Note: EAX and EDX are NOT restored!)
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EDX), new MemoryOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX, new IntPtr(28)));
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EBX), new MemoryOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX, new IntPtr(4)));
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EDI), new MemoryOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX, new IntPtr(20)));
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.ESI), new MemoryOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX, new IntPtr(16)));
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.ESP), new MemoryOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX, new IntPtr(32)));
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EBP), new MemoryOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX, new IntPtr(24)));

			// Jmp to EIP (stored in EDX)
			context.AppendInstruction(CPUx86.Instruction.JmpInstruction);
			context.SetOperand(0, new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EDX));
		}

		#endregion // Methods
	}
}
