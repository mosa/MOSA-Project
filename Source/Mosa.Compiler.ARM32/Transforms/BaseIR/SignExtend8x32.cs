// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// SignExtend8x32
/// </summary>
public sealed class SignExtend8x32 : BaseIRTransform
{
	public static readonly SignExtend8x32 Instance = new();

	private SignExtend8x32() : base(IR.SignExtend8x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Translate(transform, context, ARM32.Sxth, false);
	}
}
