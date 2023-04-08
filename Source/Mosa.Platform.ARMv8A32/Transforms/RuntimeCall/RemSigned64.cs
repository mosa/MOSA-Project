// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.RuntimeCall;

/// <summary>
/// RemSigned64
/// </summary>
[Transform("ARMv8A32.RuntimeCall")]
public sealed class RemSigned64 : BaseTransform
{
	public RemSigned64() : base(IRInstruction.RemSigned64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.ReplaceWithCall(context, "Mosa.Runtime.Math.Division", "smod64");
	}
}
