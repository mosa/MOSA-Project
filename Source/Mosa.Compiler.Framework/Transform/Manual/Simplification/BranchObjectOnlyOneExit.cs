// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Simplification
{
	public sealed class BranchObjectOnlyOneExit : BaseTransformation
	{
		public BranchObjectOnlyOneExit() : base(IRInstruction.BranchObject)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (context.Block.NextBlocks.Count != 1)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(IRInstruction.Nop);

			//context.SetInstruction(IRInstruction.Branch64, context.ConditionCode.GetReverse(), context.Result, context.Operand2, context.Operand1, context.BranchTargets[0]);
		}
	}
}
