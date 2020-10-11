// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Stages
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
			context.SetInstruction(ARMv8A32.Push, null, StackPointer, CreateConstant((1 << (17 - StackFrame.Register.Index)) | (1 << (17 - LinkRegister.Register.Index))));
			context.AppendInstruction(ARMv8A32.Mov, StackFrame, StackPointer);

			if (MethodCompiler.StackSize != 0)
			{
				context.AppendInstruction(ARMv8A32.Sub, StackPointer, StackPointer, CreateConstant(-MethodCompiler.StackSize));
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
				context.AppendInstruction(ARMv8A32.Add, StackPointer, StackFrame, CreateConstant(-MethodCompiler.StackSize));
			}

			context.AppendInstruction(ARMv8A32.Pop, null, StackPointer, CreateConstant((1 << (17 - StackFrame.Register.Index)) | (1 << (17 - LinkRegister.Register.Index))));
			context.AppendInstruction(ARMv8A32.Mov, ProgramCounter, LinkRegister);
		}
	}
}
