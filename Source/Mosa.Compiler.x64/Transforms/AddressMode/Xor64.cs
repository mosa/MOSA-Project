// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.AddressMode;

/// <summary>
/// Xor64
/// </summary>
[Transform("x64.AddressMode")]
public sealed class Xor64 : BaseAddressModeTransform
{
	public Xor64() : base(X64.Xor64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		AddressModeConversionCummulative(context, X64.Mov64);
	}
}
