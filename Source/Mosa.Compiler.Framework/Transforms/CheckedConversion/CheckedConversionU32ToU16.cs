// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionU32ToU16
/// </summary>
public sealed class CheckedConversionU32ToU16 : BaseCheckedConversionTransform
{
	public static readonly CheckedConversionU32ToU16 Instance = new();

	private CheckedConversionU32ToU16() : base(IR.CheckedConversionU32ToU16, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		CallCheckOverflow(transform, context, "U4ToU2");
	}
}
