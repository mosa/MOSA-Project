// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.RuntimeCall;

/// <summary>
/// ConvertR4ToR8
/// </summary>
[Transform("ARM32.RuntimeCall")]
public sealed class ConvertR4ToR8 : BaseTransform
{
	public ConvertR4ToR8() : base(Framework.IR.ConvertR4ToR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.ReplaceWithCall(context, "Mosa.Runtime.ARM32.Math.FloatingPoint", "FloatToDouble");
	}
}
