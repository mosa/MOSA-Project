// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// Neg32
/// </summary>
[Transform("x86.IR")]
public sealed class Neg32 : BaseIRTransform
{
	public Neg32() : base(IRInstruction.Neg32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X86.Neg32);
	}
}
