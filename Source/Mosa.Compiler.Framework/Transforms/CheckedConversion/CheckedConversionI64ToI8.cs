// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionI64ToI8
/// </summary>
public sealed class CheckedConversionI64ToI8 : BaseCheckedConversionTransform
{
	public CheckedConversionI64ToI8() : base(IRInstruction.CheckedConversionI64ToI8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "I8ToI1");
	}
}
