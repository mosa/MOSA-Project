// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// LoadParamManagedPointer
/// </summary>
[Transform("x64.IR")]
public sealed class LoadParamManagedPointer : BaseIRTransform
{
	public LoadParamManagedPointer() : base(IRInstruction.LoadParamManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.SetInstruction(X64.MovLoad32, context.Result, transform.StackFrame, context.Operand1);
	}
}
