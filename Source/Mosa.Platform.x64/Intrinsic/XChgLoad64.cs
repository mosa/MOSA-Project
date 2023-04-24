// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::XChgLoad64")]
	private static void XChgLoad64(Context context, MethodCompiler methodCompiler)
	{
		var location = context.Operand1;
		var value = context.Operand2;
		var result = context.Result;

		var v1 = methodCompiler.VirtualRegisters.Allocate64();

		context.SetInstruction(X64.Mov64, v1, value);
		context.AppendInstruction(X64.Lock);
		context.AppendInstruction(X64.XChgLoad64, v1, location, Operand.Constant32_0, v1);
		context.AppendInstruction(X64.Mov64, result, v1);
	}
}
