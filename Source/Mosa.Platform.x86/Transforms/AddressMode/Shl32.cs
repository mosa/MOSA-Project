// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.AddressMode;

/// <summary>
/// Shl32
/// </summary>
public sealed class Shl32 : BaseAddressModeTransform
{
	public Shl32() : base(X86.Shl32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		AddressModeConversion(context, X86.Mov32);
	}
}
