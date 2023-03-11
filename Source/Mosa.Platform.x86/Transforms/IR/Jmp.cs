// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// Jmp
/// </summary>
public sealed class Jmp : BaseIRTransform
{
	public Jmp() : base(IRInstruction.Jmp, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X86.Jmp);
	}
}
