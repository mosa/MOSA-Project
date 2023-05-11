// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// MulR4
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class MulR4 : BaseIRTransform
{
	public MulR4() : base(IRInstruction.MulR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		TransformContext.MoveConstantRight(context);

		Translate(transform, context, ARMv8A32.Muf, true);
	}
}
