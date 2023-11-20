// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionR8ToI8
/// </summary>
public sealed class CheckedConversionR8ToI8 : BaseCheckedConversionTransform
{
	public CheckedConversionR8ToI8() : base(IR.CheckedConversionR8ToI8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "R8ToI1");
	}
}
