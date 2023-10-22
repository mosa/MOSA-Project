// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// LoadParamManagedPointer
/// </summary>
[Transform("ARM32.IR")]
public sealed class LoadParamManagedPointer : BaseIRTransform
{
	public LoadParamManagedPointer() : base(IRInstruction.LoadParamManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		TransformLoad(transform, context, ARM32.Ldr32, context.Result, transform.StackFrame, context.Operand1);
	}
}
