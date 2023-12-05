// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// Jmp
/// </summary>
public sealed class Jmp : BaseIRTransform
{
	public Jmp() : base(IR.Jmp, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(ARM32.B, ConditionCode.Always, context.BranchTargets[0]);
	}
}
