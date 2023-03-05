// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.Stack;

/// <summary>
/// ConvertR4ToI64
/// </summary>
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
		{
			context.SetNop();
			return;
		}

		context.SetInstruction(X86.Push32, null, transform.StackFrame);
		context.AppendInstruction(X86.Mov32, transform.StackFrame, transform.StackPointer);

		if (transform.MethodCompiler.StackSize != 0)
		{
			context.AppendInstruction(X86.Sub32, transform.StackPointer, transform.StackPointer, transform.CreateConstant32(-transform.MethodCompiler.StackSize));
		}
	}
}
