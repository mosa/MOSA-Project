// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// LoadParamZeroExtend32x64
/// </summary>
[Transform("x64.IR")]
public sealed class LoadParamZeroExtend32x64 : BaseIRTransform
{
	public LoadParamZeroExtend32x64() : base(IRInstruction.LoadParamZeroExtend32x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.SetInstruction(X64.MovzxLoad32, context.Result, transform.StackFrame, context.Operand1);
	}
}
