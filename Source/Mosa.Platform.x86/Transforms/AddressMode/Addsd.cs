// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.AddressMode;

/// <summary>
/// Addsd
/// </summary>
public sealed class Addsd : BaseAddressModeTransform
{
	public Addsd() : base(X86.Addsd, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		AddressModeConversionCummulative(context, X86.Movsd);
	}
}
