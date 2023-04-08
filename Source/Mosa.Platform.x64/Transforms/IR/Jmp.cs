// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// Jmp
/// </summary>
[Transform("x64.IR")]
public sealed class Jmp : BaseIRTransform
{
	public Jmp() : base(IRInstruction.Jmp, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X64.Jmp);
	}
}
