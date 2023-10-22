﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Propagate;

public sealed class Move32PropagateConstant : BaseTransform
{
	public Move32PropagateConstant() : base(IRInstruction.Move32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Result.IsDefinedOnce)
			return false;

		if (!context.Operand1.IsResolvedConstant)
			return false;

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
