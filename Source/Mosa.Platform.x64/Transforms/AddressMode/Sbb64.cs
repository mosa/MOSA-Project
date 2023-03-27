// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.AddressMode;

/// <summary>
/// Sbb64
/// </summary>
[Transform("x64.AddressMode")]
public sealed class Sbb64 : BaseAddressModeTransform
{
	public Sbb64() : base(X64.Sbb64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		AddressModeConversion(context, X64.Mov64);
	}
}
