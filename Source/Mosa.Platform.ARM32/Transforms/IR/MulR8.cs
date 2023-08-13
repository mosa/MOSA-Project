// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARM32.Transforms.IR;

/// <summary>
/// MulR8
/// </summary>
[Transform("ARM32.IR")]
public sealed class MulR8 : BaseIRTransform
{
	public MulR8() : base(IRInstruction.MulR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		TransformContext.MoveConstantRight(context);

		Translate(transform, context, ARM32.Muf, true);
	}
}
