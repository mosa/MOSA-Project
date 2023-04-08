// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// SignExtend32x64
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class SignExtend32x64 : BaseIRTransform
{
	public SignExtend32x64() : base(IRInstruction.SignExtend32x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);

		var op1 = MoveConstantToRegisterOrImmediate(transform, context, context.Operand1);

		context.SetInstruction(ARMv8A32.Mov, resultLow, op1);
		context.AppendInstruction(ARMv8A32.Asr, resultHigh, resultLow, transform.Constant32_31);
	}
}
