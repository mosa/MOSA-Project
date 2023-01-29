// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.Optimizations.Manual.Special;

public sealed class Mov32Coalescing : BaseTransform
{
	public Mov32Coalescing() : base(X64.Mov32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Result.IsVirtualRegister)
			return false;

		if (!IsSSAForm(context.Result))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		foreach (var use in result.Uses.ToArray())
		{
			for (int i = 0; i < use.OperandCount; i++)
			{
				var operand = use.GetOperand(i);

				if (operand == result)
				{
					use.SetOperand(i, operand1);
				}
			}
		}

		context.Empty();
	}
}
