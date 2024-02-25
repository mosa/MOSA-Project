// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::In8")]
	private static void In8(Context context, Transform transform)
	{
		var v1 = transform.VirtualRegisters.Allocate32();

		var result = context.Result;

		context.SetInstruction(X64.In8, v1, context.Operand1);
		context.AppendInstruction(X64.Movzx8To64, result, v1);

		//context.SetInstruction(X64.In8, result, context.Operand1);
	}
}
