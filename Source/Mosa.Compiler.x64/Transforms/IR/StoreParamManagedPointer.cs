// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// StoreParamManagedPointer
/// </summary>
[Transform("x64.IR")]
public sealed class StoreParamManagedPointer : BaseIRTransform
{
	public StoreParamManagedPointer() : base(IRInstruction.StoreParamManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X64.MovStore32, null, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
