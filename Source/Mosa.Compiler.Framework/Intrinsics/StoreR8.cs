// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Runtime.Intrinsic::StoreR8")]
	private static void StoreR8(Context context, Transform transform)
	{
		var instruction = IR.StoreR8;

		var operand1 = context.Operand1;
		var operand2 = context.OperandCount == 3 ? context.Operand2 : transform.ConstantZero;
		var operand3 = context.OperandCount == 3 ? context.Operand3 : context.Operand2;

		LoadStore.Set(context, transform, instruction, null, operand1, operand2, operand3);
	}
}
