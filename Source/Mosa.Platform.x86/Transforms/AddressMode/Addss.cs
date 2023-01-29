// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.AddressMode;

/// <summary>
/// Addss
/// </summary>
public sealed class Addss : BaseTransform
{
	public Addss() : base(X86.Addss, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return !X86TransformHelper.IsAddressMode(context);
	}

	public override void Transform(Context context, TransformContext transform)
	{
		X86TransformHelper.AddressModeConversionCummulative(context, X86.Movss);
	}
}
