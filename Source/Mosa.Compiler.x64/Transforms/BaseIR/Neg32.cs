// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// Neg32
/// </summary>
[Transform]
public sealed class Neg32 : BaseIRTransform
{
	public Neg32() : base(IR.Neg32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Neg32);
	}
}
