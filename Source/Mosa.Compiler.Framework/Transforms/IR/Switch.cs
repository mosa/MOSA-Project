﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.IR;

public sealed class Switch : BaseTransform
{
	public Switch() : base(IRInstruction.Switch, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var targets = context.BranchTargets;
		var blocks = transform.CreateNewBlockContexts(targets.Count, context.Label);
		var operand1 = context.Operand1;

		var next = transform.Split(context);

		context.SetInstruction(IRInstruction.Jmp, blocks[0].Block);

		for (int index = 0; index < targets.Count; index++)
		{
			blocks[index].AppendInstruction(IRInstruction.Branch32, ConditionCode.Equal, null, operand1, Operand.CreateConstant(index), targets[index]);

			if (index + 1 < targets.Count)
			{
				blocks[index].AppendInstruction(IRInstruction.Jmp, blocks[index + 1].Block);
			}
			else
			{
				blocks[index].AppendInstruction(IRInstruction.Jmp, next.Block);
			}
		}
	}
}
