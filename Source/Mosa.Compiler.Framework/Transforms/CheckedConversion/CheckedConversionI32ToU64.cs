// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionI32ToU64
/// </summary>
public sealed class CheckedConversionI32ToU64 : BaseCheckedConversionTransform
{
	public CheckedConversionI32ToU64() : base(IR.CheckedConversionI32ToU64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "I4ToU8");
	}
}
