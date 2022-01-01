// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Transform.Manual.ConstantFolding
{
	public sealed class Switch : BaseTransformation
	{
		public Switch() : base(IRInstruction.Switch)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsResolvedConstant)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var index = context.Operand1.ConstantSigned32;
			var max = context.BranchTargets.Count - 1;

			var targets = new List<BasicBlock>(context.BranchTargets.Count);

			foreach (var target in context.BranchTargets)
			{
				targets.Add(target);
			}

			if (index < max)
			{
				var newtarget = context.BranchTargets[index];

				context.SetInstruction(IRInstruction.Jmp, newtarget);

				RemoveRestOfInstructions(context);
			}
			else
			{
				// fall thru
				context.SetNop();
			}

			TransformContext.UpdatePhiBlocks(targets);
		}
	}
}
