// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.Stack;

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
			return;

		context.SetInstruction(X64.Push64, null, transform.StackFrame);
		context.AppendInstruction(X64.Mov64, transform.StackFrame, transform.StackPointer);

		if (transform.MethodCompiler.StackSize != 0)
		{
			context.AppendInstruction(X64.Sub64, transform.StackPointer, transform.StackPointer, transform.CreateConstant64(-transform.MethodCompiler.StackSize));
		}
	}
}
