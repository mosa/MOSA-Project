// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// MulCarryOut32
/// </summary>
public sealed class MulCarryOut32 : BaseIRTransform
{
	public MulCarryOut32() : base(IR.MulCarryOut32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result2 = context.Result2;

		Framework.Transform.MoveConstantRight(context);

		Translate(transform, context, ARM32.Mul, false, InstructionOption.SetFlags);

		context.AppendInstruction(ARM32.Mov, ConditionCode.Overflow, result2, Operand.Constant32_1);
		context.AppendInstruction(ARM32.Mov, ConditionCode.NoOverflow, result2, Operand.Constant32_0);
	}
}
