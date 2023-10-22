// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// Not32
/// </summary>
[Transform("x86.IR")]
public sealed class Not32 : BaseIRTransform
{
	public Not32() : base(IRInstruction.Not32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X86.Not32, context.Result, context.Operand1);
	}
}
