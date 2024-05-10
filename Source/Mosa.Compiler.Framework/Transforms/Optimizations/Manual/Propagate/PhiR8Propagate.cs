﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Propagate;

public sealed class PhiR8Propagate : BaseTransform
{
	public PhiR8Propagate() : base(IR.PhiR8, TransformType.Manual | TransformType.Optimization, 40)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.OperandCount == 1)
			return true;

		if (context.Operand1.IsPhysicalRegister)
			return false;

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
