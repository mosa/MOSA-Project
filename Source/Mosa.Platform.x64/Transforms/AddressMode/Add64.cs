// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.AddressMode;

/// <summary>
/// Add64
/// </summary>
public sealed class Add64 : BaseAddressModeTransform
{
	public Add64() : base(X64.Add64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		AddressModeConversionCummulative(context, X64.Mov64);
	}
}
