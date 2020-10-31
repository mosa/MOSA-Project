// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Stages
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
			context.SetInstruction(X64.Push64, null, StackFrame);
			context.AppendInstruction(X64.Mov64, StackFrame, StackPointer);

			if (MethodCompiler.StackSize != 0)
			{
				context.AppendInstruction(X64.Sub64, StackPointer, StackPointer, CreateConstant32(-MethodCompiler.StackSize));
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
				context.AppendInstruction(X64.Add64, StackPointer, StackPointer, CreateConstant32(-MethodCompiler.StackSize));
			}

			context.AppendInstruction(X64.Pop64, StackFrame);
			context.AppendInstruction(X64.Ret);
		}
	}
}
