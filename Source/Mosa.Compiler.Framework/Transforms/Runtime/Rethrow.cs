// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Runtime;

/// <summary>
/// Rethrow
/// </summary>
public sealed class Rethrow : BaseRuntimeTransform
{
	public Rethrow() : base(IR.Rethrow, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		SetVMCall(transform, context, "Rethrow", context.Result, context.GetOperands());
	}
}
