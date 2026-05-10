// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.RuntimeCall;

/// <summary>
/// BitCopyR4To32
/// </summary>
public sealed class BitCopyR4To32 : BaseTransform
{
	public static readonly BitCopyR4To32 Instance = new();

	private BitCopyR4To32() : base(IR.BitCopyR4To32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.ReplaceWithCall(context, "Mosa.Runtime.ARM32.Math.FloatingPoint", "BitCopyFloatR4ToInt32");
	}
}
