// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.AddressMode;

/// <summary>
/// Subsd
/// </summary>
public sealed class Subsd : BaseAddressModeTransform
{
	public static readonly Subsd Instance = new();

	private Subsd() : base(X86.Subsd, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		AddressModeConversion(context, X86.Movsd);
	}
}
