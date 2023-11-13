// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.RuntimeCall;

/// <summary>
/// BitCopy32ToR4
/// </summary>
[Transform("ARM32.RuntimeCall")]
public sealed class BitCopy32ToR4 : BaseTransform
{
	public BitCopy32ToR4() : base(IR.BitCopy32ToR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.ReplaceWithCall(context, "Mosa.Runtime.ARM32.Math.FloatingPoint", "BitCopyInt32ToFloatR4");
	}
}
