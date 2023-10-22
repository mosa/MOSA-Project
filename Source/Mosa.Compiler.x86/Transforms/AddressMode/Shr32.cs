// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.AddressMode;

/// <summary>
/// Shr32
/// </summary>
[Transform("x86.AddressMode")]
public sealed class Shr32 : BaseAddressModeTransform
{
	public Shr32() : base(X86.Shr32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		AddressModeConversion(context, X86.Mov32);
	}
}
