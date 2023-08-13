// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARM32.Transforms.RuntimeCall;

/// <summary>
/// DivUnsigned64
/// </summary>
[Transform("ARM32.RuntimeCall")]
public sealed class DivUnsigned64 : BaseTransform
{
	public DivUnsigned64() : base(IRInstruction.DivUnsigned64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.ReplaceWithCall(context, "Mosa.Runtime.Math.Division", "udiv64");
	}
}
