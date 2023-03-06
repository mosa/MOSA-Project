// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.AddressMode;

/// <summary>
/// Subss
/// </summary>
public sealed class Subss : BaseAddressModeTransform
{
	public Subss() : base(X86.Subss, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		AddressModeConversion(context, X86.Mov32);
	}
}
