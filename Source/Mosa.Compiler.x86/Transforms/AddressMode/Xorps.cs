// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.AddressMode;

/// <summary>
/// Xorps
/// </summary>
public sealed class Xorps : BaseAddressModeTransform
{
	public static readonly Xorps Instance = new();

	private Xorps() : base(X86.Xorps, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		AddressModeConversion(context, X86.Xorps);
	}
}
