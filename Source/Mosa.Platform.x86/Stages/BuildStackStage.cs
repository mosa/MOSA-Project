// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Completes the stack handling after register allocation
	/// </summary>
	public sealed class BuildStackStage : Mosa.Compiler.Framework.Platform.BuildStackStage
	{
		/// <summary>
		/// Adds the prologue instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		protected override void AddPrologueInstructions(Context context)
		{
			context.SetInstruction(X86.Push32, null, StackFrame);
			context.AppendInstruction(X86.Mov32, StackFrame, StackPointer);

			if (MethodCompiler.StackSize != 0)
			{
				context.AppendInstruction(X86.Sub32, StackPointer, StackPointer, CreateConstant32(-MethodCompiler.StackSize));
			}
		}

		/// <summary>
		/// Adds the epilogue instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		protected override void AddEpilogueInstructions(Context context)
		{
			context.Empty();

			if (MethodCompiler.StackSize != 0)
			{
				context.AppendInstruction(X86.Add32, StackPointer, StackPointer, CreateConstant32(-MethodCompiler.StackSize));
			}

			context.AppendInstruction(X86.Pop32, StackFrame);
			context.AppendInstruction(X86.Ret);
		}
	}
}
