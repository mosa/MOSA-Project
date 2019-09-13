// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.IR.Special
{
	public sealed class MoveR8Propagate : BaseTransformation
	{
		public MoveR8Propagate() : base(IRInstruction.MoveR8)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (context.Result.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			return context.Operand1.IsResolvedConstant || context.Operand1.IsVirtualRegister;
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
