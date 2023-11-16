// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// Jmp
/// </summary>
[Transform]
public sealed class Jmp : BaseIRTransform
{
	public Jmp() : base(IR.Jmp, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X86.Jmp);
	}
}
