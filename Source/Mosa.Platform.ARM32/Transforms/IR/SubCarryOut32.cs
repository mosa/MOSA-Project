// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARM32.Transforms.IR;

/// <summary>
/// SubCarryOut32
/// </summary>
[Transform("ARM32.IR")]
public sealed class SubCarryOut32 : BaseIRTransform
{
	public SubCarryOut32() : base(IRInstruction.SubCarryOut32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var result2 = context.Result2;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		operand1 = MoveConstantToRegister(transform, context, operand1);
		operand2 = MoveConstantToRegisterOrImmediate(transform, context, operand2);

		context.SetInstruction(ARM32.Sub, StatusRegister.Set, result, operand1, operand2);
		context.AppendInstruction(ARM32.Mov, ConditionCode.Carry, result2, Operand.Constant32_1);
		context.AppendInstruction(ARM32.Mov, ConditionCode.NoCarry, result2, Operand.Constant32_0);
	}
}
