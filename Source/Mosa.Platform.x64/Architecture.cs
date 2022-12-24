// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.CompilerStages;
using Mosa.Compiler.Framework.Linker.Elf;
using Mosa.Compiler.Framework.Stages;
using Mosa.Platform.Intel;
using Mosa.Platform.x64.CompilerStages;
using Mosa.Platform.x64.Stages;
using System.Collections.Generic;

namespace Mosa.Platform.x64
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
		public override MachineType ElfMachineType
		{ get { return MachineType.Intel386; } }

		/// <summary>
		/// Defines the register set of the target architecture.
		/// </summary>
		private static readonly PhysicalRegister[] Registers = new PhysicalRegister[]
		{
			////////////////////////////////////////////////////////
			// 32-bit general purpose registers
			////////////////////////////////////////////////////////
			CPURegister.EAX,
			CPURegister.ECX,
			CPURegister.EDX,
			CPURegister.EBX,
			CPURegister.ESP,
			CPURegister.EBP,
			CPURegister.ESI,
			CPURegister.EDI,

			CPURegister.R8,
			CPURegister.R9,
			CPURegister.R10,
			CPURegister.R11,
			CPURegister.R12,
			CPURegister.R13,
			CPURegister.R14,
			CPURegister.R15,

			////////////////////////////////////////////////////////
			// SSE 128-bit floating point registers
			////////////////////////////////////////////////////////
			CPURegister.XMM0,
			CPURegister.XMM1,
			CPURegister.XMM2,
			CPURegister.XMM3,
			CPURegister.XMM4,
			CPURegister.XMM5,
			CPURegister.XMM6,
			CPURegister.XMM7,

			CPURegister.XMM8,
			CPURegister.XMM9,
			CPURegister.XMM10,
			CPURegister.XMM11,
			CPURegister.XMM12,
			CPURegister.XMM13,
			CPURegister.XMM14,
			CPURegister.XMM15
		};

		/// <summary>
		/// Gets the native size of architecture in bytes.
		/// </summary>
		/// <value>This property always returns 8.</value>
		public override uint NativePointerSize
		{ get { return 8; } }

		/// <summary>
		/// Retrieves the register set of the x64 platform.
		/// </summary>
		public override PhysicalRegister[] RegisterSet
		{ get { return Registers; } }

		/// <summary>
		/// Retrieves the stack frame register of the x86.
		/// </summary>
		public override PhysicalRegister StackFrameRegister
		{ get { return CPURegister.EBP; } }

		/// <summary>
		/// Retrieves the stack pointer register of the x86.
		/// </summary>
		public override PhysicalRegister StackPointerRegister
		{ get { return CPURegister.ESP; } }

		/// <summary>
		/// Gets the return register.
		/// </summary>
		public override PhysicalRegister ReturnRegister
		{ get { return CPURegister.EAX; } }

		public override PhysicalRegister LinkRegister
		{ get { return null; } }

		/// <summary>
		/// Gets the return register for the high portion of the 64bit result.
		/// </summary>
		public override PhysicalRegister ReturnHighRegister
		{ get { return null; } }

		/// <summary>
		/// Gets the return floating point register.
		/// </summary>
		public override PhysicalRegister ReturnFloatingPointRegister
		{ get { return CPURegister.XMM0; } }

		/// <summary>
		/// Retrieves the exception register of the architecture.
		/// </summary>
		public override PhysicalRegister ExceptionRegister
		{ get { return CPURegister.EDI; } }

		/// <summary>
		/// Gets the finally return block register.
		/// </summary>
		public override PhysicalRegister LeaveTargetRegister
		{ get { return CPURegister.ESI; } }

		/// <summary>
		/// Retrieves the program counter register of the x86.
		/// </summary>
		public override PhysicalRegister ProgramCounter
		{ get { return null; } }

		/// <summary>
		/// Gets the name of the platform.
		/// </summary>
		public override string PlatformName
		{ get { return "x64"; } }

		/// <summary>
		/// Gets the instructions.
		/// </summary>
		public override List<BaseInstruction> Instructions
		{ get { return X64Instructions.List; } }

		public override OpcodeEncoder GetOpcodeEncoder()
		{
			return new OpcodeEncoder(8);
		}

		/// <summary>
		/// Extends the compiler pipeline with x64 specific stages.
		/// </summary>
		/// <param name="pipeline">The pipeline to extend.</param>
		public override void ExtendCompilerPipeline(Pipeline<BaseCompilerStage> pipeline, CompilerSettings compilerSettings)
		{
			if (compilerSettings.Settings.GetValue("Multiboot.Version", string.Empty).ToLowerInvariant() == "v1")
			{
				pipeline.InsertAfterFirst<TypeInitializerStage>(
					new MultibootV1Stage()
				);
			}

			pipeline.Add(
				new Intel.CompilerStages.StartUpStage()
			);
		}

		/// <summary>
		/// Extends the method compiler pipeline with x64 specific stages.</summary>
		/// <param name="pipeline">The method compiler pipeline to extend.</param>
		/// <param name="compilerSettings"></param>
		public override void ExtendMethodCompilerPipeline(Pipeline<BaseMethodCompilerStage> pipeline, CompilerSettings compilerSettings)
		{
			pipeline.InsertBefore<CallStage>(
				new Stages.RuntimeCallStage()
			);

			pipeline.InsertAfterLast<PlatformIntrinsicStage>(
				new BaseMethodCompilerStage[]
				{
					new IRTransformationStage(),
					compilerSettings.PlatformOptimizations ? new Stages.OptimizationStage() : null,
					new TweakStage(),
					new FixedRegisterAssignmentStage(),
					compilerSettings.PlatformOptimizations ? new SimpleDeadCodeRemovalStage() : null,
					new AddressModeConversionStage(),
				});

			pipeline.InsertAfterLast<StackLayoutStage>(
				new BuildStackStage()
			);

			pipeline.InsertBefore<CodeGenerationStage>(
				new BaseMethodCompilerStage[]
				{
					new FinalTweakStage(),
					compilerSettings.PlatformOptimizations ? new PostOptimizationStage() : null,
					new JumpOptimizationStage()
				});
		}

		/// <summary>
		/// Inserts the move instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source.</param>
		public override void InsertMoveInstruction(Context context, Operand destination, Operand source)
		{
			BaseInstruction instruction = X64.Mov64;

			if (destination.IsR4)
			{
				instruction = X64.Movss;
			}
			else if (destination.IsR8)
			{
				instruction = X64.Movsd;
			}

			context.AppendInstruction(instruction, destination, source);
		}

		/// <summary>
		/// Inserts the store instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		/// <exception cref="NotImplementCompilerException"></exception>
		public override void InsertStoreInstruction(Context context, Operand destination, Operand offset, Operand value)
		{
			BaseInstruction instruction = X64.MovStore32;

			if (value.IsR4)
			{
				instruction = X64.MovssStore;
			}
			else if (value.IsR8)
			{
				instruction = X64.MovsdStore;
			}

			context.AppendInstruction(instruction, null, destination, offset, value);
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
			BaseInstruction instruction = X64.MovLoad32;

			if (destination.IsR4)
			{
				instruction = X64.MovssLoad;
			}
			else if (destination.IsR8)
			{
				instruction = X64.MovsdLoad;
			}

			context.AppendInstruction(instruction, destination, source, offset);
		}

		/// <summary>
		/// Inserts the exchange instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source.</param>
		public override void InsertExchangeInstruction(Context context, Operand destination, Operand source)
		{
			if (source.IsR4)
			{
				// TODO
				throw new CompilerException("R4 not implemented in InsertExchangeInstruction method");
			}
			else if (source.IsR8)
			{
				// TODO
				throw new CompilerException("R8 not implemented in InsertExchangeInstruction method");
			}
			else
			{
				context.AppendInstruction2(X64.XChg64, destination, source, source, destination);
			}
		}

		/// <summary>
		/// Inserts the jump instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		public override void InsertJumpInstruction(Context context, BasicBlock destination)
		{
			context.AppendInstruction(X64.Jmp, destination);
		}

		/// <summary>
		/// Determines whether [is instruction move] [the specified instruction].
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns></returns>
		public override bool IsInstructionMove(BaseInstruction instruction)
		{
			return instruction == X64.Mov64 || instruction == X64.Mov32 || instruction == X64.Movsd || instruction == X64.Movss;
		}
	}
}
