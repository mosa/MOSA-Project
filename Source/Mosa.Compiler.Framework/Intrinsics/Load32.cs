// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Runtime.Intrinsic::Load32")]
	private static void Load32(Context context, Transform transform)
	{
		var instruction = transform.Is32BitPlatform ? IRInstruction.Load32 : IRInstruction.LoadZeroExtend32x64;

		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.OperandCount == 2 ? context.Operand2 : transform.ConstantZero;

		LoadStore.Set(context, transform, instruction, result, operand1, operand2);
	}
}
