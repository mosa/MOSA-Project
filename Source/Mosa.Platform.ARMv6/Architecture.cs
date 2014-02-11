/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Marcelo Caetano (marcelocaetano) <marcelo.caetano@ymail.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Platform.ARMv6.Stages;
using System;

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
		public override ushort ElfMachineType { get { return 40; } }

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
		private static readonly Register[] registers = new Register[]
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
		private ArchitectureFeatureFlags architectureFeatures;

		/// <summary>
		/// Initializes a new instance of the <see cref="Architecture"/> class.
		/// </summary>
		/// <param name="architectureFeatures">The features this architecture supports.</param>
		private Architecture(ArchitectureFeatureFlags architectureFeatures)
		{
			this.architectureFeatures = architectureFeatures;
			this.CallingConvention = new DefaultCallingConvention(this);
		}

		/// <summary>
		/// Retrieves the native integer size of the ARMv6 platform.
		/// </summary>
		/// <value>This property always returns 32.</value>
		public override int NativeIntegerSize
		{
			get { return 32; }
		}

		/// <summary>
		/// Retrieves the register set of the ARMv6 platform.
		/// </summary>
		public override Register[] RegisterSet
		{
			get { return registers; }
		}

		/// <summary>
		/// Retrieves the stack frame register of the ARMv6.
		/// </summary>
		public override Register StackFrameRegister
		{
			get { return GeneralPurposeRegister.LR; }
		}

		/// <summary>
		/// Retrieves the stack pointer register of the ARMv6.
		/// </summary>
		public override Register StackPointerRegister
		{
			get { return GeneralPurposeRegister.SP; }
		}

		/// <summary>
		/// Retrieves the program counter register of the ARMv6.
		/// </summary>
		public override Register ProgramCounter
		{
			get { return GeneralPurposeRegister.PC; }
		}

		/// <summary>
		/// Extends the assembly compiler pipeline with ARMv6 specific stages.
		/// </summary>
		/// <param name="compilerPipeline">The pipeline to extend.</param>
		public override void ExtendCompilerPipeline(CompilerPipeline compilerPipeline)
		{
			//assemblyCompilerPipeline.InsertAfterFirst<ICompilerStage>(
			//    new InterruptVectorStage()
			//);

			//assemblyCompilerPipeline.InsertAfterFirst<InterruptVectorStage>(
			//    new ExceptionVectorStage()
			//);

			//assemblyCompilerPipeline.InsertAfterLast<TypeLayoutStage>(
			//    new MethodTableBuilderStage()
			//);
		}

		/// <summary>
		/// Extends the method compiler pipeline with ARMv6 specific stages.
		/// </summary>
		/// <param name="methodCompilerPipeline">The method compiler pipeline to extend.</param>
		public override void ExtendMethodCompilerPipeline(CompilerPipeline methodCompilerPipeline)
		{
			methodCompilerPipeline.InsertAfterLast<PlatformStubStage>(
				new IMethodCompilerStage[]
			    {
			        //new LongOperandTransformationStage(),
			        new IRTransformationStage(),
			    });

			//methodCompilerPipeline.InsertAfterLast<IBlockOrderStage>(
			//    new SimplePeepholeOptimizationStage()
			//);

			//methodCompilerPipeline.InsertAfterLast<CodeGenerationStage>(
			//    new ExceptionLayoutStage()
			//);

			methodCompilerPipeline.InsertBefore<GreedyRegisterAllocatorStage>(
				new StopStage()
			);
		}

		/// <summary>
		/// Gets the type memory requirements.
		/// </summary>
		/// <param name="type">The signature type.</param>
		/// <param name="size">Receives the memory size of the type.</param>
		/// <param name="alignment">Receives alignment requirements of the type.</param>
		/// <exception cref="System.ArgumentNullException">signatureType</exception>
		public override void GetTypeRequirements(MosaType type, out int size, out int alignment)
		{
			if (type.IsLong)
			{
				size = 8;
				alignment = 4;
			}
			else if (type.IsDouble)
			{
				size = 8;
				alignment = 8;
			}
			else
			{
				size = 4;
				alignment = 4;
			}
		}

		/// <summary>
		/// Gets the code emitter.
		/// </summary>
		/// <returns></returns>
		public override BaseCodeEmitter GetCodeEmitter()
		{
			return new MachineCodeEmitter();
		}

		/// <summary>
		/// Create platform move.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="Destination">The destination.</param>
		/// <param name="Source">The source.</param>
		public override void InsertMoveInstruction(Context context, Operand destination, Operand source)
		{
			context.AppendInstruction(BaseTransformationStage.GetMove(destination, source), destination, source);
		}

		/// <summary>
		/// Creates the swap.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="Destination">The destination.</param>
		/// <param name="Source">The source.</param>
		public override void InsertExchangeInstruction(Context context, Operand destination, Operand source)
		{
			// TODO
		}

		/// <summary>
		/// Inserts the jump instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source.</param>
		public override void InsertJumpInstruction(Context context, Operand destination)
		{
			//context.AppendInstruction(ARMv6., destination);
		}

		/// <summary>
		/// Inserts the jump instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="Destination">The destination.</param>
		public override void InsertJumpInstruction(Context context, BasicBlock destination)
		{
			// TODO
		}

		/// <summary>
		/// Inserts the call instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		public override void InsertCallInstruction(Context context, Operand destination)
		{
			//context.AppendInstruction(ARMv6., destination);
		}

		/// <summary>
		/// Inserts the add instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="Destination">The destination.</param>
		/// <param name="Source">The source.</param>
		public override void InsertAddInstruction(Context context, Operand destination, Operand source1, Operand source2)
		{
			context.AppendInstruction(ARMv6.Add, destination);
		}

		/// <summary>
		/// Inserts the sub instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="Destination">The destination.</param>
		/// <param name="Source">The source.</param>
		public override void InsertSubInstruction(Context context, Operand destination, Operand source1, Operand source2)
		{
			context.AppendInstruction(ARMv6.Sub, destination);
		}
	}
}