// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// Move32
/// </summary>
[Transform("x86.IR")]
public sealed class Move32 : BaseIRTransform
{
	public Move32() : base(IRInstruction.Move32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X86.Mov32);
	}
}
