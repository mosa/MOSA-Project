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
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;

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
		void IIntrinsicMethod.ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem, IList<RuntimeParameter> parameters)
		{
			Operand result = context.Result;
			Operand operand = context.Operand1;
			RegisterOperand eax = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EAX);
			RegisterOperand ecx = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.ECX);
			RegisterOperand reg = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.ECX);
			context.SetInstruction(OpCodes.Instruction.MovInstruction, eax, operand);
			context.AppendInstruction(OpCodes.Instruction.XorInstruction, ecx, ecx);
			context.AppendInstruction(OpCodes.Instruction.CpuIdEcxInstruction);
			context.AppendInstruction(OpCodes.Instruction.MovInstruction, result, reg);
		}

		#endregion // Methods

	}
}
