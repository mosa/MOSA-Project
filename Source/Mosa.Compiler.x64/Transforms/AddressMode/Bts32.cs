// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.AddressMode;

/// <summary>
/// Bts32
/// </summary>
[Transform]
public sealed class Bts32 : BaseAddressModeTransform
{
	public Bts32() : base(X64.Bts32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		AddressModeConversion(context, X64.Mov32);
	}
}
