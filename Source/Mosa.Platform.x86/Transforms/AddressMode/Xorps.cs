// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.AddressMode;

/// <summary>
/// Xorps
/// </summary>
[Transform("x86.AddressMode")]
public sealed class Xorps : BaseAddressModeTransform
{
	public Xorps() : base(X86.Xorps, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		AddressModeConversion(context, X86.Xorps);
	}
}
