// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// SignExtend8x64
/// </summary>
[Transform]
public sealed class SignExtend8x64 : BaseIRTransform
{
	public SignExtend8x64() : base(IR.SignExtend8x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);

		var v1 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(X86.Movsx8To32, resultLow, context.Operand1);
		context.AppendInstruction(X86.Cdq32, resultHigh, resultLow);
	}
}
