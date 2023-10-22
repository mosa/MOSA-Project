// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// StoreParamManagedPointer
/// </summary>
[Transform("ARM32.IR")]
public sealed class StoreParamManagedPointer : BaseIRTransform
{
	public StoreParamManagedPointer() : base(IRInstruction.StoreParamManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(ARM32.Mov, null, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
