// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.AddressMode;

/// <summary>
/// Addsd
/// </summary>
[Transform]
public sealed class Addsd : BaseAddressModeTransform
{
	public Addsd() : base(X64.Addsd, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		AddressModeConversionCummulative(context, X64.Movsd);
	}
}
