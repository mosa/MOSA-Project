// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.AddressMode;

/// <summary>
/// Divsd
/// </summary>
[Transform("x86.AddressMode")]
public sealed class Divsd : BaseAddressModeTransform
{
	public Divsd() : base(X86.Divsd, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		AddressModeConversion(context, X86.Movsd);
	}
}
