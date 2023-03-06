// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Framework.Transforms.RuntimeTime;

/// <summary>
/// Box
/// </summary>
public sealed class Box : BaseRuntimeTransform
{
	public Box() : base(IRInstruction.Box, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		SetVMCall(transform, context, "Box", context.Result, context.GetOperands());
	}
}
