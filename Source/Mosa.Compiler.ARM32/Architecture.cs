// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.ARM32.CompilerStages;
using Mosa.Compiler.ARM32.Stages;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.CompilerStages;
using Mosa.Compiler.Framework.Linker.Elf;
using Mosa.Compiler.Framework.Stages;
using Mosa.Utility.Configuration;

namespace Mosa.Compiler.ARM32;

/// <summary>
/// ARM32 Architecture
/// </summary>
public sealed class Architecture : BaseArchitecture
{
	/// <summary>
	/// Gets the type of the elf machine.
	/// </summary>
	public override MachineType ElfMachineType => MachineType.ARM;

	/// <summary>
	/// Gets the name of the platform.
	/// </summary>
	public override string PlatformName => "ARM32";

	/// <summary>
	/// Gets the register set of the architecture.
	/// </summary>
	private static readonly PhysicalRegister[] registers = new PhysicalRegister[]
	{
		////////////////////////////////////////////////////////
		// 32-bit general purpose registers
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
	/// <value>This property always returns 4.</value>
	public override uint NativePointerSize => 4;

	/// <summary>
	/// Retrieves the register set of the ARM32 platform.
	/// </summary>
	public override PhysicalRegister[] RegisterSet => registers;

	/// <summary>
	/// Retrieves the stack frame register of the ARM32.
	/// </summary>
	public override PhysicalRegister StackFrameRegister => CPURegister.FP;

	/// <summary>
	/// Retrieves the stack pointer register of the ARM32.
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
	/// Retrieves the program counter register of the ARM32.
	/// </summary>
	public override PhysicalRegister ProgramCounter => CPURegister.PC;

	/// <summary>
	/// Updates the setting.
	/// </summary>
	/// <param name="settings">The settings.</param>
	public override void UpdateSetting(MosaSettings settings)
	{
		//settings.LongExpansion = true;  // required for ARM
	}

	/// <summary>
	/// Gets the opcode encoder.
	/// </summary>
	public override OpcodeEncoder GetOpcodeEncoder()
	{
		return new OpcodeEncoder(32);
	}

	/// <summary>
	/// Extends the compiler pipeline with ARM32 specific stages.
	/// </summary>
	/// <param name="pipeline">The pipeline to extend.</param>
	public override void ExtendCompilerPipeline(Pipeline<BaseCompilerStage> pipeline, MosaSettings mosaSettings)
	{
		if (!string.IsNullOrEmpty(mosaSettings.MultibootVersion))
		{
			pipeline.InsertAfterFirst<TypeInitializerStage>(
				new MultibootStage()
			);
		}

		pipeline.Add(new StartUpStage());
	}

	/// <summary>
	/// Extends the method compiler pipeline with ARM32 specific stages.
	/// </summary>
	/// <param name="pipeline">The method compiler pipeline to extend.</param>
	/// <param name="mosaSettings">The compiler options.</param>
	public override void ExtendMethodCompilerPipeline(Pipeline<BaseMethodCompilerStage> pipeline, MosaSettings mosaSettings)
	{
		pipeline.InsertBefore<CallStage>(
			new RuntimeCallStage()
		);

		pipeline.InsertAfterLast<PlatformIntrinsicStage>(
		[
			new AdvanceTransformStage(),
			new IRTransformStage(),
			mosaSettings.PlatformOptimizations ? new Stages.OptimizationStage() : null,
			new PlatformTransformStage(),
		]);

		pipeline.InsertBefore<CodeGenerationStage>(
		[
			new PlatformTransformStage(),
			mosaSettings.PlatformOptimizations ? new Stages.OptimizationStage() : null,
		]);

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
		var instruction = ARM32.Mov;

		if (destination.IsR4)
		{
			instruction = ARM32.VMov;
		}
		else if (destination.IsR8)
		{
			instruction = ARM32.VMov;
		}

		context.AppendInstruction(instruction, destination, source);
	}

	public override void InsertStoreInstruction(Context context, Operand destination, Operand offset, Operand value)
	{
		var instruction = destination.IsFloatingPoint ? ARM32.VStr : ARM32.Str32;

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
		var instruction = destination.IsFloatingPoint ? ARM32.VLdr : ARM32.Ldr32;

		context.AppendInstruction(instruction, destination, source, offset);
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
		context.AppendInstruction(ARM32.B, destination);
		context.ConditionCode = ConditionCode.Always;
	}

	/// <summary>
	/// Determines whether [is instruction move] [the specified instruction].
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <returns></returns>
	public override bool IsInstructionMove(BaseInstruction instruction)
	{
		return instruction == ARM32.Mov || instruction == ARM32.VMov;
	}

	/// <summary>
	/// Determines whether [is parameter store] [the specified context].
	/// </summary>
	/// <param name="node">the node</param>
	/// <param name="operand">The operand.</param>
	/// <returns>
	///   <c>true</c> if [is parameter store] [the specified context]; otherwise, <c>false</c>.</returns>
	public override bool IsParameterStore(Node node, out Operand operand)
	{
		// TODO
		operand = null;
		return false;
	}

	/// <summary>
	/// Determines whether [is parameter load] [the specified context].
	/// </summary>
	/// <param name="node">the node</param>
	/// <returns>
	///   <c>true</c> if [is parameter load] [the specified context]; otherwise, <c>false</c>.</returns>
	public override bool IsParameterLoad(Node node, out Operand operand)
	{
		// TODO
		operand = null;

		if (node.ResultCount != 1
			|| node.OperandCount != 2
			|| !node.Instruction.IsMemoryRead
			|| !node.Operand1.IsPhysicalRegister
			|| node.Operand1.Register != CPURegister.FP)
			return false;

		return false;
	}

	public override bool IsConstantIntegerLoad(Node node, out Operand operand)
	{
		// TODO
		operand = null;

		if (node.Instruction != ARM32.Mov)
			return false;

		operand = node.Operand1;
		return false;
	}
}
