// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Stages
{
	/// <summary>
	/// Completes the stack handling after register allocation
	/// </summary>
	public sealed class BuildStackStage : Mosa.Compiler.Framework.Platform.BuildStackStage
	{
		private Operand PushRegisterList;
		private Operand PopRegisterList;

		protected override void Setup()
		{
			base.Setup();

			PushRegisterList = PushRegisterList ?? CreateConstant32((1 << (17 - StackFrame.Register.Index)) | (1 << (17 - LinkRegister.Register.Index)));
			PopRegisterList = PopRegisterList ?? CreateConstant32((1 << (17 - StackFrame.Register.Index)) | (1 << (17 - ProgramCounter.Register.Index)));
		}

		/// <summary>
		/// Adds the prologue instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		protected override void AddPrologueInstructions(Context context)
		{
			context.SetInstruction(ARMv8A32.Push, null, PushRegisterList);
			context.AppendInstruction(ARMv8A32.Mov, StackFrame, StackPointer);

			if (MethodCompiler.StackSize != 0)
			{
				context.AppendInstruction(ARMv8A32.Sub, StackPointer, StackPointer, CreateConstant32(-MethodCompiler.StackSize));
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
				context.AppendInstruction(ARMv8A32.Add, StackPointer, StackFrame, CreateConstant32(-MethodCompiler.StackSize));
			}

			context.AppendInstruction(ARMv8A32.Pop, null, PopRegisterList);

			//context.AppendInstruction(ARMv8A32.Mov, ProgramCounter, LinkRegister);
			//context.AppendInstruction(ARMv8A32.Bx, LinkRegister);
		}
	}
}
