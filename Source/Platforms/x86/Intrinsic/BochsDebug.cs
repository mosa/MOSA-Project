/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Platforms.x86.Intrinsic
{
	/// <summary>
	/// Representations the x86 BochsDebug instruction.
	/// </summary>
	public sealed class BochsDebug : IIntrinsicMethod
	{

		#region Methods

		/// <summary>
		/// Replaces the instrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		public void ReplaceIntrinsicCall(Context context)
		{
			// xchg	bx, bx 
			context.SetInstruction(CPUx86.Instruction.XchgInstruction, new RegisterOperand(new SigType(CilElementType.U2), GeneralPurposeRegister.EBX), new RegisterOperand(new SigType(CilElementType.U2), GeneralPurposeRegister.EBX));
		}

		#endregion // Methods

	}
}
