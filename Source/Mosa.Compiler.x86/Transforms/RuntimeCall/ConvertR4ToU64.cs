// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.RuntimeCall;

/// <summary>
/// ConvertR4ToU64
/// </summary>
public sealed class ConvertR4ToU64 : BaseTransform
{
	public ConvertR4ToU64() : base(IR.ConvertR4ToU64, TransformType.Manual | TransformType.Transform, -100)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.ReplaceWithCall(context, "Mosa.Runtime.Math.Conversion", "R4ToU8");
	}
}
