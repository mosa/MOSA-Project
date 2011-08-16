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
using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using CIL = Mosa.Runtime.CompilerFramework.CIL;
using IR = Mosa.Runtime.CompilerFramework.IR;

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
			SigType u4 = new SigType(Runtime.Metadata.CilElementType.U4);

			// Retrieve register context
			context.SetInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(u4, GeneralPurposeRegister.EAX), new MemoryOperand(u4, GeneralPurposeRegister.ESP, new IntPtr(28)));

			// Restore registers (Note: EAX and EDX are NOT restored!)
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(u4, GeneralPurposeRegister.EDX), new MemoryOperand(u4, GeneralPurposeRegister.EAX, new IntPtr(28)));
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(u4, GeneralPurposeRegister.EBX), new MemoryOperand(u4, GeneralPurposeRegister.EAX, new IntPtr(4)));
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(u4, GeneralPurposeRegister.EDI), new MemoryOperand(u4, GeneralPurposeRegister.EAX, new IntPtr(20)));
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(u4, GeneralPurposeRegister.ESI), new MemoryOperand(u4, GeneralPurposeRegister.EAX, new IntPtr(16)));
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(u4, GeneralPurposeRegister.ESP), new MemoryOperand(u4, GeneralPurposeRegister.EAX, new IntPtr(32)));
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(u4, GeneralPurposeRegister.EBP), new MemoryOperand(u4, GeneralPurposeRegister.EAX, new IntPtr(24)));

			// Jmp to EIP (stored in EDX)
			context.AppendInstruction(CPUx86.Instruction.JmpInstruction);
			context.SetOperand(0, new RegisterOperand(u4, GeneralPurposeRegister.EDX));
		}

		#endregion // Methods
	}
}
