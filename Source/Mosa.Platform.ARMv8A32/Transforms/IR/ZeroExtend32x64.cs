// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// ZeroExtend32x64
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class ZeroExtend32x64 : BaseIRTransform
{
	public ZeroExtend32x64() : base(IRInstruction.ZeroExtend32x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var op1L, out _);

		op1L = MoveConstantToRegisterOrImmediate(transform, context, op1L);

		context.SetInstruction(ARMv8A32.Mov, resultLow, op1L);
		context.AppendInstruction(ARMv8A32.Mov, resultHigh, Operand.Constant32_0);
	}
}
