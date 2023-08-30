// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.AddressMode;

/// <summary>
/// Addss
/// </summary>
[Transform("x86.AddressMode")]
public sealed class Addss : BaseAddressModeTransform
{
	public Addss() : base(X86.Addss, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		AddressModeConversionCummulative(context, X86.Movss);
	}
}
