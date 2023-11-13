// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// LoadParamZeroExtend8x64
/// </summary>
[Transform("x64.BaseIR")]
public sealed class LoadParamZeroExtend8x64 : BaseIRTransform
{
	public LoadParamZeroExtend8x64() : base(Framework.IR.LoadParamZeroExtend8x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X64.MovzxLoad8, context.Result, transform.StackFrame, context.Operand1);
	}
}
