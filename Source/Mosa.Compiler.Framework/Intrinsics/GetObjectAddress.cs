// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Runtime.Intrinsic::GetObjectAddress")]
	[IntrinsicMethod("Mosa.Runtime.Intrinsic::GetValueTypeAddress")]
	private static void GetObjectAddress(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		if (operand1.IsValueType)
		{
			var def = operand1.Definitions[0];

			foreach (var use in operand1.Uses)
			{
				use.ReplaceOperand(operand1, def.Operand1);
			}

			operand1 = def.Operand1;
			def.Empty();
		}

		context.SetInstruction(transform.MoveInstruction, result, operand1);
	}
}
