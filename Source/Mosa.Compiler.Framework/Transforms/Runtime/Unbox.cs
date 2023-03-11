// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Runtime;

/// <summary>
/// Unbox
/// </summary>
public sealed class Unbox : BaseRuntimeTransform
{
	public Unbox() : base(IRInstruction.Unbox, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		SetVMCall(transform, context, "Unbox", context.Result, context.GetOperands());
	}
}
