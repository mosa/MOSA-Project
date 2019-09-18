// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.IR.Special
{
	public sealed class Move32PropagateConstant : BaseTransformation
	{
		public Move32PropagateConstant() : base(IRInstruction.Move32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!IsSSAForm(context.Result))
				return false;

			if (!context.Operand1.IsResolvedConstant)
				return false;

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

			context.SetInstruction(IRInstruction.Nop);
		}
	}
}
