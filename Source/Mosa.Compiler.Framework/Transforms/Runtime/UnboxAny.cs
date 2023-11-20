// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Runtime;

/// <summary>
/// UnboxAny
/// </summary>
public sealed class UnboxAny : BaseRuntimeTransform
{
	public UnboxAny() : base(IR.UnboxAny, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		SetVMCall(transform, context, "UnboxAny", context.Result, context.GetOperands());
	}
}
