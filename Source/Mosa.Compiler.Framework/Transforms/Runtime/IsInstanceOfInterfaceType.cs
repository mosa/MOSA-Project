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

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		SetVMCall(transform, context, "IsInstanceOfInterfaceType", context.Result, context.GetOperands());
	}
}
