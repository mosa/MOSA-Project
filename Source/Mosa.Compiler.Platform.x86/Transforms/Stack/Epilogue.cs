// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.Stack
{
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

			if (transform.MethodCompiler.StackSize != 0)
			{
				context.AppendInstruction(X86.Add32, transform.StackPointer, transform.StackPointer, transform.CreateConstant32(-transform.MethodCompiler.StackSize));
			}

			context.AppendInstruction(X86.Pop32, transform.StackFrame);
			context.AppendInstruction(X86.Ret);
		}
	}
}
