// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Expand;

public sealed class Switch : BaseTransform
{
	public Switch() : base(IR.Switch, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var targets = context.BranchTargets;
		var blocks = transform.CreateNewBlockContexts(targets.Count, context.Label);
		var operand1 = context.Operand1;

		var next = transform.Split(context);

		context.SetInstruction(IR.Jmp, blocks[0].Block);

		for (int index = 0; index < targets.Count; index++)
		{
			blocks[index].AppendInstruction(IR.Branch32, ConditionCode.Equal, null, operand1, Operand.CreateConstant(index), targets[index]);

			if (index + 1 < targets.Count)
			{
				blocks[index].AppendInstruction(IR.Jmp, blocks[index + 1].Block);
			}
			else
			{
				blocks[index].AppendInstruction(IR.Jmp, next.Block);
			}
		}
	}
}
