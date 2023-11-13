// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Stack;

/// <summary>
/// Epilogue
/// </summary>
[Transform("x64.Stack")]
public sealed class Epilogue : BaseTransform
{
	public Epilogue() : base(IR.Epilogue, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return transform.MethodCompiler.IsLocalStackFinalized;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.Empty();

		if (!transform.MethodCompiler.IsStackFrameRequired)
			return;

		if (transform.MethodCompiler.StackSize != 0)
		{
			context.AppendInstruction(X64.Add64, transform.StackPointer, transform.StackPointer, Operand.CreateConstant64(-transform.MethodCompiler.StackSize));
		}

		context.AppendInstruction(X64.Pop64, transform.StackFrame);
		context.AppendInstruction(X64.Ret);
	}
}
