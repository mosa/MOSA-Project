// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.Stack;

/// <summary>
/// ConvertR4ToI64
/// </summary>
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

		var registerList = Operand.CreateConstant32((1 << (17 - transform.StackFrame.Register.Index)) | (1 << (17 - transform.Architecture.LinkRegister.Index)));

		context.SetInstruction(ARM32.Push, null, registerList);
		context.AppendInstruction(ARM32.Mov, transform.StackFrame, transform.StackPointer);

		if (transform.MethodCompiler.StackSize != 0)
		{
			context.AppendInstruction(ARM32.Sub, transform.StackPointer, transform.StackPointer, Operand.CreateConstant32(-transform.MethodCompiler.StackSize));
		}
	}
}
