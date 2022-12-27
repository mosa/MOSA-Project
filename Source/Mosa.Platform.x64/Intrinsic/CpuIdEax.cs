// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.Intel;

namespace Mosa.Platform.x64.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	internal static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::CpuIdEax")]
		private static void CpuIdEax(Context context, MethodCompiler methodCompiler)
		{
			var result = context.Result;
			var operand = context.Operand1;

			var rax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, CPURegister.RAX);
			var rcx = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, CPURegister.RCX);

			context.SetInstruction(X64.Mov64, rax, operand);
			context.AppendInstruction(X64.Mov64, rcx, methodCompiler.ConstantZero32);
			context.AppendInstruction(X64.CpuId, rax, rax, rcx);
			context.AppendInstruction(X64.Mov64, result, rax);
		}
	}
}
