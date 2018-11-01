// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker.Elf;
using Mosa.Compiler.Framework.Stages;
using Mosa.Platform.Intel;
using Mosa.Platform.x64.Stages;
using System.Collections.Generic;

namespace Mosa.Platform.x64
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
		/// The endianness.
		/// </value>
		public override Endianness Endianness { get { return Endianness.Little; } }

		/// <summary>
		/// Gets the type of the elf machine.
		/// </summary>
		/// <value>
		/// The type of the elf machine.
		/// </value>
		public override MachineType MachineType { get { return MachineType.IA_64; } }

		/// <summary>
		/// Defines the register set of the target architecture.
		/// </summary>
		private static readonly PhysicalRegister[] Registers = new PhysicalRegister[]
		{
			////////////////////////////////////////////////////////
			// 32-bit general purpose registers
			////////////////////////////////////////////////////////
			GeneralPurposeRegister.EAX,
			GeneralPurposeRegister.ECX,
			GeneralPurposeRegister.EDX,
			GeneralPurposeRegister.EBX,
			GeneralPurposeRegister.ESP,
			GeneralPurposeRegister.EBP,
			GeneralPurposeRegister.ESI,
			GeneralPurposeRegister.EDI,

			////////////////////////////////////////////////////////
			// SSE 128-bit floating point registers
			////////////////////////////////////////////////////////
			SSE2Register.XMM0,
			SSE2Register.XMM1,
			SSE2Register.XMM2,
			SSE2Register.XMM3,
			SSE2Register.XMM4,
			SSE2Register.XMM5,
			SSE2Register.XMM6,
			SSE2Register.XMM7,

			////////////////////////////////////////////////////////
			// Segmentation Registers
			////////////////////////////////////////////////////////
			//SegmentRegister.CS,
			//SegmentRegister.DS,
			//SegmentRegister.ES,
			//SegmentRegister.FS,
			//SegmentRegister.GS,
			//SegmentRegister.SS
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
		/// <value>This property always returns 8.</value>
		public override int NativePointerSize { get { return 8; } }

		/// <summary>
		/// Retrieves the register set of the x64 platform.
		/// </summary>
		public override PhysicalRegister[] RegisterSet
		{
			get { return Registers; }
		}

		/// <summary>
		/// Retrieves the stack frame register of the x86.
		/// </summary>
		public override PhysicalRegister StackFrameRegister
		{
			get { return GeneralPurposeRegister.EBP; }
		}

		/// <summary>
		/// Retrieves the stack pointer register of the x86.
		/// </summary>
		public override PhysicalRegister StackPointerRegister
		{
			get { return GeneralPurposeRegister.ESP; }
		}

		/// <summary>
		/// Retrieves the scratch register of the x86.
		/// </summary>
		public override PhysicalRegister ScratchRegister
		{
			get { return GeneralPurposeRegister.EDX; }
		}

		/// <summary>
		/// Gets the return32 bit register.
		/// </summary>
		public override PhysicalRegister Return32BitRegister
		{
			get { return null; }
		}

		/// <summary>
		/// Gets the return64 bit register.
		/// </summary>
		public override PhysicalRegister Return64BitRegister
		{
			get { return GeneralPurposeRegister.EAX; }
		}

		/// <summary>
		/// Gets the return floating point register.
		/// </summary>
		public override PhysicalRegister ReturnFloatingPointRegister
		{
			get { return SSE2Register.XMM0; }
		}

		/// <summary>
		/// Retrieves the exception register of the architecture.
		/// </summary>
		public override PhysicalRegister ExceptionRegister
		{
			get { return GeneralPurposeRegister.EDI; }
		}

		/// <summary>
		/// Gets the finally return block register.
		/// </summary>
		public override PhysicalRegister LeaveTargetRegister
		{
			get { return GeneralPurposeRegister.ESI; }
		}

		/// <summary>
		/// Retrieves the program counter register of the x86.
		/// </summary>
		public override PhysicalRegister ProgramCounter
		{
			get { return null; }
		}

		/// <summary>
		/// Gets the name of the platform.
		/// </summary>
		/// <value>
		/// The name of the platform.
		/// </value>
		public override string PlatformName { get { return "x64"; } }

		/// <summary>
		/// Gets the instructions.
		/// </summary>
		/// <value>
		/// The instructions.
		/// </value>
		public override List<BaseInstruction> Instructions { get { return X64Instructions.List; } }

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
			return new Architecture(architectureFeatures);
		}

		/// <summary>
		/// Extends the pre compiler pipeline with x64 specific stages.
		/// </summary>
		/// <param name="compilerPipeline">The pipeline to extend.</param>
		public override void ExtendCompilerPipeline(Pipeline<BaseCompilerStage> compilerPipeline)
		{
			// TODO
		}

		/// <summary>
		/// Extends the method compiler pipeline with x64 specific stages.
		/// </summary>
		/// <param name="compilerPipeline">The method compiler pipeline to extend.</param>
		public override void ExtendMethodCompilerPipeline(Pipeline<BaseMethodCompilerStage> compilerPipeline)
		{
			compilerPipeline.InsertAfterLast<PlatformIntrinsicStage>(
				new BaseMethodCompilerStage[]
				{
					//new LongOperandStage(),
					//new IRTransformationStage(),
					//new TweakStage(),
					//new FixedRegisterAssignmentStage(),
					//new SimpleDeadCodeRemovalStage(),
					new AddressModeConversionStage(),

					//new FloatingPointStage(),
					//new ConstantInstructionStage(),
				});

			//compilerPipeline.InsertAfterLast<StackLayoutStage>(
			//	new BuildStackStage()
			//);

			//compilerPipeline.InsertBefore<CodeGenerationStage>(
			//	new FinalTweakStage()
			//);

			//compilerPipeline.InsertBefore<CodeGenerationStage>(
			//	new JumpOptimizationStage()
			//);
		}

		/// <summary>
		/// Gets the code emitter.
		/// </summary>
		/// <returns></returns>
		public override BaseCodeEmitter GetCodeEmitter()
		{
			// TODO
			return null;

			//return new MachineCodeEmitter();
		}

		/// <summary>
		/// Inserts the move instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source.</param>
		public override void InsertMoveInstruction(Context context, Operand destination, Operand source)
		{
			throw new NotImplementCompilerException();
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
		/// <param name="destination">The destination.</param>
		/// <param name="sourceBase">The source.</param>
		/// <param name="source">The source.</param>
		/// <param name="size">The size.</param>
		/// <exception cref="NotImplementCompilerException"></exception>
		public override void InsertCompoundCopy(MethodCompiler methodCompiler, Context context, Operand destinationBase, Operand destination, Operand sourceBase, Operand source, int size)
		{
			throw new NotImplementCompilerException();
		}

		/// <summary>
		/// Inserts the exchange instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source.</param>
		public override void InsertExchangeInstruction(Context context, Operand destination, Operand source)
		{
			throw new NotImplementCompilerException();
		}

		/// <summary>
		/// Inserts the jump instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <exception cref="NotImplementCompilerException"></exception>
		public override void InsertJumpInstruction(Context context, BasicBlock destination)
		{
			throw new NotImplementCompilerException();
		}

		/// <summary>
		/// Determines whether [is instruction move] [the specified instruction].
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns></returns>
		public override bool IsInstructionMove(BaseInstruction instruction)
		{
			// TODO
			return false;
		}
	}
}
