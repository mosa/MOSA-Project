// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// LoadParamSignExtend32x64
/// </summary>
[Transform("x64.IR")]
public sealed class LoadParamSignExtend32x64 : BaseIRTransform
{
	public LoadParamSignExtend32x64() : base(IRInstruction.LoadParamSignExtend32x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.SetInstruction(X64.MovsxLoad32, context.Result, transform.StackFrame, context.Operand1);
	}
}
