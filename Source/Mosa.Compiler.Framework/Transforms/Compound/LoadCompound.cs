// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Compound;

/// <summary>
/// LoadCompound
/// </summary>
public sealed class LoadCompound : BaseCompoundTransform
{
	public LoadCompound() : base(IR.LoadCompound, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		CopyCompound(transform, context, transform.StackFrame, context.Result, context.Operand1, context.Operand2, context.Result);
	}
}
