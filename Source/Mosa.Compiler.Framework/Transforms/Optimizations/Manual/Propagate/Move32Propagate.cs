// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Propagate;

public sealed class Move32Propagate : BaseTransform
{
	public Move32Propagate() : base(IRInstruction.Move32, TransformType.Manual | TransformType.Optimization)
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
			for (var i = 0; i < use.OperandCount; i++)
			{
				var operand = use.GetOperand(i);

				if (operand == result)
				{
					use.SetOperand(i, operand1);
				}
			}
		}

		context.SetNop();
	}
}
