// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Compound;

/// <summary>
/// StoreParamCompound
/// </summary>
public sealed class StoreParamCompound : BaseCompoundTransform
{
	public StoreParamCompound() : base(IR.StoreParamCompound, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		CopyCompound(transform, context, transform.StackFrame, context.Operand1, transform.StackFrame, context.Operand2, context.Operand1);
	}
}
