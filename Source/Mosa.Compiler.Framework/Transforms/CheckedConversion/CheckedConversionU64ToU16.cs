// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionU64TCheckedConversionU64ToU16oU86
/// </summary>
public sealed class CheckedConversionU64ToU16 : BaseCheckedConversionTransform
{
	public CheckedConversionU64ToU16() : base(IR.CheckedConversionU64ToU16, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "U8ToU2");
	}
}
