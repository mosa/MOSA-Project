// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// MoveR8
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class MoveR8 : BaseIRTransform
{
	public MoveR8() : base(IRInstruction.MoveR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Translate(transform, context, ARMv8A32.Mvf, true);
	}
}
