// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.AddressMode;

/// <summary>
/// Adc32
/// </summary>
[Transform]
public sealed class Adc32 : BaseAddressModeTransform
{
	public Adc32() : base(X64.Adc32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		AddressModeConversionCummulative(context, X64.Mov32);
	}
}
