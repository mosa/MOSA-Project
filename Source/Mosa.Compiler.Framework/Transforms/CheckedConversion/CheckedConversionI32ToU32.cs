// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionI32ToU32
/// </summary>
public sealed class CheckedConversionI32ToU32 : BaseCheckedConversionTransform
{
	public CheckedConversionI32ToU32() : base(IR.CheckedConversionI32ToU32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "I4ToU4");
	}
}
