// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// Or32
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class Or32 : BaseIRTransform
{
	public Or32() : base(IRInstruction.Or32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.MoveConstantRight(context);

		Translate(transform, context, ARMv8A32.Orr, true);
	}
}
