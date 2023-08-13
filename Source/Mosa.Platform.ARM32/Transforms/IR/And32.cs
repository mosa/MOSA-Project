// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARM32.Transforms.IR;

/// <summary>
/// And32
/// </summary>
[Transform("ARM32.IR")]
public sealed class And32 : BaseIRTransform
{
	public And32() : base(IRInstruction.And32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		TransformContext.MoveConstantRight(context);

		Translate(transform, context, ARM32.And, true);
	}
}
