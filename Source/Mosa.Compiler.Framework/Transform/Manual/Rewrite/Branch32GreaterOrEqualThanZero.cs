// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Rewrite
{
	public sealed class Branch32GreaterOrEqualThanZero : BaseTransformation
	{
		public Branch32GreaterOrEqualThanZero() : base(IRInstruction.Branch32, TransformationType.Manual | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (context.ConditionCode != ConditionCode.UnsignedGreaterOrEqual)
				return false;

			if (!IsZero(context.Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var target = context.BranchTargets[0];

			var phiBlock = GetOtherBranchTarget(context.Block, target);

			context.SetInstruction(IRInstruction.Jmp, target);

			RemoveRestOfInstructions(context);

			TransformContext.UpdatePhiBlock(phiBlock);
		}
	}
}
