﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Simplification
{
	public sealed class Branch64OnlyOneExit : BaseTransformation
	{
		public Branch64OnlyOneExit() : base(IRInstruction.Branch64, TransformationType.Manual | TransformationType.Optimization)
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
			var target = context.BranchTargets[0];

			context.SetNop();

			TransformContext.UpdatePhiBlock(target);
		}
	}
}
