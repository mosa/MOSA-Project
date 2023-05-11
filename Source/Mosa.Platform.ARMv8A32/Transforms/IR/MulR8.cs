// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// MulR8
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class MulR8 : BaseIRTransform
{
	public MulR8() : base(IRInstruction.MulR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		TransformContext.MoveConstantRight(context);

		Translate(transform, context, ARMv8A32.Muf, true);
	}
}
