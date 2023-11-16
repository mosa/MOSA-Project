// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.Stack;

/// <summary>
/// Epilogue
/// </summary>
[Transform]
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

		var registerList = Operand.CreateConstant32((1 << (17 - transform.StackFrame.Register.Index)) | (1 << (17 - transform.Architecture.ProgramCounter.Index)));

		context.Empty();

		if (transform.MethodCompiler.StackSize != 0)
		{
			context.AppendInstruction(ARM32.Add, transform.StackPointer, transform.StackFrame, Operand.CreateConstant32(-transform.MethodCompiler.StackSize));
		}

		context.AppendInstruction(ARM32.Pop, null, registerList);
	}
}
