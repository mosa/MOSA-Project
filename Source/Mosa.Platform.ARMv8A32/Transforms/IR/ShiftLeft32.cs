// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// ShiftLeft32
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class ShiftLeft32 : BaseIRTransform
{
	public ShiftLeft32() : base(IRInstruction.ShiftLeft32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Translate(transform, context, ARMv8A32.Lsl, true);
	}
}
