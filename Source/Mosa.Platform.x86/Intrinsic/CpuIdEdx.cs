// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.Intel;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	internal static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::CpuIdEdx")]
		private static void CpuIdEdx(Context context, MethodCompiler methodCompiler)
		{
			var result = context.Result;
			var operand = context.Operand1;
			var eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);
			var ecx = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ECX);
			var reg = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EDX);

			context.SetInstruction(X86.Mov32, eax, operand);
			context.AppendInstruction(X86.Mov32, ecx, methodCompiler.ConstantZero32);
			context.AppendInstruction(X86.CpuId, eax, eax);
			context.AppendInstruction(X86.Mov32, result, reg);
		}
	}
}
