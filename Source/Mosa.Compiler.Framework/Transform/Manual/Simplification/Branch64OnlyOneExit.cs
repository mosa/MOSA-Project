// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Simplification
{
	public sealed class Branch64OnlyOneExit : BaseTransformation
	{
		public Branch64OnlyOneExit() : base(IRInstruction.Branch64)
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
			var target = context.BranchTargets[0];
			var block = context.Block;

			context.SetInstruction(IRInstruction.Nop);

			TransformContext.RemoveBlockFromPHIInstructions(block, target);
		}
	}
}
