// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionU32ToI16
/// </summary>
public sealed class CheckedConversionU32ToI16 : BaseCheckedConversionTransform
{
	public CheckedConversionU32ToI16() : base(Framework.IR.CheckedConversionU32ToI16, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "U4ToI2");
	}
}
