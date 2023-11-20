// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Runtime;

/// <summary>
/// BoxR4
/// </summary>
public sealed class BoxR4 : BaseRuntimeTransform
{
	public BoxR4() : base(IR.BoxR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		SetVMCall(transform, context, "BoxR4", context.Result, context.GetOperands());
	}
}
