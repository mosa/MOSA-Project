// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.AddressMode;

/// <summary>
/// Shld32
/// </summary>
[Transform("x86.AddressMode")]
public sealed class Shld32 : BaseAddressModeTransform
{
	public Shld32() : base(X86.Shld32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		AddressModeConversion(context, X86.Mov32);
	}
}
