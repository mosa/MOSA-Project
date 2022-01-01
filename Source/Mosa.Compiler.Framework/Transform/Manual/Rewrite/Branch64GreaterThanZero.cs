// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Rewrite
{
	public sealed class Branch64GreaterThanZero : BaseTransformation
	{
		public Branch64GreaterThanZero() : base(IRInstruction.Branch64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (context.ConditionCode != ConditionCode.UnsignedGreater)
				return false;

			if (!IsZero(context.Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var target = context.BranchTargets[0];

			context.SetNop();

			TransformContext.UpdatePhiBlock(target);
		}
	}
}
