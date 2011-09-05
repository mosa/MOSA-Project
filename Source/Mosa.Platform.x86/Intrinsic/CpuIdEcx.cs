/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System.Collections.Generic;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.TypeSystem;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations the x86 CPUID instruction.
	/// </summary>
	public sealed class CpuIdEcx : IIntrinsicMethod
	{

		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		public void ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem, IList<RuntimeParameter> parameters)
		{
			Operand result = context.Result;
			Operand operand = context.Operand1;
			RegisterOperand eax = new RegisterOperand(new SigType(Mosa.Runtime.Metadata.CilElementType.I4), GeneralPurposeRegister.EAX);
			RegisterOperand ecx = new RegisterOperand(new SigType(Mosa.Runtime.Metadata.CilElementType.I4), GeneralPurposeRegister.ECX);
			RegisterOperand reg = new RegisterOperand(new SigType(Mosa.Runtime.Metadata.CilElementType.I4), GeneralPurposeRegister.ECX);
			context.SetInstruction(CPUx86.Instruction.MovInstruction, eax, operand);
			context.AppendInstruction(CPUx86.Instruction.XorInstruction, ecx, ecx);
			context.AppendInstruction(CPUx86.Instruction.CpuIdEcxInstruction);
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, result, reg);
		}

		#endregion // Methods

	}
}
