// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Stack;

[Transform]
public sealed class Prologue : BaseTransform
{
	public Prologue() : base(IR.Prologue, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return transform.MethodCompiler.IsLocalStackFinalized;
	}

	public override void Transform(Context context, Transform transform)
	{
		if (!transform.MethodCompiler.IsStackFrameRequired)
			return;

		context.SetInstruction(X64.Push64, null, transform.StackFrame);
		context.AppendInstruction(X64.Mov64, transform.StackFrame, transform.StackPointer);

		if (transform.MethodCompiler.StackSize != 0)
		{
			context.AppendInstruction(X64.Sub64, transform.StackPointer, transform.StackPointer, Operand.CreateConstant64(-transform.MethodCompiler.StackSize));
		}
	}
}
