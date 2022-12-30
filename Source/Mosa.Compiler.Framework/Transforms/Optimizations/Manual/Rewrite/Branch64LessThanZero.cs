// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Rewrite
{
	public sealed class Branch64LessThanZero : BaseTransformation
	{
		public Branch64LessThanZero() : base(IRInstruction.Branch64, TransformationType.Manual | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (context.ConditionCode != ConditionCode.UnsignedLess)
				return false;

			if (!IsZero(context.Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var target = context.BranchTargets[0];

			context.SetNop();

			TransformContext.UpdatePhiBlock(target);
		}
	}
}
