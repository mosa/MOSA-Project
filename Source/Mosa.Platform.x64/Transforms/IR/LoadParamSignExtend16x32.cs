// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// LoadParamSignExtend16x32
/// </summary>
[Transform("x64.IR")]
public sealed class LoadParamSignExtend16x32 : BaseIRTransform
{
	public LoadParamSignExtend16x32() : base(IRInstruction.LoadParamSignExtend16x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.SetInstruction(X64.MovsxLoad16, context.Result, transform.StackFrame, context.Operand1);
	}
}
