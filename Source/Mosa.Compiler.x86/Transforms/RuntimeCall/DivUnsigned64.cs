// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.RuntimeCall;

/// <summary>
/// DivUnsigned64
/// </summary>
[Transform("x86.RuntimeCall")]
public sealed class DivUnsigned64 : BaseTransform
{
	public DivUnsigned64() : base(IRInstruction.DivUnsigned64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -100;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.ReplaceWithCall(context, "Mosa.Runtime.Math.Division", "udiv64");
	}
}
