// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.AddressMode;

/// <summary>
/// Divss
/// </summary>
[Transform("x64.AddressMode")]
public sealed class Divss : BaseAddressModeTransform
{
	public Divss() : base(X64.Divss, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		AddressModeConversion(context, X64.Movss);
	}
}
