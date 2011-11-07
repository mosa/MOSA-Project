/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System.Collections.Generic;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.TypeSystem;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations the x86 CPUID instruction.
	/// </summary>
	public sealed class CpuIdEbx : IIntrinsicMethod
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
			RegisterOperand eax = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EAX);
			RegisterOperand ecx = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.ECX);
			RegisterOperand reg = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EBX);
			context.SetInstruction(CPUx86.Instruction.MovInstruction, eax, operand);
			context.AppendInstruction(CPUx86.Instruction.XorInstruction, ecx, ecx);
			context.AppendInstruction(CPUx86.Instruction.CpuIdEbxInstruction);
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, result, reg);
		}

		#endregion // Methods

	}
}
