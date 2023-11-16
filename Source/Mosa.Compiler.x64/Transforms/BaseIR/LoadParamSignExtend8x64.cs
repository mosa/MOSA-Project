// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// LoadParamSignExtend8x64
/// </summary>
[Transform]
public sealed class LoadParamSignExtend8x64 : BaseIRTransform
{
	public LoadParamSignExtend8x64() : base(IR.LoadParamSignExtend8x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X64.MovsxLoad8, context.Result, transform.StackFrame, context.Operand1);
	}
}
