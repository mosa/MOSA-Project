// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Transform.Manual.Special
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
			var max = context.OperandCount;

			var targets = new List<BasicBlock>(max - 1);

			foreach (var target in context.BranchTargets)
			{
				targets.Add(target);
			}

			if (index >= max)
			{
				// fall thru
				context.SetNop();
			}
			else
			{
				var newtarget = context.BranchTargets[index];

				context.SetInstruction(IRInstruction.Jmp, newtarget);

				RemoveRestOfInstructions(context);
			}

			transformContext.UpdatePhiBlocks(targets);
		}
	}
}
