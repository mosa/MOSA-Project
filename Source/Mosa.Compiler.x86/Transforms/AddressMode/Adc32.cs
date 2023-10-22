// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.AddressMode;

/// <summary>
/// Adc32
/// </summary>
[Transform("x86.AddressMode")]
public sealed class Adc32 : BaseAddressModeTransform
{
	public Adc32() : base(X86.Adc32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		AddressModeConversionCummulative(context, X86.Mov32);
	}
}
