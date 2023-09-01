// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.AddressMode;

/// <summary>
/// Subsd
/// </summary>
[Transform("x64.AddressMode")]
public sealed class Subsd : BaseAddressModeTransform
{
	public Subsd() : base(X64.Subsd, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		AddressModeConversion(context, X64.Mov32);
	}
}
