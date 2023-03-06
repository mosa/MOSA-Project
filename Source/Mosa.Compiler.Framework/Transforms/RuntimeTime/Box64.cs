// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Framework.Transforms.RuntimeTime;

/// <summary>
/// Box64
/// </summary>
public sealed class Box64 : BaseRuntimeTransform
{
	public Box64() : base(IRInstruction.Box64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		SetVMCall(transform, context, "Box64", context.Result, context.GetOperands());
	}
}
