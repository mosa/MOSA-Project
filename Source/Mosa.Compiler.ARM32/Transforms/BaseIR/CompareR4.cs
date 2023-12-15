// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// CompareR4
/// </summary>
public sealed class CompareR4 : BaseIRTransform
{
	public CompareR4() : base(IR.CompareR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		MoveConstantRightForComparison(context);

		var result = context.Result;
		var condition = context.ConditionCode;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		operand1 = MoveConstantToFloatRegister(transform, context, operand1);
		operand2 = MoveConstantToFloatRegisterOrImmediate(transform, context, operand2);

		context.SetInstruction(ARM32.VCmp, null, operand1, operand2);
		context.AppendInstruction(ARM32.VMrs_APSR);
		context.AppendInstruction(ARM32.Mov, condition, result, Operand.Constant32_0);
		context.AppendInstruction(ARM32.Mov, condition.GetOpposite(), result, Operand.Constant32_1);
	}
}
