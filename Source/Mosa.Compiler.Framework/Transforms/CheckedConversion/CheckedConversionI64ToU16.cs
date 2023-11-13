// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionI64ToU16
/// </summary>
public sealed class CheckedConversionI64ToU16 : BaseCheckedConversionTransform
{
	public CheckedConversionI64ToU16() : base(Framework.IR.CheckedConversionI64ToU16, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "I8ToU2");
	}
}
