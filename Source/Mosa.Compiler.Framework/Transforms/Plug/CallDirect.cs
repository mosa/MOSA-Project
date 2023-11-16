// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Plug;

/// <summary>
/// CallDirect
/// </summary>
public sealed class CallDirect : BasePlugTransform
{
	public CallDirect() : base(IR.CallDirect, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, Transform transform)
	{
		return IsPlugged(context, transform);
	}

	public override void Transform(Context context, Transform transform)
	{
		Plug(context, transform);
	}
}
