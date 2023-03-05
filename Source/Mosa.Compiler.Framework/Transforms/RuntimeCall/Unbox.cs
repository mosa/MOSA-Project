// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Transforms.RuntimeCall;

/// <summary>
/// Unbox
/// </summary>
public sealed class Unbox : BaseTransform
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
		VMTransformHelper.SetVMCall(transform, context, "Unbox", context.Result, context.GetOperands());
	}
}
