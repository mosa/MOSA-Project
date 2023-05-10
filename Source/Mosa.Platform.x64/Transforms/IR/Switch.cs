// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// Switch
/// </summary>
[Transform("x64.IR")]
public sealed class Switch : BaseIRTransform
{
	public Switch() : base(IRInstruction.Switch, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var targets = context.BranchTargets;
		var operand = context.Operand1;

		context.Empty();

		for (int i = 0; i < targets.Count - 1; ++i)
		{
			context.AppendInstruction(X64.Cmp32, null, operand, Operand.CreateConstant32(i));
			context.AppendInstruction(X64.Branch, ConditionCode.Equal, targets[i]);
		}
	}
}
