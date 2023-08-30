// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.AddressMode;

/// <summary>
/// And32
/// </summary>
[Transform("x86.AddressMode")]
public sealed class And32 : BaseAddressModeTransform
{
	public And32() : base(X86.And32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		AddressModeConversionCummulative(context, X86.Mov32);
	}
}
