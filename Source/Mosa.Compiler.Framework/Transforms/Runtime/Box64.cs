// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Runtime;

/// <summary>
/// Box64
/// </summary>
public sealed class Box64 : BaseRuntimeTransform
{
	public Box64() : base(IR.Box64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		SetVMCall(transform, context, "Box64", context.Result, context.GetOperands());
	}
}
