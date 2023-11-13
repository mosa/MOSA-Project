// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// SignExtend32x64
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class SignExtend32x64 : BaseIRTransform
{
	public SignExtend32x64() : base(IR.SignExtend32x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);

		var op1 = MoveConstantToRegisterOrImmediate(transform, context, context.Operand1);

		context.SetInstruction(ARM32.Mov, resultLow, op1);
		context.AppendInstruction(ARM32.Asr, resultHigh, resultLow, Operand.Constant32_31);
	}
}
