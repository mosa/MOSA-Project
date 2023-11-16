// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.AddressMode;

/// <summary>
/// Roundsd
/// </summary>
[Transform]
public sealed class Roundsd : BaseAddressModeTransform
{
	public Roundsd() : base(X64.Roundsd, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		AddressModeConversion(context, X64.Movsd);
	}
}
