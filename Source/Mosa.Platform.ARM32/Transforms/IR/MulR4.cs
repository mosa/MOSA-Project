// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// MulR4
/// </summary>
[Transform("ARM32.IR")]
public sealed class MulR4 : BaseIRTransform
{
	public MulR4() : base(IRInstruction.MulR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		TransformContext.MoveConstantRight(context);

		Translate(transform, context, ARM32.Muf, true);
	}
}
