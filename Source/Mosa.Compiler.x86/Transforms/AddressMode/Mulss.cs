// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.AddressMode;

/// <summary>
/// Mulss
/// </summary>
public sealed class Mulss : BaseAddressModeTransform
{
	public static readonly Mulss Instance = new();

	private Mulss() : base(X86.Mulss, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		AddressModeConversionCummulative(context, X86.Movss);
	}
}
