// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.AddressMode;

/// <summary>
/// Subsd
/// </summary>
[Transform]
public sealed class Subsd : BaseAddressModeTransform
{
	public Subsd() : base(X86.Subsd, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		AddressModeConversion(context, X86.Movsd);
	}
}
