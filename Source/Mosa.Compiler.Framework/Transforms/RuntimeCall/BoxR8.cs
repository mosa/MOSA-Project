// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Transforms.RuntimeCall;

/// <summary>
/// BoxR8
/// </summary>
public sealed class BoxR8 : BaseTransform
{
	public BoxR8() : base(IRInstruction.BoxR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		VMTransformHelper.SetVMCall(transform, context, "BoxR8", context.Result, context.GetOperands());
	}
}
