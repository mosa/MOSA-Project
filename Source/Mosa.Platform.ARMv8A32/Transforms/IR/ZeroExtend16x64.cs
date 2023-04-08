// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// ZeroExtend16x64
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class ZeroExtend16x64 : BaseIRTransform
{
	public ZeroExtend16x64() : base(IRInstruction.ZeroExtend16x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitLongOperand(context.Operand1, out var op1L, out _);

		op1L = MoveConstantToRegisterOrImmediate(transform, context, op1L);

		var operand1 = MoveConstantToRegister(transform, context, transform.CreateConstant32((uint)0xFFFF));

		context.SetInstruction(ARMv8A32.Mov, resultLow, op1L);
		context.AppendInstruction(ARMv8A32.And, resultLow, resultLow, operand1);
		context.AppendInstruction(ARMv8A32.Mov, resultHigh, transform.Constant32_0);
	}
}
