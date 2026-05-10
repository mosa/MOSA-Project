// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.AddressMode;

/// <summary>
/// Bts32
/// </summary>
public sealed class Bts32 : BaseAddressModeTransform
{
	public static readonly Bts32 Instance = new();

	private Bts32() : base(X64.Bts32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		AddressModeConversion(context, X64.Mov32);
	}
}
