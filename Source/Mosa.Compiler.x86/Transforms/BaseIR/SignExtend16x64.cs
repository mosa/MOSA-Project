// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// SignExtend16x64
/// </summary>
[Transform("x86.BaseIR")]
public sealed class SignExtend16x64 : BaseIRTransform
{
	public SignExtend16x64() : base(Framework.IR.SignExtend16x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);

		context.SetInstruction(X86.Movsx16To32, resultLow, context.Operand1);
		context.AppendInstruction(X86.Cdq32, resultHigh, resultLow);
	}
}
