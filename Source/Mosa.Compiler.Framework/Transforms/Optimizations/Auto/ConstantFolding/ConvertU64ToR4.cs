// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

/// <summary>
/// ConvertU64ToR4
/// </summary>
[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class ConvertU64ToR4 : BaseTransform
{
	public ConvertU64ToR4() : base(IRInstruction.ConvertU64ToR4, TransformType.Auto | TransformType.Optimization)
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

		var e1 = Operand.CreateConstant(ToR4(To64(t1)));

		context.SetInstruction(IRInstruction.MoveR4, result, e1);
	}
}
