// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transform;

namespace Mosa.Platform.x86.Transform.Manual.Special
{
	public sealed class Mov32Propagate : BaseTransformation
	{
		public Mov32Propagate() : base(X86.Mov32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (!context.Result.IsVirtualRegister)
				return false;

			if (!IsSSAForm(context.Result))
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

			context.Empty();
		}
	}
}
