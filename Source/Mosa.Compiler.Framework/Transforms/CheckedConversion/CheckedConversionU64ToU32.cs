// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionU64ToU32
/// </summary>
public sealed class CheckedConversionU64ToU32 : BaseCheckedConversionTransform
{
	public CheckedConversionU64ToU32() : base(IR.CheckedConversionU64ToU32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "U8ToU4");
	}
}
