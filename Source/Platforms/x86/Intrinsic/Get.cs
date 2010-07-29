/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;
using CIL = Mosa.Runtime.CompilerFramework.CIL;
using IR = Mosa.Runtime.CompilerFramework.IR;
using System.Diagnostics;

namespace Mosa.Platforms.x86.Intrinsic
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class Get : IIntrinsicMethod
	{

		#region Methods

		/// <summary>
		/// Replaces the instrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		public void ReplaceIntrinsicCall(Context context, RuntimeBase runtime)
		{
			Operand result = context.Result;

			RegisterOperand tmp = new RegisterOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.Ptr), GeneralPurposeRegister.EDX);
			MemoryOperand operand = new MemoryOperand(context.Operand1.Type, GeneralPurposeRegister.EDX, new System.IntPtr(0));

			context.SetInstruction(CPUx86.Instruction.MovInstruction, tmp, context.Operand1);
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, result, operand);
		}

		#endregion // Methods

	}
}
