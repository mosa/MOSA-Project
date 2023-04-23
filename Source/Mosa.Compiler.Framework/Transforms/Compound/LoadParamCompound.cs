// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Compound;

/// <summary>
/// LoadParamCompound
/// </summary>
public sealed class LoadParamCompound : BaseCompoundTransform
{
	public LoadParamCompound() : base(IRInstruction.LoadParamCompound, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		CopyCompound(transform, context, context.Result, transform.StackFrame, context.Result, transform.StackFrame, context.Operand1);
	}
}
