// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionR4ToI8
/// </summary>
public sealed class CheckedConversionR4ToI8 : BaseCheckedConversionTransform
{
	public CheckedConversionR4ToI8() : base(IR.CheckedConversionR4ToI8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "R4ToI1");
	}
}
