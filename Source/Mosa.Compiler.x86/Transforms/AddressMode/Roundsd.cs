// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.AddressMode;

/// <summary>
/// Roundsd
/// </summary>
[Transform]
public sealed class Roundsd : BaseAddressModeTransform
{
	public Roundsd() : base(X86.Roundsd, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		AddressModeConversion(context, X86.Movsd);
	}
}
