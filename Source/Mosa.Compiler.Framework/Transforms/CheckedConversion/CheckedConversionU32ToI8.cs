// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionU32ToI8
/// </summary>
public sealed class CheckedConversionU32ToI8 : BaseCheckedConversionTransform
{
	public CheckedConversionU32ToI8() : base(IR.CheckedConversionU32ToI8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "U4ToI1");
	}
}
