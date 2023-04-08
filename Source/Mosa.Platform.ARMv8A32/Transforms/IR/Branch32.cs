// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// Branch32
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class Branch32 : BaseIRTransform
{
	public Branch32() : base(IRInstruction.Branch32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		MoveConstantRightForComparison(context);

		var target = context.BranchTargets[0];
		var condition = context.ConditionCode;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		operand1 = MoveConstantToRegister(transform, context, operand1);
		operand2 = MoveConstantToRegisterOrImmediate(transform, context, operand2);

		context.SetInstruction(ARMv8A32.Cmp, null, operand1, operand2);
		context.AppendInstruction(ARMv8A32.B, condition, target);
	}
}
