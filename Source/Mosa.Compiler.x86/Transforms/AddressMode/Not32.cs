// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.AddressMode;

/// <summary>
/// Not32
/// </summary>
[Transform]
public sealed class Not32 : BaseAddressModeTransform
{
	public Not32() : base(X86.Not32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		AddressModeConversion(context, X86.Mov32);
	}
}
