// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// LoadParamSignExtend8x32
/// </summary>
[Transform("x64.IR")]
public sealed class LoadParamSignExtend8x32 : BaseIRTransform
{
	public LoadParamSignExtend8x32() : base(IRInstruction.LoadParamSignExtend8x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X64.MovsxLoad8, context.Result, transform.StackFrame, context.Operand1);
	}
}
