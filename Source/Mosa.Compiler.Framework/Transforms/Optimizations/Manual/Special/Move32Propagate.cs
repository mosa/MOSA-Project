﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Special
{
	public sealed class Move32Propagate : BaseTransformation
	{
		public Move32Propagate() : base(IRInstruction.Move32, TransformationType.Manual | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!IsSSAForm(context.Result))
				return false;

			if (!IsSSAForm(context.Operand1))
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

			context.SetNop();
		}
	}
}
