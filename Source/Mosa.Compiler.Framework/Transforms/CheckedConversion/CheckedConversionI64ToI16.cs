// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionI64ToI16
/// </summary>
public sealed class CheckedConversionI64ToI16 : BaseCheckedConversionTransform
{
	public CheckedConversionI64ToI16() : base(IRInstruction.CheckedConversionI64ToI16, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		CallCheckOverflow(transform, context, "I8ToI2");
	}
}
