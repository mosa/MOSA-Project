// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// BitCopyR8To64
/// </summary>
[Transform("x86.IR")]
public sealed class BitCopyR8To64 : BaseIRTransform
{
	public BitCopyR8To64() : base(IRInstruction.BitCopyR8To64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var operand1 = context.Operand1;

		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);

		context.SetInstruction(X86.Movdssi32, resultLow, operand1); // FIXME
		context.AppendInstruction(X86.Pextrd32, resultHigh, operand1, Operand.Constant32_1);
	}
}
