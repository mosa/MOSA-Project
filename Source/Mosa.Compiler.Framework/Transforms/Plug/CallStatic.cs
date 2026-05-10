// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Plug;

/// <summary>
/// CallStatic
/// </summary>
public sealed class CallStatic : BasePlugTransform
{
	public static readonly CallStatic Instance = new();

	private CallStatic() : base(IR.CallStatic, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Plug(context, transform);
	}
}
