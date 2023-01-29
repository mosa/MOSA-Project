// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.AddressMode;

/// <summary>
/// Roundss
/// </summary>
public sealed class Roundss : BaseTransform
{
	public Roundss() : base(X86.Roundss, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return !X86TransformHelper.IsAddressMode(context);
	}

	public override void Transform(Context context, TransformContext transform)
	{
		X86TransformHelper.AddressModeConversion(context, X86.Movss);
	}
}
