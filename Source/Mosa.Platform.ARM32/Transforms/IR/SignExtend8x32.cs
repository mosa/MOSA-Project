// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARM32.Transforms.IR;

/// <summary>
/// SignExtend8x32
/// </summary>
[Transform("ARM32.IR")]
public sealed class SignExtend8x32 : BaseIRTransform
{
	public SignExtend8x32() : base(IRInstruction.SignExtend8x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Translate(transform, context, ARM32.Sxth, false);
	}
}
