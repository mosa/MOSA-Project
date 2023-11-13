// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.RuntimeCall;

/// <summary>
/// ConvertR8ToU64
/// </summary>
[Transform("x86.RuntimeCall")]
public sealed class ConvertR8ToU64 : BaseTransform
{
	public ConvertR8ToU64() : base(IR.ConvertR8ToU64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -100;

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.ReplaceWithCall(context, "Mosa.Runtime.Math.Conversion", "R8ToU8");
	}
}
