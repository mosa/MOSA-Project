// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

/// <summary>
/// Compare32x64GreaterOrEqualThanZero
/// </summary>
[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class Compare32x64GreaterOrEqualThanZero : BaseTransform
{
	public Compare32x64GreaterOrEqualThanZero() : base(Framework.IR.Compare32x64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.UnsignedGreaterOrEqual)
			return false;

		if (!context.Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.ConstantUnsigned64 != 0)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var c1 = Operand.CreateConstant(1);

		context.SetInstruction(Framework.IR.Move64, result, c1);
	}
}
