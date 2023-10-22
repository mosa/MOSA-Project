// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Compound;

/// <summary>
/// StoreCompound
/// </summary>
public sealed class StoreCompound : BaseCompoundTransform
{
	public StoreCompound() : base(IRInstruction.StoreCompound, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		CopyCompound(transform, context, context.Operand1, context.Operand2, transform.StackFrame, context.Operand3, context.Operand3);
	}
}
