// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// StoreParamObject
/// </summary>
[Transform("x86.IR")]
public sealed class StoreParamObject : BaseIRTransform
{
	public StoreParamObject() : base(Framework.IR.StoreParamObject, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X86.MovStore32, null, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
