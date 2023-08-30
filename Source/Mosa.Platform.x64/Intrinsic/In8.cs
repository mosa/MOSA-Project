// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::In8")]
	private static void In8(Context context, TransformContext transformContext)
	{
		var v1 = transformContext.VirtualRegisters.Allocate32();

		var result = context.Result;

		context.SetInstruction(X64.In8, v1, context.Operand1);
		context.AppendInstruction(X64.Movzx8To64, result, v1);

		//context.SetInstruction(X64.In8, result, context.Operand1);
	}
}
