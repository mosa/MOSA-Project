// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionI32ToI16
/// </summary>
public sealed class CheckedConversionI32ToI16 : BaseCheckedConversionTransform
{
	public CheckedConversionI32ToI16() : base(IR.CheckedConversionI32ToI16, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "I4ToI2");
	}
}
