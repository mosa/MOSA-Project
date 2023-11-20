// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Runtime;

/// <summary>
/// Box
/// </summary>
public sealed class Box : BaseRuntimeTransform
{
	public Box() : base(IR.Box, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		SetVMCall(transform, context, "Box", context.Result, context.GetOperands());
	}
}
