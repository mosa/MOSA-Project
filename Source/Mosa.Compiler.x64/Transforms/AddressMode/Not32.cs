// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.AddressMode;

/// <summary>
/// Not32
/// </summary>
[Transform("x64.AddressMode")]
public sealed class Not32 : BaseAddressModeTransform
{
	public Not32() : base(X64.Not32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		AddressModeConversion(context, X64.Mov32);
	}
}
