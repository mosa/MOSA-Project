// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.AddressMode;

/// <summary>
/// Mulsd
/// </summary>
public sealed class Mulsd : BaseAddressModeTransform
{
	public static readonly Mulsd Instance = new();

	private Mulsd() : base(X64.Mulsd, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		AddressModeConversionCummulative(context, X64.Movsd);
	}
}
