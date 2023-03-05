// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Transforms.RuntimeCall;

/// <summary>
/// IsInstanceOfType
/// </summary>
public sealed class IsInstanceOfType : BaseTransform
{
	public IsInstanceOfType() : base(IRInstruction.IsInstanceOfType, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		VMHelper.SetVMCall(transform, context, "IsInstanceOfType", context.Result, context.GetOperands());
	}
}
