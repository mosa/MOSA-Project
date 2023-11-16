// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// LoadParamManagedPointer
/// </summary>
[Transform]
public sealed class LoadParamManagedPointer : BaseIRTransform
{
	public LoadParamManagedPointer() : base(IR.LoadParamManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X64.MovLoad32, context.Result, transform.StackFrame, context.Operand1);
	}
}
