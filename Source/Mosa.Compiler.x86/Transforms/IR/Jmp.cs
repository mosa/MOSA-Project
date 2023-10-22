// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// Jmp
/// </summary>
[Transform("x86.IR")]
public sealed class Jmp : BaseIRTransform
{
	public Jmp() : base(IRInstruction.Jmp, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X86.Jmp);
	}
}
