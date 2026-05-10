// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.AddressMode;

/// <summary>
/// Shld32
/// </summary>
public sealed class Shld32 : BaseAddressModeTransform
{
	public static readonly Shld32 Instance = new();

	private Shld32() : base(X86.Shld32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		AddressModeConversion(context, X86.Mov32);
	}
}
