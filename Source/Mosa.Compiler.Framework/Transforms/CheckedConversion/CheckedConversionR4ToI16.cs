// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionR4ToI16
/// </summary>
public sealed class CheckedConversionR4ToI16 : BaseCheckedConversionTransform
{
	public CheckedConversionR4ToI16() : base(IR.CheckedConversionR4ToI16, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "R4ToI2");
	}
}
