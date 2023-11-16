// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// StoreParamObject
/// </summary>
[Transform]
public sealed class StoreParamObject : BaseIRTransform
{
	public StoreParamObject() : base(IR.StoreParamObject, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X86.MovStore32, null, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
