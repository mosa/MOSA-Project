// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionR4ToI32
/// </summary>
public sealed class CheckedConversionR4ToI32 : BaseCheckedConversionTransform
{
	public CheckedConversionR4ToI32() : base(IR.CheckedConversionR4ToI32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "R4ToI4");
	}
}
