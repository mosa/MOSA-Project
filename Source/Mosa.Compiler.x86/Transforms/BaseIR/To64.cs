// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// To64
/// </summary>
[Transform("x86.BaseIR")]
public sealed class To64 : BaseIRTransform
{
	public To64() : base(IR.To64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);

		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		context.SetInstruction(X86.Mov32, resultLow, operand1);
		context.AppendInstruction(X86.Mov32, resultHigh, operand2);
	}
}
