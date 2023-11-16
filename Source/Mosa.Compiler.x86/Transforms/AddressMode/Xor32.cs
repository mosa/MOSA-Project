// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.AddressMode;

/// <summary>
/// Xor32
/// </summary>
[Transform]
public sealed class Xor32 : BaseAddressModeTransform
{
	public Xor32() : base(X86.Xor32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		AddressModeConversionCummulative(context, X86.Mov32);
	}
}
