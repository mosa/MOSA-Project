// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.AddressMode;

/// <summary>
/// Shl64
/// </summary>
[Transform("x64.AddressMode")]
public sealed class Shl64 : BaseAddressModeTransform
{
	public Shl64() : base(X64.Shl64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		AddressModeConversion(context, X64.Mov64);
	}
}
