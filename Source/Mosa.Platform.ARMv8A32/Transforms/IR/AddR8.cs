// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// AddR8
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class AddR8 : BaseIRTransform
{
	public AddR8() : base(IRInstruction.AddR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.MoveConstantRight(context);

		Translate(transform, context, ARMv8A32.Adf, true);
	}
}
