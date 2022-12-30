// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.RuntimeCall
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
			if (!transform.MethodCompiler.IsStackFrameRequired)
				return;

			context.SetInstruction(X86.Push32, null, transform.Compiler.StackFrame);
			context.AppendInstruction(X86.Mov32, transform.Compiler.StackFrame, transform.Compiler.StackPointer);

			if (transform.MethodCompiler.StackSize != 0)
			{
				context.AppendInstruction(X86.Sub32, transform.Compiler.StackPointer, transform.Compiler.StackPointer, transform.CreateConstant32(-transform.MethodCompiler.StackSize));
			}
		}
	}
}
