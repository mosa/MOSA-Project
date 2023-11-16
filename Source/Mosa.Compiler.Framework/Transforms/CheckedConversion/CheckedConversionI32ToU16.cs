// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionI32ToU16
/// </summary>
public sealed class CheckedConversionI32ToU16 : BaseCheckedConversionTransform
{
	public CheckedConversionI32ToU16() : base(IR.CheckedConversionI32ToU16, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "I4ToU2");
	}
}
