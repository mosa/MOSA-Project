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
	/// Representations the instruction to set the stack pointer
	/// </summary>
	public sealed class SetStackPointer : IIntrinsicMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the instrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		public void ReplaceIntrinsicCall(Context context)
		{
			context.SetInstruction(IR.Instruction.MoveInstruction, new RegisterOperand(new SigType(CilElementType.U4), GeneralPurposeRegister.ESP), context.Operand1);
			context.AppendInstruction(IR.Instruction.MoveInstruction, new RegisterOperand(new SigType(CilElementType.U4), GeneralPurposeRegister.EBP), context.Operand1);
		}

		#endregion // Methods
	}
}
