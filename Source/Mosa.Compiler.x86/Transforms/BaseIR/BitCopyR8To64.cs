// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// BitCopyR8To64
/// </summary>
public sealed class BitCopyR8To64 : BaseIRTransform
{
	public BitCopyR8To64() : base(IR.BitCopyR8To64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var operand1 = context.Operand1;

		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);

		context.SetInstruction(X86.Movdssi32, resultLow, operand1); // FIXME
		context.AppendInstruction(X86.Pextrd32, resultHigh, operand1, Operand.Constant32_1);
	}
}
