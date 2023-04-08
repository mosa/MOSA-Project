// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.AddressMode;

/// <summary>
/// Add32
/// </summary>
[Transform("x86.AddressMode")]
public sealed class Add32 : BaseAddressModeTransform
{
	public Add32() : base(X86.Add32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		AddressModeConversionCummulative(context, X86.Mov32);
	}
}
