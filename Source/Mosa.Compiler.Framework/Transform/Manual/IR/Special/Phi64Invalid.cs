// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Manual.IR.Special
{
	public sealed class Phi64Invalid : BaseTransformation
	{
		public Phi64Invalid() : base(IRInstruction.Phi64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (context.ResultCount == 0 || context.ResultCount > 2)
				return false;

			var result = context.Result;

			if (!IsSSAForm(result))
				return false;

			foreach (var operands in context.Operands)
			{
				if (operands == result)
					return true;
			}

			return false;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(IRInstruction.Nop);
		}
	}
}
