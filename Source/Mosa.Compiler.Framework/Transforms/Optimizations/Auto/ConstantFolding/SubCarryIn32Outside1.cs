// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

/// <summary>
/// SubCarryIn32Outside1
/// </summary>
public sealed class SubCarryIn32Outside1 : BaseTransform
{
	public SubCarryIn32Outside1() : base(IRInstruction.SubCarryIn32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand3))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand3;

		var e1 = transform.CreateConstant(Sub32(To32(t1), BoolTo32(To32(t2))));

		context.SetInstruction(IRInstruction.Sub32, result, e1);
	}
}
