// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

/// <summary>
/// MulR8
/// </summary>
[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class MulR8 : BaseTransform
{
	public MulR8() : base(IR.MulR8, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, Transform transform)
	{
		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2;

		var e1 = Operand.CreateConstant(MulR8(ToR8(t1), ToR8(t2)));

		context.SetInstruction(IR.MoveR8, result, e1);
	}
}
