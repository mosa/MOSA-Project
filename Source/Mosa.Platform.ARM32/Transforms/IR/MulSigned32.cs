// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARM32.Transforms.IR;

/// <summary>
/// MulSigned32
/// </summary>
[Transform("ARM32.IR")]
public sealed class MulSigned32 : BaseIRTransform
{
	public MulSigned32() : base(IRInstruction.MulSigned32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		TransformContext.MoveConstantRight(context);

		Translate(transform, context, ARM32.Mul, false);
	}
}
