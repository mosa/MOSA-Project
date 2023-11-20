// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionR4ToU16
/// </summary>
public sealed class CheckedConversionR4ToU16 : BaseCheckedConversionTransform
{
	public CheckedConversionR4ToU16() : base(IR.CheckedConversionR4ToU16, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "R4ToU2");
	}
}
