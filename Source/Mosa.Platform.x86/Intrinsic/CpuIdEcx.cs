/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations the x86 CPUID instruction.
	/// </summary>
	public sealed class CpuIdEcx : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			Operand result = context.Result;
			Operand operand = context.Operand1;
			Operand eax = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EAX);
			Operand ecx = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.ECX);
			Operand reg = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.ECX);
			context.SetInstruction(X86.Mov, eax, operand);
			context.AppendInstruction(X86.Mov, ecx, Operand.CreateConstant((uint)0));
			context.AppendInstruction(X86.CpuId, eax, eax);
			context.AppendInstruction(X86.Mov, result, reg);
		}

		#endregion Methods
	}
}