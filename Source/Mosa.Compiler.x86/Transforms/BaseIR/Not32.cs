// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// Not32
/// </summary>
[Transform]
public sealed class Not32 : BaseIRTransform
{
	public Not32() : base(IR.Not32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X86.Not32, context.Result, context.Operand1);
	}
}
