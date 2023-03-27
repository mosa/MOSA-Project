// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.AddressMode;

/// <summary>
/// Bts32
/// </summary>
[Transform("x86.AddressMode")]
public sealed class Bts32 : BaseAddressModeTransform
{
	public Bts32() : base(X86.Bts32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		AddressModeConversion(context, X86.Mov32);
	}
}
