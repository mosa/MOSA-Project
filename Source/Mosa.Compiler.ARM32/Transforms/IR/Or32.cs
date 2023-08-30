// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// Or32
/// </summary>
[Transform("ARM32.IR")]
public sealed class Or32 : BaseIRTransform
{
	public Or32() : base(IRInstruction.Or32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		TransformContext.MoveConstantRight(context);

		Translate(transform, context, ARM32.Orr, true);
	}
}
