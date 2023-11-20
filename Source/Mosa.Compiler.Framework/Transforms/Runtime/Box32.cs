// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Runtime;

/// <summary>
/// Box32
/// </summary>
public sealed class Box32 : BaseRuntimeTransform
{
	public Box32() : base(IR.Box32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		SetVMCall(transform, context, "Box32", context.Result, context.GetOperands());
	}
}
