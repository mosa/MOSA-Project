// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Transforms.RuntimeCall;

/// <summary>
/// Box32
/// </summary>
public sealed class Box32 : BaseTransform
{
	public Box32() : base(IRInstruction.Box32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		VMTransformHelper.SetVMCall(transform, context, "Box32", context.Result, context.GetOperands());
	}
}
