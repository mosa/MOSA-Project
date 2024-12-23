// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.BitValue;

/// <summary>
/// Compare32x32BitValue
/// </summary>
public sealed class Compare32x32BitValue : BaseTransform
{
	public Compare32x32BitValue() : base(IR.Compare32x32, TransformType.Manual | TransformType.Optimization, 35)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Result.IsDefinedOnce)
			return false;

		var value = EvaluateCompare(context.Operand1, context.Operand2, context.ConditionCode);

		return value.HasValue;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var value = EvaluateCompare(context.Operand1, context.Operand2, context.ConditionCode);

		var constant = Operand.CreateConstant32(value.Value ? 1 : 0);

		context.SetInstruction(IR.Move32, result, constant);
	}
}
