// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Runtime.Intrinsic::Load8")]
	private static void Load8(Context context, TransformContext transformContext)
	{
		var instruction = transformContext.Is32BitPlatform ? IRInstruction.LoadZeroExtend8x32 : IRInstruction.LoadZeroExtend8x64;

		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.OperandCount == 2 ? context.Operand2 : transformContext.ConstantZero;

		LoadStore.Set(context, transformContext, instruction, result, operand1, operand2);
	}
}
