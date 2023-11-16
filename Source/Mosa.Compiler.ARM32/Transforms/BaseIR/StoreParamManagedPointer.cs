// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// StoreParamManagedPointer
/// </summary>
[Transform]
public sealed class StoreParamManagedPointer : BaseIRTransform
{
	public StoreParamManagedPointer() : base(IR.StoreParamManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(ARM32.Mov, null, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
