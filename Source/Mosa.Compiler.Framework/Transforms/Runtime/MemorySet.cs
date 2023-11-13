// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Runtime;

/// <summary>
/// MemorySet
/// </summary>
public sealed class MemorySet : BaseRuntimeTransform
{
	public MemorySet() : base(IR.MemorySet, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		SetVMCall(transform, context, "MemorySet", context.Result, context.GetOperands());
	}
}
