// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// Neg32
/// </summary>
[Transform("x86.BaseIR")]
public sealed class Neg32 : BaseIRTransform
{
	public Neg32() : base(IR.Neg32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X86.Neg32);
	}
}
