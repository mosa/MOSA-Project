// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction;

/// <summary>
/// Sub32Same
/// </summary>
[Transform("IR.Optimizations.Auto.StrengthReduction")]
public sealed class Sub32Same : BaseTransform
{
	public Sub32Same() : base(IRInstruction.Sub32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 80;

	public override bool Match(Context context, TransformContext transform)
	{
		if (!AreSame(context.Operand1, context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var e1 = Operand.CreateConstant(To32(0));

		context.SetInstruction(IRInstruction.Move32, result, e1);
	}
}
