// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

/// <summary>
/// Compare32x32LessThanMax
/// </summary>
[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class Compare32x32LessThanMax : BaseTransform
{
	public Compare32x32LessThanMax() : base(IRInstruction.Compare32x32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, TransformContext transform)
	{
		if (context.ConditionCode != ConditionCode.UnsignedGreaterOrEqual)
			return false;

		if (!context.Operand1.IsResolvedConstant)
			return false;

		if (context.Operand1.ConstantUnsigned64 != 0xFFFFFFFF)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var c1 = transform.CreateConstant(1);

		context.SetInstruction(IRInstruction.Move32, result, c1);
	}
}
