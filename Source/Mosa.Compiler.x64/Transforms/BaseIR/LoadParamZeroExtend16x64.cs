// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// LoadParamZeroExtend16x64
/// </summary>
[Transform("x64.BaseIR")]
public sealed class LoadParamZeroExtend16x64 : BaseIRTransform
{
	public LoadParamZeroExtend16x64() : base(IR.LoadParamZeroExtend16x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X64.MovzxLoad16, context.Result, transform.StackFrame, context.Operand1);
	}
}
