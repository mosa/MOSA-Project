// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.RuntimeCall;

/// <summary>
/// DivSigned64
/// </summary>
[Transform("x86.RuntimeCall")]
public sealed class DivSigned64 : BaseTransform
{
	public DivSigned64() : base(IRInstruction.DivSigned64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -100;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.ReplaceWithCall(context, "Mosa.Runtime.Math.Division", "sdiv64");
	}
}
