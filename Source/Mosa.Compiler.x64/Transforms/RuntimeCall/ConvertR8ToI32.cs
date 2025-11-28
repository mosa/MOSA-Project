// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.RuntimeCall;

/// <summary>
/// ConvertR8ToI32
public sealed class ConvertR8ToI32 : BaseTransform
{
	public ConvertR8ToI32() : base(IR.ConvertR8ToI32, TransformType.Manual | TransformType.Transform, -100)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.ReplaceWithCall(context, "Mosa.Runtime.Math.Conversion", "R8ToI4");
	}
}
