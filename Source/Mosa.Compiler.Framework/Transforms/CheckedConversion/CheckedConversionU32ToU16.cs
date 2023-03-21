// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionU32ToU16
/// </summary>
public sealed class CheckedConversionU32ToU16 : BaseCheckedConversionTransform
{
	public CheckedConversionU32ToU16() : base(IRInstruction.CheckedConversionU32ToU16, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		CallCheckOverflow(transform, context, "U4ToU2");
	}
}
