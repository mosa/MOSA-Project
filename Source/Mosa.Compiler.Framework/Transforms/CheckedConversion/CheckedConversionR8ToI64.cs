// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionR8ToI64
/// </summary>
public sealed class CheckedConversionR8ToI64 : BaseCheckedConversionTransform
{
	public CheckedConversionR8ToI64() : base(Framework.IR.CheckedConversionR8ToI64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "R8ToI8");
	}
}
