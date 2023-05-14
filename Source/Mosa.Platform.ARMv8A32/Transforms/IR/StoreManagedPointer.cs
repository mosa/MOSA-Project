// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// StoreManagedPointer
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class StoreManagedPointer : BaseIRTransform
{
	public StoreManagedPointer() : base(IRInstruction.StoreManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		TransformLoad(transform, context, ARMv8A32.Ldr32, context.Result, transform.StackFrame, context.Operand1);
	}
}
