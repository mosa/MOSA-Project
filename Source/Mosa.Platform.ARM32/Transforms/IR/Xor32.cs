// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARM32.Transforms.IR;

/// <summary>
/// Xor32
/// </summary>
[Transform("ARM32.IR")]
public sealed class Xor32 : BaseIRTransform
{
	public Xor32() : base(IRInstruction.Xor32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		TransformContext.MoveConstantRight(context);

		Translate(transform, context, ARM32.Eor, true);
	}
}
