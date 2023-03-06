// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.AddressMode;

/// <summary>
/// Or64
/// </summary>
public sealed class Or64 : BaseAddressModeTransform
{
	public Or64() : base(X64.Or64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		AddressModeConversionCummulative(context, X64.Mov64);
	}
}
