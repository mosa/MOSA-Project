// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionI64ToI32
/// </summary>
public sealed class CheckedConversionI64ToI32 : BaseCheckedConversionTransform
{
	public CheckedConversionI64ToI32() : base(IR.CheckedConversionI64ToI32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "I8ToI4");
	}
}
