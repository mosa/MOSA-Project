// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// BranchManagedPointer
/// </summary>
[Transform("x86.IR")]
public sealed class BranchManagedPointer : BaseIRTransform
{
	public BranchManagedPointer() : base(IRInstruction.BranchManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		TransformContext.MoveConstantRight(context);

		var target = context.BranchTargets[0];
		var condition = context.ConditionCode;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		context.SetInstruction(X86.Cmp32, null, operand1, operand2);
		context.AppendInstruction(X86.Branch, condition, target);
	}
}
