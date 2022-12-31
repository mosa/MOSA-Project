// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Simplification
{
	public sealed class BranchObjectOnlyOneExit : BaseTransform
	{
		public BranchObjectOnlyOneExit() : base(IRInstruction.BranchObject, TransformType.Manual | TransformType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (context.Block.NextBlocks.Count != 1)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetNop();

			//context.SetInstruction(IRInstruction.Branch64, context.ConditionCode.GetReverse(), context.Result, context.Operand2, context.Operand1, context.BranchTargets[0]);
		}
	}
}
