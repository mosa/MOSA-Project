// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Transforms.Plug;

/// <summary>
/// CallDirect
/// </summary>
public sealed class CallDirect : BaseTransform
{
	public CallDirect() : base(IRInstruction.CallDirect, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, TransformContext transform)
	{
		return PlugTransformHelper.IsPlugged(context, transform);
	}

	public override void Transform(Context context, TransformContext transform)
	{
		PlugTransformHelper.Plug(context, transform);
	}
}
