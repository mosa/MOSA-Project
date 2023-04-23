// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Compound;

/// <summary>
/// LoadCompound
/// </summary>
public sealed class LoadCompound : BaseCompoundTransform
{
	public LoadCompound() : base(IRInstruction.LoadCompound, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		CopyCompound(transform, context, context.Result, transform.StackFrame, context.Result, context.Operand1, context.Operand2);
	}
}
