// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.AddressMode;

/// <summary>
/// Sbb32
/// </summary>
[Transform("x86.AddressMode")]
public sealed class Sbb32 : BaseAddressModeTransform
{
	public Sbb32() : base(X86.Sbb32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		AddressModeConversion(context, X86.Mov32);
	}
}
