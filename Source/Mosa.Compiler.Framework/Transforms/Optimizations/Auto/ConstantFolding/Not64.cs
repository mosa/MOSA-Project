// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

/// <summary>
/// Not64
/// </summary>
[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class Not64 : BaseTransform
{
	public Not64() : base(IRInstruction.Not64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, TransformContext transform)
	{
		if (!IsResolvedConstant(context.Operand1))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;

		var e1 = Operand.CreateConstant(Not64(To64(t1)));

		context.SetInstruction(IRInstruction.Move64, result, e1);
	}
}
