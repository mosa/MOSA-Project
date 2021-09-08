﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Special
{
	public sealed class Phi32Propagate : BaseTransformation
	{
		public Phi32Propagate() : base(IRInstruction.Phi32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
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

		public override void Transform(Context context, TransformContext transformContext)
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
