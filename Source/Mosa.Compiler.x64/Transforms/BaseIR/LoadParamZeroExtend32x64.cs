// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// LoadParamZeroExtend32x64
/// </summary>
public sealed class LoadParamZeroExtend32x64 : BaseIRTransform
{
	public LoadParamZeroExtend32x64() : base(IR.LoadParamZeroExtend32x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X64.MovzxLoad32, context.Result, transform.StackFrame, context.Operand1);
	}
}
