// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionR4ToI64
/// </summary>
public sealed class CheckedConversionR4ToI64 : BaseCheckedConversionTransform
{
	public CheckedConversionR4ToI64() : base(IR.CheckedConversionR4ToI64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "R4ToI8");
	}
}
