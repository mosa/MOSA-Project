// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionR8ToU16
/// </summary>
public sealed class CheckedConversionR8ToU16 : BaseCheckedConversionTransform
{
	public CheckedConversionR8ToU16() : base(IR.CheckedConversionR8ToU16, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "R8ToU2");
	}
}
