// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Stack;

/// <summary>
/// Epilogue
/// </summary>
[Transform("x86.Stack")]
public sealed class Epilogue : BaseTransform
{
	public Epilogue() : base(Framework.IR.Epilogue, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return transform.MethodCompiler.IsLocalStackFinalized;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetNop();

		if (!transform.MethodCompiler.IsStackFrameRequired)
			return;

		if (transform.MethodCompiler.StackSize != 0)
		{
			context.AppendInstruction(X86.Add32, transform.StackPointer, transform.StackPointer, Operand.CreateConstant32(-transform.MethodCompiler.StackSize));
		}

		context.AppendInstruction(X86.Pop32, transform.StackFrame);
		context.AppendInstruction(X86.Ret);
	}
}
