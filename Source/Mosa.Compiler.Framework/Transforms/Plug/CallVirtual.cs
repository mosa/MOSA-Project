// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Plug;

/// <summary>
/// CallVirtual
/// </summary>
public sealed class CallVirtual : BasePlugTransform
{
	public CallVirtual() : base(IR.CallVirtual, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Plug(context, transform);
	}
}
