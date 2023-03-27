// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// Nop
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class Nop : BaseIRTransform
{
	public Nop() : base(IRInstruction.Nop, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.Empty();
	}
}
