// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.RuntimeCall;

/// <summary>
/// ConvertR4ToU32
public sealed class ConvertR4ToU32 : BaseTransform
{
	public ConvertR4ToU32() : base(IR.ConvertR4ToU32, TransformType.Manual | TransformType.Transform, -100)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.ReplaceWithCall(context, "Mosa.Runtime.Math.Conversion", "R4ToU4");
	}
}
