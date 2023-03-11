// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Runtime;

/// <summary>
/// IsInstanceOfInterfaceType
/// </summary>
public sealed class IsInstanceOfInterfaceType : BaseRuntimeTransform
{
	public IsInstanceOfInterfaceType() : base(IRInstruction.IsInstanceOfInterfaceType, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		SetVMCall(transform, context, "IsInstanceOfInterfaceType", context.Result, context.GetOperands());
	}
}
