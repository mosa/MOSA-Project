﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.Framework.Linker.Elf;
using Mosa.Platform.ARMv6.Stages;

namespace Mosa.Platform.ARMv6
{
	/// <summary>
	/// This class provides a common base class for architecture
	/// specific operations.
	/// </summary>
	public class Architecture : BaseArchitecture
	{
		/// <summary>
		/// Gets the endianness of the target architecture.
		/// </summary>
		/// <value>
		/// The endianness. The ARM Architecture allows either big endian or little endian formats.
		/// In storing data, the ARM follows the little endian convention.
		/// </value>
		public override Endianness Endianness { get { return Endianness.Little; } }

		/// <summary>
		/// Gets the type of the elf machine.
		/// </summary>
		/// <value>
		/// The type of the elf machine. As defined in ARM ELF File Format doc. page 5, EM_ARM.
		/// </value>
		public override MachineType MachineType { get { return MachineType.ARM; } }

		/// <summary>
		/// Gets the name of the platform.
		/// </summary>
		/// <value>
		/// The name of the platform.
		/// </value>
		public override string PlatformName { get { return "ARMv6"; } }

		/// <summary>
		/// Factory method for the Architecture class.
		/// </summary>
		/// <returns>The created architecture instance.</returns>
		/// <param name="architectureFeatures">The features available in the architecture and code generation.</param>
		/// <remarks>
		/// This method creates an instance of an appropriate architecture class, which supports the specific
		/// architecture features.
		/// </remarks>
		public static BaseArchitecture CreateArchitecture(ArchitectureFeatureFlags architectureFeatures)
		{
			if (architectureFeatures == ArchitectureFeatureFlags.AutoDetect)
				architectureFeatures = ArchitectureFeatureFlags.ARM; // TODO

			return new Architecture(architectureFeatures);
		}

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
		/// Specifies the architecture features to use in generated code.
		/// </summary>
		private readonly ArchitectureFeatureFlags architectureFeatures;

		/// <summary>
		/// Initializes a new instance of the <see cref="Architecture"/> class.
		/// </summary>
		/// <param name="architectureFeatures">The features this architecture supports.</param>
		private Architecture(ArchitectureFeatureFlags architectureFeatures)
		{
			this.architectureFeatures = architectureFeatures;
		}

		/// <summary>
		/// Gets the native size of architecture in bytes.
		/// </summary>
		/// <value>This property always returns 4.</value>
		public override int NativePointerSize { get { return 4; } }

		/// <summary>
		/// Retrieves the register set of the ARMv6 platform.
		/// </summary>
		public override PhysicalRegister[] RegisterSet
		{
			get { return registers; }
		}

		/// <summary>
		/// Retrieves the stack frame register of the ARMv6.
		/// </summary>
		public override PhysicalRegister StackFrameRegister
		{
			get { return GeneralPurposeRegister.LR; }
		}

		/// <summary>
		/// Retrieves the stack pointer register of the ARMv6.
		/// </summary>
		public override PhysicalRegister StackPointerRegister
		{
			get { return GeneralPurposeRegister.SP; }
		}

		/// <summary>
		/// Retrieves the scratch register of the ARMv6.
		/// </summary>
		public override PhysicalRegister ScratchRegister
		{
			get { return null; } // TODO
		}

		/// <summary>
		/// Gets the return32 bit register.
		/// </summary>
		public override PhysicalRegister Return32BitRegister
		{
			get { return null; /* TODO */}
		}

		/// <summary>
		/// Gets the return64 bit register.
		/// </summary>
		public override PhysicalRegister Return64BitRegister
		{
			get { return null; /* TODO */}
		}

		/// <summary>
		/// Gets the return floating point register.
		/// </summary>
		public override PhysicalRegister ReturnFloatingPointRegister
		{
			get { return null; /* TODO */}
		}

		/// <summary>
		/// Retrieves the exception register of the architecture.
		/// </summary>
		public override PhysicalRegister ExceptionRegister
		{
			get { return GeneralPurposeRegister.R10; }
		}

		/// <summary>
		/// Gets the finally return block register.
		/// </summary>
		public override PhysicalRegister LeaveTargetRegister
		{
			get { return GeneralPurposeRegister.R9; }
		}

		/// <summary>
		/// Retrieves the program counter register of the ARMv6.
		/// </summary>
		public override PhysicalRegister ProgramCounter
		{
			get { return GeneralPurposeRegister.PC; }
		}

		/// <summary>
		/// Extends the assembly compiler pipeline with ARMv6 specific stages.
		/// </summary>
		/// <param name="compilerPipeline">The pipeline to extend.</param>
		public override void ExtendCompilerPipeline(Pipeline<BaseCompilerStage> compilerPipeline)
		{
			// TODO
		}

		/// <summary>
		/// Extends the method compiler pipeline with ARMv6 specific stages.
		/// </summary>
		/// <param name="compilerPipeline">The method compiler pipeline to extend.</param>
		public override void ExtendMethodCompilerPipeline(Pipeline<BaseMethodCompilerStage> compilerPipeline)
		{
			compilerPipeline.InsertAfterLast<PlatformIntrinsicStage>(
				new BaseMethodCompilerStage[]
				{
			        //new LongOperandTransformationStage(),
			        new IRTransformationStage(),
				});

			//methodCompilerPipeline.InsertAfterLast<CodeGenerationStage>(
			//    new ExceptionLayoutStage()
			//);

			compilerPipeline.InsertBefore<GreedyRegisterAllocatorStage>(
				new StopStage()
			);
		}

		/// <summary>
		/// Gets the code emitter.
		/// </summary>
		/// <returns></returns>
		public override BaseCodeEmitter GetCodeEmitter()
		{
			return new ARMv6CodeEmitter();
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
		/// Create platform compound move.
		/// </summary>
		/// <param name="methodCompiler">The compiler.</param>
		/// <param name="context">The context.</param>
		/// <param name="destinationBase">The destination.</param>
		/// <param name="destination">The destination offset.</param>
		/// <param name="sourceBase">The source.</param>
		/// <param name="source">The source offset.</param>
		/// <param name="size">The size.</param>
		/// <exception cref="NotImplementCompilerException"></exception>
		public override void InsertCompoundCopy(MethodCompiler methodCompiler, Context context, Operand destinationBase, Operand destination, Operand sourceBase, Operand source, int size)
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
			return instruction == ARMv6.Mov;
		}
	}
}
