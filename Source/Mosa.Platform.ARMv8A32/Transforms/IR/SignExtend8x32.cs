// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// SignExtend8x32
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class SignExtend8x32 : BaseIRTransform
{
	public SignExtend8x32() : base(IRInstruction.SignExtend8x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Translate(transform, context, ARMv8A32.Sxth, false);
	}
}
