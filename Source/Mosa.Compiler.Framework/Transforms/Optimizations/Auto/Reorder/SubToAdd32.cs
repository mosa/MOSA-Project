// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Reorder;

/// <summary>
/// SubToAdd32
/// </summary>
[Transform("IR.Optimizations.Auto.Reorder")]
public sealed class SubToAdd32 : BaseTransform
{
	public SubToAdd32() : base(IRInstruction.Sub32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!IsResolvedConstant(context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2;

		var e1 = Operand.CreateConstant(Neg32(ToSigned32(t2)));

		context.SetInstruction(IRInstruction.Add32, result, t1, e1);
	}
}
