// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionR8ToU32
/// </summary>
public sealed class CheckedConversionR8ToU32 : BaseCheckedConversionTransform
{
	public CheckedConversionR8ToU32() : base(IR.CheckedConversionR8ToU32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "R8ToU4");
	}
}
