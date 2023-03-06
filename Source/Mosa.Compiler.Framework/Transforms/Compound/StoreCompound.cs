// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Compound;

/// <summary>
/// StoreCompound
/// </summary>
public sealed class StoreCompound : BaseRuntimeTransform
{
	public StoreCompound() : base(IRInstruction.StoreCompound, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		CopyCompound(transform, context, context.Operand3.Type, context.Operand1, context.Operand2, transform.StackFrame, context.Operand3);
	}
}
