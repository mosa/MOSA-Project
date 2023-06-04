// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Runtime.Intrinsic::Load64")]
	private static void Load64(Context context, TransformContext transformContext)
	{
		var instruction = IRInstruction.Load64;

		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.OperandCount == 2 ? context.Operand2 : transformContext.ConstantZero;

		LoadStore.Set(context, transformContext, instruction, result, operand1, operand2);
	}
}
