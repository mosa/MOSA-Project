// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// Xor32
/// </summary>
[Transform("x86.IR")]
public sealed class Xor32 : BaseIRTransform
{
	public Xor32() : base(IRInstruction.Xor32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X86.Xor32);
	}
}
