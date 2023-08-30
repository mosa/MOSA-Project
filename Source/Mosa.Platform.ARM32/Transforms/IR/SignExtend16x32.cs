// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// SignExtend16x32
/// </summary>
[Transform("ARM32.IR")]
public sealed class SignExtend16x32 : BaseIRTransform
{
	public SignExtend16x32() : base(IRInstruction.SignExtend16x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Translate(transform, context, ARM32.Sxth, false);
	}
}
