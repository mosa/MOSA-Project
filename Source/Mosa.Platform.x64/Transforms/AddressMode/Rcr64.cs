// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.AddressMode;

/// <summary>
/// Rcr64
/// </summary>
[Transform("x64.AddressMode")]
public sealed class Rcr64 : BaseAddressModeTransform
{
	public Rcr64() : base(X64.Rcr64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		AddressModeConversion(context, X64.Mov64);
	}
}
