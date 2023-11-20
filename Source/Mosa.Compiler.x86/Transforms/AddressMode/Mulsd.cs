// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.AddressMode;

/// <summary>
/// Mulsd
/// </summary>
public sealed class Mulsd : BaseAddressModeTransform
{
	public Mulsd() : base(X86.Mulsd, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		AddressModeConversionCummulative(context, X86.Movsd);
	}
}
