// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// Move32
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class Move32 : BaseIRTransform
{
	public Move32() : base(IRInstruction.Move32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Translate(transform, context, ARMv8A32.Mov, true);
	}
}
