// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.RuntimeTime;

/// <summary>
/// Rethrow
/// </summary>
public sealed class Rethrow : BaseRuntimeTransform
{
	public Rethrow() : base(IRInstruction.Rethrow, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		SetVMCall(transform, context, "Rethrow", context.Result, context.GetOperands());
	}
}
