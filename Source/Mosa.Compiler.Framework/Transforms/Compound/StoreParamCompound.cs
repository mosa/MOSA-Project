// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Compound;

/// <summary>
/// StoreParamCompound
/// </summary>
public sealed class StoreParamCompound : BaseRuntimeTransform
{
	public StoreParamCompound() : base(IRInstruction.StoreParamCompound, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		CopyCompound(transform, context, context.Operand2.Type, transform.StackFrame, context.Operand1, transform.StackFrame, context.Operand2);
	}
}
