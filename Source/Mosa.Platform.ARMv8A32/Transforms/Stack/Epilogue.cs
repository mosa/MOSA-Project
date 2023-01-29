// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.Stack;

/// <summary>
/// Epilogue
/// </summary>
public sealed class Epilogue : BaseTransform
{
	public Epilogue() : base(IRInstruction.Epilogue, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return transform.MethodCompiler.IsLocalStackFinalized;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.Empty();

		if (!transform.MethodCompiler.IsStackFrameRequired)
			return;

		var registerList = transform.CreateConstant32((1 << (17 - transform.StackFrame.Register.Index)) | (1 << (17 - transform.Architecture.ProgramCounter.Index)));

		context.Empty();

		if (transform.MethodCompiler.StackSize != 0)
		{
			context.AppendInstruction(ARMv8A32.Add, transform.StackPointer, transform.StackFrame, transform.CreateConstant32(-transform.MethodCompiler.StackSize));
		}

		context.AppendInstruction(ARMv8A32.Pop, null, registerList);
	}
}
