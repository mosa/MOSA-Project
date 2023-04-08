// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// Nop
/// </summary>
[Transform("x64.IR")]
public sealed class Nop : BaseIRTransform
{
	public Nop() : base(IRInstruction.Nop, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.SetInstruction(X64.Nop);
	}
}
