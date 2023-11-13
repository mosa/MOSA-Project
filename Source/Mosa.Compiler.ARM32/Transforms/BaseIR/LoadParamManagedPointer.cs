// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// LoadParamManagedPointer
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class LoadParamManagedPointer : BaseIRTransform
{
	public LoadParamManagedPointer() : base(IR.LoadParamManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		TransformLoad(transform, context, ARM32.Ldr32, context.Result, transform.StackFrame, context.Operand1);
	}
}
