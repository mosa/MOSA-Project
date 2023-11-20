// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Compound;

/// <summary>
/// MoveCompound
/// </summary>
public sealed class MoveCompound : BaseCompoundTransform
{
	public MoveCompound() : base(IR.MoveCompound, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		CopyCompound(transform, context, transform.StackFrame, context.Result, transform.StackFrame, context.Operand1, context.Result);
	}
}
