// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.AddressMode;

/// <summary>
/// Btr64
/// </summary>
[Transform("x64.AddressMode")]
public sealed class Btr64 : BaseAddressModeTransform
{
	public Btr64() : base(X64.Btr64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		AddressModeConversion(context, X64.Mov64);
	}
}
