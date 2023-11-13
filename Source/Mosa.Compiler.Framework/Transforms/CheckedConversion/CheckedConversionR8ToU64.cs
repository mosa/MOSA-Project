// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionR8ToU64
/// </summary>
public sealed class CheckedConversionR8ToU64 : BaseCheckedConversionTransform
{
	public CheckedConversionR8ToU64() : base(IR.CheckedConversionR8ToU64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "R8ToU8");
	}
}
