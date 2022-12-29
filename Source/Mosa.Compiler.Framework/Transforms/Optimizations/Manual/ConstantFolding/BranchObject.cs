// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.ConstantFolding
{
	public sealed class BranchObject : BaseTransformation
	{
		public BranchObject() : base(IRInstruction.BranchObject, TransformationType.Manual | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!IsResolvedConstant(context.Operand1))
				return false;

			if (!IsResolvedConstant(context.Operand2))
				return false;

			return IsNormal(context.ConditionCode);
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var target = context.BranchTargets[0];

			if (!Compare64(context))
			{
				context.SetNop();

				TransformContext.UpdatePhiBlock(target);
			}
			else
			{
				var phiBlock = GetOtherBranchTarget(context.Block, target);

				context.SetInstruction(IRInstruction.Jmp, target);

				RemoveRestOfInstructions(context);

				TransformContext.UpdatePhiBlock(phiBlock);
			}
		}
	}
}
