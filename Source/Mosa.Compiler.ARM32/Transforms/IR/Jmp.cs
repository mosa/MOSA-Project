// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// Jmp
/// </summary>
[Transform("ARM32.IR")]
public sealed class Jmp : BaseIRTransform
{
	public Jmp() : base(Framework.IR.Jmp, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(ARM32.B);
		context.ConditionCode = ConditionCode.Always;
	}
}
