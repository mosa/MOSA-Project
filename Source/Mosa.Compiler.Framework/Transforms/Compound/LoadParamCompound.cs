// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Compound;

/// <summary>
/// LoadParamCompound
/// </summary>
public sealed class LoadParamCompound : BaseCompoundTransform
{
	public LoadParamCompound() : base(Framework.IR.LoadParamCompound, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		CopyCompound(transform, context, transform.StackFrame, context.Result, transform.StackFrame, context.Operand1, context.Result);
	}
}
