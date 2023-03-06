// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.AddressMode;

/// <summary>
/// Shl32
/// </summary>
public sealed class Shl32 : BaseAddressModeTransform
{
	public Shl32() : base(X64.Shl32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		AddressModeConversion(context, X64.Mov32);
	}
}
