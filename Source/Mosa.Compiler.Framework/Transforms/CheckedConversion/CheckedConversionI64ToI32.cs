// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionI64ToI32
/// </summary>
public sealed class CheckedConversionI64ToI32 : BaseCheckedConversionTransform
{
	public CheckedConversionI64ToI32() : base(Framework.IR.CheckedConversionI64ToI32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "I8ToI4");
	}
}
