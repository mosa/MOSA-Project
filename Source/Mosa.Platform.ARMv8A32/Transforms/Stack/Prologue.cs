// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.Stack;

/// <summary>
/// ConvertR4ToI64
/// </summary>
[Transform("ARMv8A32.Stack")]
public sealed class Prologue : BaseTransform
{
	public Prologue() : base(IRInstruction.Prologue, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return transform.MethodCompiler.IsLocalStackFinalized;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		if (!transform.MethodCompiler.IsStackFrameRequired)
			return;

		var registerList = transform.CreateConstant32((1 << (17 - transform.StackFrame.Register.Index)) | (1 << (17 - transform.Architecture.LinkRegister.Index)));

		context.SetInstruction(ARMv8A32.Push, null, registerList);
		context.AppendInstruction(ARMv8A32.Mov, transform.StackFrame, transform.StackPointer);

		if (transform.MethodCompiler.StackSize != 0)
		{
			context.AppendInstruction(ARMv8A32.Sub, transform.StackPointer, transform.StackPointer, transform.CreateConstant32(-transform.MethodCompiler.StackSize));
		}
	}
}
