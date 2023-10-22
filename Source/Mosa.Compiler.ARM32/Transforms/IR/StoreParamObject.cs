// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// StoreParamObject
/// </summary>
[Transform("ARM32.IR")]
public sealed class StoreParamObject : BaseIRTransform
{
	public StoreParamObject() : base(IRInstruction.StoreParamObject, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(ARM32.Mov, null, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
