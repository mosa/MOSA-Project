﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Propagate;

public sealed class MoveManagedPointerPropagate : BaseTransform
{
	public MoveManagedPointerPropagate() : base(IRInstruction.MoveManagedPointer, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Result.IsDefinedOnce)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
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
