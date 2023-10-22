// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.AddressMode;

/// <summary>
/// Addss
/// </summary>
[Transform("x64.AddressMode")]
public sealed class Addss : BaseAddressModeTransform
{
	public Addss() : base(X64.Addss, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		AddressModeConversionCummulative(context, X64.Movss);
	}
}
