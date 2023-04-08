// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.AddressMode;

/// <summary>
/// Or32
/// </summary>
[Transform("x86.AddressMode")]
public sealed class Or32 : BaseAddressModeTransform
{
	public Or32() : base(X86.Or32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		AddressModeConversionCummulative(context, X86.Mov32);
	}
}
