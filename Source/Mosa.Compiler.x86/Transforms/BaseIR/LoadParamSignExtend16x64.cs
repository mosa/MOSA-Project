// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// LoadParamSignExtend16x64
/// </summary>
public sealed class LoadParamSignExtend16x64 : BaseIRTransform
{
	public LoadParamSignExtend16x64() : base(IR.LoadParamSignExtend16x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var lowOffset, out _);

		context.SetInstruction(X86.MovsxLoad16, resultLow, transform.StackFrame, lowOffset);
		context.AppendInstruction(X86.Cdq32, resultHigh, resultLow);
	}
}
