// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.RuntimeCall;

/// <summary>
/// ConvertR8ToU64
/// </summary>
public sealed class ConvertR8ToU64 : BaseTransform
{
	public ConvertR8ToU64() : base(IR.ConvertR8ToU64, TransformType.Manual | TransformType.Transform, -100)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.ReplaceWithCall(context, "Mosa.Runtime.Math.Conversion", "R8ToU8");
	}
}
