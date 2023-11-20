// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Runtime;

/// <summary>
/// IsInstanceOfInterfaceType
/// </summary>
public sealed class IsInstanceOfInterfaceType : BaseRuntimeTransform
{
	public IsInstanceOfInterfaceType() : base(IR.IsInstanceOfInterfaceType, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		SetVMCall(transform, context, "IsInstanceOfInterfaceType", context.Result, context.GetOperands());
	}
}
