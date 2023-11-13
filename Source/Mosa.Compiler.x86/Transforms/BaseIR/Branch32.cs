// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// Branch32
/// </summary>
[Transform("x86.BaseIR")]
public sealed class Branch32 : BaseIRTransform
{
	public Branch32() : base(IR.Branch32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Framework.Transform.MoveConstantRight(context);

		var target = context.BranchTargets[0];
		var condition = context.ConditionCode;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		context.SetInstruction(X86.Cmp32, null, operand1, operand2);
		context.AppendInstruction(X86.Branch, condition, target);
	}
}
