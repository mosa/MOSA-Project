// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction;

[Transform()]
public sealed class Sub32Same : BaseTransform
{
	public Sub32Same() : base(IR.Sub32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 80;

	public override bool Match(Context context, Transform transform)
	{
		if (!AreSame(context.Operand1, context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var e1 = Operand.Constant32_0;

		context.SetInstruction(IR.Move32, result, e1);
	}
}
