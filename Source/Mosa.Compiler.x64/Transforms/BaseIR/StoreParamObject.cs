// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// StoreParamObject
/// </summary>
[Transform("x64.BaseIR")]
public sealed class StoreParamObject : BaseIRTransform
{
	public StoreParamObject() : base(Framework.IR.StoreParamObject, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X64.MovStore64, null, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
