// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Framework.Transforms.RuntimeTime;

/// <summary>
/// Unbox
/// </summary>
public sealed class Unbox : BaseRuntimeTransform
{
	public Unbox() : base(IRInstruction.Unbox, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		SetVMCall(transform, context, "Unbox", context.Result, context.GetOperands());
	}
}
