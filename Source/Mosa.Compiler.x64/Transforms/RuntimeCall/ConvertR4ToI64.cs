// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.RuntimeCall;

/// <summary>
/// ConvertR4ToI64
/// </summary>
[Transform("x64.RuntimeCall")]
public sealed class ConvertR4ToI64 : BaseTransform
{
	public ConvertR4ToI64() : base(IRInstruction.ConvertR4ToI64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -100;

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.ReplaceWithCall(context, "Mosa.Runtime.Math.Conversion", "R4ToI8");
	}
}
