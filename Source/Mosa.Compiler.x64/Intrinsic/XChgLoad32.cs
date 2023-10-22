// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::XChgLoad32")]
	private static void XChgLoad32(Context context, Transform transform)
	{
		var location = context.Operand1;
		var value = context.Operand2;
		var result = context.Result;

		var v1 = transform.VirtualRegisters.Allocate64();

		context.SetInstruction(X64.Mov32, v1, value);
		context.AppendInstruction(X64.Lock);
		context.AppendInstruction(X64.XChgLoad32, v1, location, Operand.Constant32_0, v1);
		context.AppendInstruction(X64.Mov32, result, v1);
	}
}
