// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Rewrite
{
	public sealed class Branch32LessOrEqualThanZero : BaseTransformation
	{
		public Branch32LessOrEqualThanZero() : base(IRInstruction.Branch32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (context.ConditionCode != ConditionCode.UnsignedLessOrEqual)
				return false;

			if (!IsZero(context.Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var target = context.BranchTargets[0];

			context.SetInstruction(IRInstruction.Jmp, target);

			RemoveRestOfInstructions(context);

			TransformContext.RemoveBlockFromPHIInstructions(context.Block, target);
		}
	}
}
