// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker.Elf;
using Mosa.Compiler.Framework.Stages;
using Mosa.Platform.ARMv8A32.Stages;
using System.Collections.Generic;

namespace Mosa.Platform.ARMv8A32
{
	/// <summary>
	/// This class provides a common base class for architecture
	/// specific operations.
	/// </summary>
	public sealed class Architecture : BaseArchitecture
	{
		/// <summary>
		/// Gets the type of the elf machine.
		/// </summary>
		public override MachineType ElfMachineType { get { return MachineType.ARM; } }

		/// <summary>
		/// Gets the name of the platform.
		/// </summary>
		public override string PlatformName { get { return "ARMv8A32"; } }

		/// <summary>
		/// Gets the register set of the architecture.
		/// </summary>
		private static readonly PhysicalRegister[] registers = new PhysicalRegister[]
		{
			////////////////////////////////////////////////////////
			// 32-bit general purpose registers
			////////////////////////////////////////////////////////
			GeneralPurposeRegister.R0,
			GeneralPurposeRegister.R1,
			GeneralPurposeRegister.R2,
			GeneralPurposeRegister.R3,
			GeneralPurposeRegister.R4,
			GeneralPurposeRegister.R5,
			GeneralPurposeRegister.R6,
			GeneralPurposeRegister.R7,
			GeneralPurposeRegister.R8,
			GeneralPurposeRegister.R9,
			GeneralPurposeRegister.R10,
			GeneralPurposeRegister.R11,
			GeneralPurposeRegister.R12,
			GeneralPurposeRegister.SP,
			GeneralPurposeRegister.LR,
			GeneralPurposeRegister.PC
		};

		/// <summary>
		/// Gets the native size of architecture in bytes.
		/// </summary>
		/// <value>This property always returns 4.</value>
		public override int NativePointerSize { get { return 4; } }

		/// <summary>
		/// Retrieves the register set of the ARMv8A32 platform.
		/// </summary>
		public override PhysicalRegister[] RegisterSet { get { return registers; } }

		/// <summary>
		/// Retrieves the stack frame register of the ARMv8A32.
		/// </summary>
		public override PhysicalRegister StackFrameRegister { get { return GeneralPurposeRegister.LR; } }

		/// <summary>
		/// Retrieves the stack pointer register of the ARMv8A32.
		/// </summary>
		public override PhysicalRegister StackPointerRegister { get { return GeneralPurposeRegister.SP; } }

		/// <summary>
		/// Retrieves the scratch register of the ARMv8A32.
		/// </summary>
		public override PhysicalRegister ScratchRegister { get { return null; /* TODO */} }

		/// <summary>
		/// Gets the return register.
		/// </summary>
		public override PhysicalRegister ReturnRegister { get { return GeneralPurposeRegister.R0; } }

		/// <summary>
		/// Gets the return register for the high portion of the 64bit result.
		/// </summary>
		public override PhysicalRegister ReturnHighRegister { get { return GeneralPurposeRegister.R1; } }

		/// <summary>
		/// Gets the return floating point register.
		/// </summary>
		public override PhysicalRegister ReturnFloatingPointRegister { get { return null; /* TODO */} }

		/// <summary>
		/// Retrieves the exception register of the architecture.
		/// </summary>
		public override PhysicalRegister ExceptionRegister { get { return GeneralPurposeRegister.R10; } }

		/// <summary>
		/// Gets the finally return block register.
		/// </summary>
		public override PhysicalRegister LeaveTargetRegister { get { return GeneralPurposeRegister.R9; } }

		/// <summary>
		/// Retrieves the program counter register of the ARMv8A32.
		/// </summary>
		public override PhysicalRegister ProgramCounter { get { return GeneralPurposeRegister.PC; } }

		/// <summary>
		/// Gets the instructions.
		/// </summary>
		/// <value>
		/// The instructions.
		/// </value>
		public override List<BaseInstruction> Instructions { get { return ARMv8A32Instructions.List; } }

		/// <summary>
		/// Extends the compiler pipeline with ARMv8A32 specific stages.
		/// </summary>
		/// <param name="pipeline">The pipeline to extend.</param>
		public override void ExtendCompilerPipeline(Pipeline<BaseCompilerStage> pipeline, CompilerSettings compilerSettings)
		{
		}

		/// <summary>
		/// Extends the method compiler pipeline with ARMv8A32 specific stages.
		/// </summary>
		/// <param name="pipeline">The method compiler pipeline to extend.</param>
		/// <param name="compilerSettings">The compiler options.</param>
		public override void ExtendMethodCompilerPipeline(Pipeline<BaseMethodCompilerStage> pipeline, CompilerSettings compilerSettings)
		{
			pipeline.InsertBefore<GreedyRegisterAllocatorStage>(
				new StopStage()
			);

			pipeline.InsertBefore<CallStage>(
				new Stages.RuntimeCallStage()
			);

			pipeline.InsertAfterLast<PlatformIntrinsicStage>(
				new BaseMethodCompilerStage[]
				{
					new LongOperandStage(),
					new IRTransformationStage(),

			//		compilerSettings.EnablePlatformOptimizations ? new OptimizationStage() : null,
			//		new TweakStage(),
			//		new FixedRegisterAssignmentStage(),
			//		new SimpleDeadCodeRemovalStage(),
			//		new AddressModeConversionStage(),
			//		new FloatingPointStage(),
				});

			//pipeline.InsertAfterLast<StackLayoutStage>(
			//	new BuildStackStage()
			//);

			//pipeline.InsertBefore<CodeGenerationStage>(
			//	new BaseMethodCompilerStage[]
			//	{
			//		new FinalTweakStage(),
			//		compilerSettings.EnablePlatformOptimizations ? new PostOptimizationStage() : null,
			//	});

			//pipeline.InsertBefore<CodeGenerationStage>(
			//	new JumpOptimizationStage()
			//);
		}

		/// <summary>
		/// Create platform move.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source.</param>
		/// <exception cref="NotImplementCompilerException"></exception>
		public override void InsertMoveInstruction(Context context, Operand destination, Operand source)
		{
			throw new NotImplementCompilerException();
		}

		public override void InsertStoreInstruction(Context context, Operand destination, Operand offset, Operand value)
		{
			throw new NotImplementCompilerException();
		}

		/// <summary>
		/// Inserts the load instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source.</param>
		/// <param name="offset">The offset.</param>
		/// <exception cref="NotImplementCompilerException"></exception>
		public override void InsertLoadInstruction(Context context, Operand destination, Operand source, Operand offset)
		{
			throw new NotImplementCompilerException();
		}

		/// <summary>
		/// Creates the swap.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source.</param>
		public override void InsertExchangeInstruction(Context context, Operand destination, Operand source)
		{
			// TODO
		}

		/// <summary>
		/// Inserts the jump instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		public override void InsertJumpInstruction(Context context, BasicBlock destination)
		{
			// TODO
		}

		/// <summary>
		/// Determines whether [is instruction move] [the specified instruction].
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns></returns>
		public override bool IsInstructionMove(BaseInstruction instruction)
		{
			return instruction == ARMv8A32.Mov;
		}
	}
}
