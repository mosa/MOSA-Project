// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.Stack
{
	/// <summary>
	/// ConvertR4ToI64
	/// </summary>
	public sealed class Epilogue : BaseTransformation
	{
		public Epilogue() : base(IRInstruction.Epilogue, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.Empty();

			if (!transform.MethodCompiler.IsStackFrameRequired)
				return;

			if (transform.MethodCompiler.StackSize != 0)
			{
				context.AppendInstruction(X86.Add32, transform.Compiler.StackPointer, transform.Compiler.StackPointer, transform.CreateConstant32(-transform.MethodCompiler.StackSize));
			}

			context.AppendInstruction(X86.Pop32, transform.Compiler.StackFrame);
			context.AppendInstruction(X86.Ret);
		}
	}
}
