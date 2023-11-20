// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionR8ToI64
/// </summary>
public sealed class CheckedConversionR8ToI64 : BaseCheckedConversionTransform
{
	public CheckedConversionR8ToI64() : base(IR.CheckedConversionR8ToI64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "R8ToI8");
	}
}
