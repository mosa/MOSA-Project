// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::XChgLoad32")]
	private static void XChgLoad32(Context context, TransformContext transformContext)
	{
		var location = context.Operand1;
		var value = context.Operand2;
		var result = context.Result;

		var v1 = transformContext.VirtualRegisters.Allocate32();

		context.SetInstruction(X86.Mov32, v1, value);
		context.AppendInstruction(X86.Lock);
		context.AppendInstruction(X86.XChgLoad32, v1, location, Operand.Constant32_0, v1);
		context.AppendInstruction(X86.Mov32, result, v1);
	}
}
