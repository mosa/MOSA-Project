// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Runtime;

/// <summary>
/// Unbox
/// </summary>
public sealed class Unbox : BaseRuntimeTransform
{
	public Unbox() : base(IR.Unbox, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		//SetVMCall(transform, context, "Unbox", context.Result, context.GetOperands());

		context.SetInstruction(transform.MoveInstruction, context.Result, context.Operand1);
	}
}
