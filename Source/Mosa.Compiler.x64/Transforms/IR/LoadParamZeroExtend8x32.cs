// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// LoadParamZeroExtend8x32
/// </summary>
[Transform("x64.IR")]
public sealed class LoadParamZeroExtend8x32 : BaseIRTransform
{
	public LoadParamZeroExtend8x32() : base(IRInstruction.LoadParamZeroExtend8x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X64.MovzxLoad8, context.Result, transform.StackFrame, context.Operand1);
	}
}
