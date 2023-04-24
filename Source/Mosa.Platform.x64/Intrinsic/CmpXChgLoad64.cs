// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::CmpXChgLoad64")]
	private static void CmpXChgLoad64(Context context, MethodCompiler methodCompiler)
	{
		var location = context.Operand1;
		var value = context.Operand2;
		var comparand = context.Operand3;
		var result = context.Result;

		var rax = Operand.CreateCPURegister64(CPURegister.RAX);
		var v1 = methodCompiler.VirtualRegisters.Allocate64();

		context.SetInstruction(X64.Mov64, rax, comparand);
		context.AppendInstruction(X64.Mov64, v1, value);
		context.AppendInstruction(X64.Lock);
		context.AppendInstruction(X64.CmpXChgLoad64, rax, rax, location, Operand.Constant32_0, v1);
		context.AppendInstruction(X64.Mov64, result, rax);
	}
}
