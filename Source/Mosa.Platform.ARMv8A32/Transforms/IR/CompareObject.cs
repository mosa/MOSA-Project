// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// CompareObject
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class CompareObject : BaseIRTransform
{
	public CompareObject() : base(IRInstruction.CompareObject, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		MoveConstantRightForComparison(context);

		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var condition = context.ConditionCode;

		operand1 = MoveConstantToRegister(transform, context, operand1);
		operand2 = MoveConstantToRegisterOrImmediate(transform, context, operand2);

		context.SetInstruction(ARMv8A32.Cmp, condition, null, operand1, operand2);
		context.AppendInstruction(ARMv8A32.Mov, condition, result, Operand.Constant32_1);
		context.AppendInstruction(ARMv8A32.Mov, condition.GetOpposite(), result, Operand.Constant32_0);
	}
}
