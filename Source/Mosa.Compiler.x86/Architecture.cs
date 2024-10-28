// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.CompilerStages;
using Mosa.Compiler.Framework.Linker.Elf;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.x86.CompilerStages;
using Mosa.Compiler.x86.Stages;
using Mosa.Utility.Configuration;

namespace Mosa.Compiler.x86;

/// <summary>
/// x64 Architecture
/// </summary>
public sealed class Architecture : BaseArchitecture
{
	/// <summary>
	/// Gets the type of the elf machine.
	/// </summary>
	public override MachineType ElfMachineType => MachineType.Intel386;

	/// <summary>
	/// Defines the register set of the target architecture.
	/// </summary>
	private static readonly PhysicalRegister[] registers = new PhysicalRegister[]
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
		CPURegister.XMM7
	};

	/// <summary>
	/// Gets the native size of architecture in bytes.
	/// </summary>
	/// <value>This property always returns 4.</value>
	public override uint NativePointerSize => 4;

	/// <summary>
	/// Retrieves the register set of the x86 platform.
	/// </summary>
	public override PhysicalRegister[] RegisterSet => registers;

	/// <summary>
	/// Retrieves the stack frame register of the x86.
	/// </summary>
	public override PhysicalRegister StackFrameRegister => CPURegister.EBP;

	/// <summary>
	/// Retrieves the stack pointer register of the x86.
	/// </summary>
	public override PhysicalRegister StackPointerRegister => CPURegister.ESP;

	public override PhysicalRegister LinkRegister => null;

	/// <summary>
	/// Gets the return32 bit register.
	/// </summary>
	public override PhysicalRegister ReturnRegister => CPURegister.EAX;

	/// <summary>
	/// Gets the return64 bit register.
	/// </summary>
	public override PhysicalRegister ReturnHighRegister => CPURegister.EDX;

	/// <summary>
	/// Gets the return floating point register.
	/// </summary>
	public override PhysicalRegister ReturnFloatingPointRegister => CPURegister.XMM0;

	/// <summary>
	/// Retrieves the exception register of the architecture.
	/// </summary>
	public override PhysicalRegister ExceptionRegister => CPURegister.EDI;

	/// <summary>
	/// Gets the finally return block register.
	/// </summary>
	public override PhysicalRegister LeaveTargetRegister => CPURegister.ESI;

	/// <summary>
	/// Retrieves the program counter register of the x86.
	/// </summary>
	public override PhysicalRegister ProgramCounter => null;

	/// <summary>
	/// Gets the name of the platform.
	/// </summary>
	public override string PlatformName => "x86";

	/// <summary>
	/// Updates the setting.
	/// </summary>
	/// <param name="settings">The settings.</param>
	public override void UpdateSetting(MosaSettings settings)
	{
	}

	/// <summary>
	/// Gets the opcode encoder.
	/// </summary>
	public override OpcodeEncoder GetOpcodeEncoder()
	{
		return new OpcodeEncoder(8);
	}

	/// <summary>
	/// Extends the compiler pipeline with x86 compiler stages.
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
	/// Extends the method compiler pipeline with x86 specific stages.
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
			//new AdvanceIRTransformStage(),
			new IRTransformationStage(),
			mosaSettings.PlatformOptimizations ? new Stages.OptimizationStage() : null,
			new PlatformTransformStage(),
		]);

		pipeline.InsertBefore<CodeGenerationStage>(
		[
			new PlatformTransformStage(),
			mosaSettings.PlatformOptimizations ? new Stages.OptimizationStage() : null,
		]);
	}

	/// <summary>
	/// Create platform move.
	/// </summary>
	/// <param name="context">The context.</param>
	/// <param name="destination">The destination.</param>
	/// <param name="source">The source.</param>
	public override void InsertMoveInstruction(Context context, Operand destination, Operand source)
	{
		BaseInstruction instruction = X86.Mov32;

		if (destination.IsR4)
		{
			instruction = X86.Movss;
		}
		else if (destination.IsR8)
		{
			instruction = X86.Movsd;
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
	public override void InsertStoreInstruction(Context context, Operand destination, Operand offset, Operand value)
	{
		var instruction = X86.MovStore32;

		if (value.IsR4)
		{
			instruction = X86.MovssStore;
		}
		else if (value.IsR8)
		{
			instruction = X86.MovsdStore;
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
	public override void InsertLoadInstruction(Context context, Operand destination, Operand source, Operand offset)
	{
		BaseInstruction instruction = X86.MovLoad32;

		if (destination.IsR4)
		{
			instruction = X86.MovssLoad;
		}
		else if (destination.IsR8)
		{
			instruction = X86.MovsdLoad;
		}

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
		if (source.IsR8 || source.IsR4)
		{
			context.AppendInstruction(X86.PXor, destination, source);
			context.AppendInstruction(X86.PXor, source, destination);
			context.AppendInstruction(X86.PXor, destination, source);
		}
		else
		{
			context.AppendInstruction2(X86.XChg32, destination, source, source, destination);
		}
	}

	/// <summary>
	/// Inserts the jump instruction.
	/// </summary>
	/// <param name="context">The context.</param>
	/// <param name="destination">The destination.</param>
	public override void InsertJumpInstruction(Context context, BasicBlock destination)
	{
		context.AppendInstruction(X86.Jmp, destination);
	}

	/// <summary>
	/// Determines whether [is instruction move] [the specified instruction].
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <returns></returns>
	public override bool IsInstructionMove(BaseInstruction instruction)
	{
		return instruction == X86.Mov32 || instruction == X86.Movsd || instruction == X86.Movss;
	}

	/// <summary>
	/// Determines whether [is parameter store] [the specified context].
	/// </summary>
	/// <param name="node">The node.</param>
	/// <param name="operand">The operand.</param>
	/// <returns>
	///   <c>true</c> if [is parameter store] [the specified context]; otherwise, <c>false</c>.</returns>
	public override bool IsParameterStore(Node node, out Operand operand)
	{
		operand = null;

		if (node.OperandCount != 3
			|| !node.Instruction.IsMemoryWrite
			|| !node.Operand1.IsPhysicalRegister
			|| node.Operand1.Register != CPURegister.EBP
			)
			return false;

		if (node.Instruction == X86.MovStore32
			|| node.Instruction == X86.MovStore16
			|| node.Instruction == X86.MovStore8
			|| node.Instruction == X86.MovssStore
			|| node.Instruction == X86.MovssStore
			|| node.Instruction == X86.MovsdStore
			)
		{
			operand = node.Operand2;
			return true;
		}

		return false;
	}

	/// <summary>
	/// Determines whether [is parameter load] [the specified context].
	/// </summary>
	/// <param name="node">The node.</param>
	/// <returns>
	///   <c>true</c> if [is parameter load] [the specified context]; otherwise, <c>false</c>.</returns>
	public override bool IsParameterLoad(Node node, out Operand operand)
	{
		operand = null;

		if (node.ResultCount != 1
			|| node.OperandCount != 2
			|| !node.Instruction.IsMemoryRead
			|| !node.Operand1.IsPhysicalRegister
			|| node.Operand1.Register != CPURegister.EBP)
			return false;

		if (node.Instruction == X86.MovLoad32
			|| node.Instruction == X86.MovLoad16
			|| node.Instruction == X86.MovLoad8
			|| node.Instruction == X86.MovssLoad
			|| node.Instruction == X86.MovsdLoad
			|| node.Instruction == X86.MovzxLoad16
			|| node.Instruction == X86.MovzxLoad8
			|| node.Instruction == X86.Movzx16To32
			|| node.Instruction == X86.Movzx8To32
			|| node.Instruction == X86.MovsxLoad16
			|| node.Instruction == X86.MovsxLoad8
			|| node.Instruction == X86.Movsx16To32
			|| node.Instruction == X86.Movsx8To32)
		{
			operand = node.Operand2;
			return true;
		}

		return false;
	}

	public override bool IsConstantIntegerLoad(Node node, out Operand operand)
	{
		operand = null;

		if (node.Instruction == X86.Mov32 && node.Operand1.IsResolvedConstant)
		{
			operand = node.Operand1;
			return true;
		}

		return false;
	}
}
