// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// LoadParam32
/// </summary>
[Transform("x86.BaseIR")]
public sealed class LoadParam32 : BaseIRTransform
{
	public LoadParam32() : base(IR.LoadParam32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X86.MovLoad32, context.Result, transform.StackFrame, context.Operand1);
	}
}
