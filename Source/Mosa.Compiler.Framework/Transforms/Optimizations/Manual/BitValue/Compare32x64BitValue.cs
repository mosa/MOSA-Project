// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.BitValue;

/// <summary>
/// Compare32x64BitValue
/// </summary>
[Transform("IR.Optimizations.Manual.BitVaule")]
public sealed class Compare32x64BitValue : BaseTransform
{
	public Compare32x64BitValue() : base(Framework.IR.Compare32x64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override int Priority => 35;

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

		var constant = Operand.CreateConstant64(value.Value ? 1 : 0);

		context.SetInstruction(Framework.IR.Move64, result, constant);
	}
}
