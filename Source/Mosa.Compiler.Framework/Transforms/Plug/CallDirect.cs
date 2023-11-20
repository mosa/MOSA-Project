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

	public override void Transform(Context context, Transform transform)
	{
		Plug(context, transform);
	}
}
