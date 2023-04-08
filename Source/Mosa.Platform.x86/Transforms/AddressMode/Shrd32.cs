// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.AddressMode;

/// <summary>
/// Shrd32
/// </summary>
[Transform("x86.AddressMode")]
public sealed class Shrd32 : BaseAddressModeTransform
{
	public Shrd32() : base(X86.Shrd32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		AddressModeConversion(context, X86.Mov32);
	}
}
