// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.AddressMode;

/// <summary>
/// IMul32
/// </summary>
[Transform("x64.AddressMode")]
public sealed class IMul32 : BaseAddressModeTransform
{
	public IMul32() : base(X64.IMul32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		AddressModeConversionCummulative(context, X64.Mov32);
	}
}
