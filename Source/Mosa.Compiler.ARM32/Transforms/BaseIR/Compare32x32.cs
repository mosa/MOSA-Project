// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// Compare32x32
/// </summary>
public sealed class Compare32x32 : BaseIRTransform
{
	public Compare32x32() : base(IR.Compare32x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		MoveConstantRightForComparison(context);

		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var condition = context.ConditionCode;

		operand1 = MoveConstantToRegister(transform, context, operand1);
		operand2 = MoveConstantToRegisterOrImmediate(transform, context, operand2);

		context.SetInstruction(ARM32.Cmp, condition, null, operand1, operand2);
		context.AppendInstruction(ARM32.Mov, condition, result, Operand.Constant32_1);
		context.AppendInstruction(ARM32.Mov, condition.GetOpposite(), result, Operand.Constant32_0);
	}
}
