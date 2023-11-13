// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// BitCopy64ToR8
/// </summary>
[Transform("x86.BaseIR")]
public sealed class BitCopy64ToR8 : BaseIRTransform
{
	public BitCopy64ToR8() : base(Framework.IR.BitCopy64ToR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		transform.SplitOperand(context.Operand1, out var op1L, out var op1H);

		context.SetInstruction(X86.Movdssi32, result, op1L);    // FIXME
		context.AppendInstruction(X86.Pextrd32, result, op1H, Operand.Constant32_1);
	}
}
