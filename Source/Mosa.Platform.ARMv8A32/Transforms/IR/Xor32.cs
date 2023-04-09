// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// Xor32
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class Xor32 : BaseIRTransform
{
	public Xor32() : base(IRInstruction.Xor32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		TransformContext.MoveConstantRight(context);

		Translate(transform, context, ARMv8A32.Eor, true);
	}
}
