// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.AddressMode;

/// <summary>
/// Subss
/// </summary>
[Transform("x86.AddressMode")]
public sealed class Subss : BaseAddressModeTransform
{
	public Subss() : base(X86.Subss, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		AddressModeConversion(context, X86.Movss);
	}
}
