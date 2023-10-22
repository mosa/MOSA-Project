// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.AddressMode;

/// <summary>
/// Shrd64
/// </summary>
[Transform("x64.AddressMode")]
public sealed class Shrd64 : BaseAddressModeTransform
{
	public Shrd64() : base(X64.Shrd64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		AddressModeConversion(context, X64.Mov64);
	}
}
