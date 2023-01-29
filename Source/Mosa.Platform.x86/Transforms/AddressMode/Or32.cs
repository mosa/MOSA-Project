// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.AddressMode;

/// <summary>
/// Or32
/// </summary>
public sealed class Or32 : BaseTransform
{
	public Or32() : base(X86.Or32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return !X86TransformHelper.IsAddressMode(context);
	}

	public override void Transform(Context context, TransformContext transform)
	{
		X86TransformHelper.AddressModeConversionCummulative(context, X86.Mov32);
	}
}
