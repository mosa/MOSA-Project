// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARM32.Transforms.RuntimeCall;

/// <summary>
/// BitCopyR4To32
/// </summary>
[Transform("ARM32.RuntimeCall")]
public sealed class BitCopyR4To32 : BaseTransform
{
	public BitCopyR4To32() : base(IRInstruction.BitCopyR4To32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.ReplaceWithCall(context, "Mosa.Runtime.ARM32.Math.FloatingPoint", "BitCopyFloatR4ToInt32");
	}
}
