// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Transforms.Plug;

/// <summary>
/// CallStatic
/// </summary>
public sealed class CallStatic : BaseTransform
{
	public CallStatic() : base(IRInstruction.CallStatic, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, TransformContext transform)
	{
		return PlugHelper.IsPlugged(context, transform);
	}

	public override void Transform(Context context, TransformContext transform)
	{
		PlugHelper.Plug(context, transform);
	}
}
