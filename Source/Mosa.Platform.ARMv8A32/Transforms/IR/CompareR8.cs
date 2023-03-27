// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// CompareR8
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class CompareR8 : BaseIRTransform
{
	public CompareR8() : base(IRInstruction.CompareR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		MoveConstantRightForComparison(context);

		var result = context.Result;
		var condition = context.ConditionCode;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		operand1 = MoveConstantToFloatRegister(transform, context, operand1);
		operand2 = MoveConstantToFloatRegisterOrImmediate(transform, context, operand2);

		context.SetInstruction(ARMv8A32.Cmf, null, operand1, operand2);
		context.AppendInstruction(ARMv8A32.Mov, condition, result, transform.Constant32_0);
		context.AppendInstruction(ARMv8A32.Mov, condition.GetOpposite(), result, transform.Constant32_1);
	}
}
