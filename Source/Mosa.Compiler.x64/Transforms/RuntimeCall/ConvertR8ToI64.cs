// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.RuntimeCall;

/// <summary>
/// ConvertR8ToI64
/// </summary>
[Transform("x64.RuntimeCall")]
public sealed class ConvertR8ToI64 : BaseTransform
{
	public ConvertR8ToI64() : base(IRInstruction.ConvertR8ToI64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -100;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.ReplaceWithCall(context, "Mosa.Runtime.Math.Conversion", "R8ToI8");
	}
}
