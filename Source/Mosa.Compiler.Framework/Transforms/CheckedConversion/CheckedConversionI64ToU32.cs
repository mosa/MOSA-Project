// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionI64ToU32
/// </summary>
public sealed class CheckedConversionI64ToU32 : BaseCheckedConversionTransform
{
	public CheckedConversionI64ToU32() : base(IR.CheckedConversionI64ToU32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "I8ToU4");
	}
}
