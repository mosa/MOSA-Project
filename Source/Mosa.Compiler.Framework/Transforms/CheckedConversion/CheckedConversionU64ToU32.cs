// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionU64ToU32
/// </summary>
public sealed class CheckedConversionU64ToU32 : BaseCheckedConversionTransform
{
	public CheckedConversionU64ToU32() : base(IRInstruction.CheckedConversionU64ToU32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		CallCheckOverflow(transform, context, "U8ToU4");
	}
}
