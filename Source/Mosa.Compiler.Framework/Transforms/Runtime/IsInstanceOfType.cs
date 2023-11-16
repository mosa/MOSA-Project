// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Runtime;

/// <summary>
/// IsInstanceOfType
/// </summary>
public sealed class IsInstanceOfType : BaseRuntimeTransform
{
	public IsInstanceOfType() : base(IR.IsInstanceOfType, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		SetVMCall(transform, context, "IsInstanceOfType", context.Result, context.GetOperands());
	}
}
