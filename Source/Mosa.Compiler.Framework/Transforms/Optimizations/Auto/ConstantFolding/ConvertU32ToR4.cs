// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

/// <summary>
/// ConvertU32ToR4
/// </summary>
[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class ConvertU32ToR4 : BaseTransform
{
	public ConvertU32ToR4() : base(IRInstruction.ConvertI32ToR4, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, Transform transform)
	{
		if (!IsResolvedConstant(context.Operand1))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;

		var e1 = Operand.CreateConstant(ToR4(To32(t1)));

		context.SetInstruction(IRInstruction.MoveR4, result, e1);
	}
}
