// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Runtime;

/// <summary>
/// GetVirtualFunctionPtr
/// </summary>
public sealed class GetVirtualFunctionPtr : BaseRuntimeTransform
{
	public GetVirtualFunctionPtr() : base(IRInstruction.GetVirtualFunctionPtr, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		SetVMCall(transform, context, "GetVirtualFunctionPtr", context.Result, context.GetOperands());
	}
}
