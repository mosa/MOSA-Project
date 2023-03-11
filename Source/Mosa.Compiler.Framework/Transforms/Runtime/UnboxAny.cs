// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Runtime;

/// <summary>
/// UnboxAny
/// </summary>
public sealed class UnboxAny : BaseRuntimeTransform
{
	public UnboxAny() : base(IRInstruction.UnboxAny, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		SetVMCall(transform, context, "UnboxAny", context.Result, context.GetOperands());
	}
}
