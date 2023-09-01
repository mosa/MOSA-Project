// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// AddR8
/// </summary>
[Transform("ARM32.IR")]
public sealed class AddR8 : BaseIRTransform
{
	public AddR8() : base(IRInstruction.AddR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		TransformContext.MoveConstantRight(context);

		Translate(transform, context, ARM32.Adf, true);
	}
}
