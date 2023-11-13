// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Propagate;

public sealed class PhiR4Propagate : BaseTransform
{
	public PhiR4Propagate() : base(Framework.IR.PhiR4, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.OperandCount == 1)
			return true;

		var operand = context.Operand1;

		foreach (var op in context.Operands)
		{
			if (!AreSame(op, operand))
				return false;
		}

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		foreach (var use in result.Uses.ToArray())
		{
			use.ReplaceOperand(result, operand1);
		}

		context.SetNop();
	}
}
