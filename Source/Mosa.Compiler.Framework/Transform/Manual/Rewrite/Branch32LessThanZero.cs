﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Rewrite
{
	public sealed class Branch32LessThanZero : BaseTransformation
	{
		public Branch32LessThanZero() : base(IRInstruction.Branch32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (context.ConditionCode != ConditionCode.UnsignedLess)
				return false;

			if (!IsZero(context.Operand2))
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
