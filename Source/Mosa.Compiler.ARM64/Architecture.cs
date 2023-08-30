// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker.Elf;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.ARM64.Stages;
using Mosa.Utility.Configuration;

namespace Mosa.Compiler.ARM64;

/// <summary>
/// This class provides a common base class for architecture
/// specific operations.
/// </summary>
public sealed class Architecture : BaseArchitecture
{
	/// <summary>
	/// Gets the type of the elf machine.
	/// </summary>
	public override MachineType ElfMachineType => MachineType.ARM;  // FIXME

	/// <summary>
	/// Gets the name of the platform.
	/// </summary>
	public override string PlatformName => "ARM64";

	/// <summary>
	/// Gets the register set of the architecture.
	/// </summary>
	private static readonly PhysicalRegister[] registers = new PhysicalRegister[]
	{
		////////////////////////////////////////////////////////
		// 64-bit general purpose registers
		////////////////////////////////////////////////////////
		CPURegister.R0,
		CPURegister.R1,
		CPURegister.R2,
		CPURegister.R3,
		CPURegister.R4,
		CPURegister.R5,
		CPURegister.R6,
		CPURegister.R7,
		CPURegister.R8,
		CPURegister.R9,
		CPURegister.R10,
		CPURegister.FP,
		CPURegister.R12,
		CPURegister.SP,
		CPURegister.LR,
		CPURegister.PC,

		////////////////////////////////////////////////////////
		// Floating Point 128-bit floating point registers
		////////////////////////////////////////////////////////
		CPURegister.d0,
		CPURegister.d1,
		CPURegister.d2,
		CPURegister.d3,
		CPURegister.d4,
		CPURegister.d5,
		CPURegister.d6,
		CPURegister.d7
	};

	/// <summary>
	/// Gets the native size of architecture in bytes.
	/// </summary>
	/// <value>This property always returns 8.</value>
	public override uint NativePointerSize => 8;

	/// <summary>
	/// Retrieves the register set of the ARM64 platform.
	/// </summary>
	public override PhysicalRegister[] RegisterSet => registers;

	/// <summary>
	/// Retrieves the stack frame register of the ARM64.
	/// </summary>
	public override PhysicalRegister StackFrameRegister => CPURegister.FP;

	/// <summary>
	/// Retrieves the stack pointer register of the ARM64.
	/// </summary>
	public override PhysicalRegister StackPointerRegister => CPURegister.SP;

	/// <summary>
	/// Gets the return register.
	/// </summary>
	public override PhysicalRegister ReturnRegister => CPURegister.R0;

	public override PhysicalRegister LinkRegister => CPURegister.LR;

	/// <summary>
	/// Gets the return register for the high portion of the 64bit result.
	/// </summary>
	public override PhysicalRegister ReturnHighRegister => CPURegister.R1;

	/// <summary>
	/// Gets the return floating point register.
	/// </summary>
	public override PhysicalRegister ReturnFloatingPointRegister => CPURegister.d0;

	/// <summary>
	/// Retrieves the exception register of the architecture.
	/// </summary>
	public override PhysicalRegister ExceptionRegister => CPURegister.R8;

	/// <summary>
	/// Gets the finally return block register.
	/// </summary>
	public override PhysicalRegister LeaveTargetRegister => CPURegister.R9;

	/// <summary>
	/// Retrieves the program counter register of the ARM64.
	/// </summary>
	public override PhysicalRegister ProgramCounter => CPURegister.PC;

	/// <summary>
	/// Gets the instructions.
	/// </summary>
	/// <value>
	/// The instructions.
	/// </value>
	public override List<BaseInstruction> Instructions => ARM64Instructions.List;

	public override OpcodeEncoder GetOpcodeEncoder()
	{
		return new OpcodeEncoder(32);
	}

	/// <summary>
	/// Extends the compiler pipeline with ARM64 specific stages.
	/// </summary>
	/// <param name="pipeline">The pipeline to extend.</param>
	public override void ExtendCompilerPipeline(Pipeline<BaseCompilerStage> pipeline, MosaSettings mosaSettings)
	{
	}

	/// <summary>
	/// Extends the method compiler pipeline with ARM64 specific stages.
	/// </summary>
	/// <param name="pipeline">The method compiler pipeline to extend.</param>
	/// <param name="mosaSettings">The compiler options.</param>
	public override void ExtendMethodCompilerPipeline(Pipeline<BaseMethodCompilerStage> pipeline, MosaSettings mosaSettings)
	{
		pipeline.InsertBefore<CallStage>(
			new Stages.RuntimeCallStage()
		);

		pipeline.InsertAfterLast<PlatformIntrinsicStage>(
			new BaseMethodCompilerStage[]
			{
				new IRTransformationStage(),
				mosaSettings.PlatformOptimizations ? new Stages.OptimizationStage() : null,
				new PlatformTransformationStage(),
			});

		pipeline.InsertBefore<CodeGenerationStage>(
			new BaseMethodCompilerStage[]
			{
				new PlatformTransformationStage(),
				mosaSettings.PlatformOptimizations ? new Stages.OptimizationStage() : null,
			});

		pipeline.InsertBefore<CodeGenerationStage>(
			new JumpOptimizationStage()
		);

		//pipeline.InsertBefore<GreedyRegisterAllocatorStage>(
		//	new StopStage()
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
		//var instruction = ARM64.Mov; // TODO

		//if (destination.IsR4)
		//{
		//	instruction = ARM64.Mvf;
		//}
		//else if (destination.IsR8)
		//{
		//	instruction = ARM64.Mvf;
		//}

		//context.AppendInstruction(instruction, destination, source);
	}

	public override void InsertStoreInstruction(Context context, Operand destination, Operand offset, Operand value)
	{
		//var instruction = ARM64.Str32; // TODO

		//if (destination.IsR4)
		//{
		//	instruction = ARM64.Stf;
		//}
		//else if (destination.IsR8)
		//{
		//	instruction = ARM64.Stf;
		//}

		//context.AppendInstruction(instruction, null, destination, offset, value);
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
		//var instruction = ARM64.Ldr32; // TODO

		//if (destination.IsR4)
		//{
		//	instruction = ARM64.Ldf;
		//}
		//else if (destination.IsR8)
		//{
		//	instruction = ARM64.Ldf;
		//}

		//context.AppendInstruction(instruction, destination, source, offset);
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
		//context.AppendInstruction(ARM64.B, destination);
		context.ConditionCode = ConditionCode.Always;
	}

	/// <summary>
	/// Determines whether [is instruction move] [the specified instruction].
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <returns></returns>
	public override bool IsInstructionMove(BaseInstruction instruction)
	{
		//return instruction == ARM64.Mov || instruction == ARM64.Mvf; // TODO
		return false;
	}
}
