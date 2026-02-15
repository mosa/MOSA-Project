// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.ConstantFolding;

public sealed class Switch : BaseTransform
{
	public Switch() : base(IR.Switch, TransformType.Manual | TransformType.Optimization, 100)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsResolvedConstant)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var index = context.Operand1.ConstantSigned32;
		var count = context.BranchTargetsCount;

		// Guard: no branch targets -> nothing to fold
		if (count == 0)
		{
			context.SetNop();
			return;
		}

		// Copy targets for phi update after modifications
		var targets = new List<BasicBlock>(context.BranchTargets);

		// If the constant index is inside the valid target range, jump to that target.
		// Otherwise, fall through.
		if (index >= 0 && index < count)
		{
			var newtarget = context.BranchTargets[index];

			context.SetInstruction(IR.Jmp, newtarget);

			RemoveRemainingInstructionInBlock(context);
		}
		else
		{
			// fall thru
			context.SetNop();
		}

		Framework.Transform.UpdatePhiBlocks(targets);
	}
}
