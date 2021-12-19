// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Rewrite
{
	public sealed class Branch64LessOrEqualThanZero : BaseTransformation
	{
		public Branch64LessOrEqualThanZero() : base(IRInstruction.Branch64)
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

			var phiBlock = GetOtherBranchTarget(context.Block, target);

			context.SetInstruction(IRInstruction.Jmp, target);

			RemoveRestOfInstructions(context);

			transformContext.UpdatePHIBlock(phiBlock);
		}
	}
}
