// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionU64ToI32
/// </summary>
public sealed class CheckedConversionU64ToI32 : BaseCheckedConversionTransform
{
	public CheckedConversionU64ToI32() : base(IRInstruction.CheckedConversionU64ToI32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "U8ToI4");
	}
}
