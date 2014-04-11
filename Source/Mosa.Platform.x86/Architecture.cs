/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Platform.x86.Stages;
using System.Diagnostics;

namespace Mosa.Platform.x86
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
		public override ushort ElfMachineType { get { return 3; } }

		/// <summary>
		/// Defines the register set of the target architecture.
		/// </summary>
		private static readonly Register[] registers = new Register[]
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
		/// Retrieves the native integer size of the x86 platform.
		/// </summary>
		/// <value>This property always returns 32.</value>
		public override int NativeIntegerSize
		{
			get { return 32; }
		}

		/// <summary>
		/// Gets the native alignment of the architecture in bytes.
		/// </summary>
		/// <value>This property always returns 4.</value>
		public override int NativeAlignment
		{
			get { return 4; }
		}

		/// <summary>
		/// Gets the native size of architecture in bytes.
		/// </summary>
		/// <value>This property always returns 4.</value>
		public override int NativePointerSize
		{
			get { return 4; }
		}

		/// <summary>
		/// Retrieves the register set of the x86 platform.
		/// </summary>
		public override Register[] RegisterSet
		{
			get { return registers; }
		}

		/// <summary>
		/// Retrieves the stack frame register of the x86.
		/// </summary>
		public override Register StackFrameRegister
		{
			get { return GeneralPurposeRegister.EBP; }
		}

		/// <summary>
		/// Retrieves the stack pointer register of the x86.
		/// </summary>
		public override Register StackPointerRegister
		{
			get { return GeneralPurposeRegister.ESP; }
		}

		/// <summary>
		/// Retrieves the program counter register of the x86.
		/// </summary>
		public override Register ProgramCounter
		{
			get { return null; }
		}

		/// <summary>
		/// Gets the name of the platform.
		/// </summary>
		/// <value>
		/// The name of the platform.
		/// </value>
		public override string PlatformName { get { return "x86"; } }

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
				architectureFeatures = ArchitectureFeatureFlags.MMX | ArchitectureFeatureFlags.SSE | ArchitectureFeatureFlags.SSE2;

			return new Architecture(architectureFeatures);
		}

		/// <summary>
		/// Extends the compiler pipeline with x86 specific stages.
		/// </summary>
		/// <param name="compilerPipeline">The pipeline to extend.</param>
		public override void ExtendCompilerPipeline(CompilerPipeline compilerPipeline)
		{
			compilerPipeline.InsertAfterFirst<ICompilerStage>(
				new InterruptVectorStage()
			);

			compilerPipeline.InsertAfterFirst<InterruptVectorStage>(
				new ExceptionVectorStage()
			);

			//FIXME: Uncomment
			//compilerPipeline.InsertAfterLast<TypeLayoutStage>(
			//    new MethodTableBuilderStage()
			//);
		}

		/// <summary>
		/// Extends the method compiler pipeline with x86 specific stages.
		/// </summary>
		/// <param name="methodCompilerPipeline">The method compiler pipeline to extend.</param>
		public override void ExtendMethodCompilerPipeline(CompilerPipeline methodCompilerPipeline)
		{
			// FIXME: Create a specific code generator instance using requested feature flags.
			// FIXME: Add some more optimization passes, which take advantage of advanced x86 instructions
			// and packed operations available with MMX/SSE extensions
			methodCompilerPipeline.InsertAfterLast<PlatformStubStage>(
				new IMethodCompilerStage[]
				{
					//new CheckOperandCountStage(),
					new PlatformIntrinsicTransformationStage(),
					new LongOperandTransformationStage(),

					new IRTransformationStage(),
				    new TweakTransformationStage(),

					new FixedRegisterAssignmentStage(),
					new SimpleDeadCodeRemovalStage(),
				    new AddressModeConversionStage(),
					new FloatingPointStage(),
				});

			methodCompilerPipeline.InsertAfterLast<StackLayoutStage>(
				new BuildStackStage()
			);

			methodCompilerPipeline.InsertBefore<CodeGenerationStage>(
				new FinalTweakTransformationStage()
			);

			methodCompilerPipeline.InsertBefore<CodeGenerationStage>(
				new JumpPeepholeOptimizationStage()
			);

			// FIXME: Disabled for now
			//methodCompilerPipeline.InsertAfterLast<CodeGenerationStage>(
			//    new ExceptionLayoutStage()
			//);
		}

		/// <summary>
		/// Gets the type memory requirements.
		/// </summary>
		/// <param name="typeLayout">The type layouts.</param>
		/// <param name="type">The type.</param>
		/// <param name="size">Receives the memory size of the type.</param>
		/// <param name="alignment">Receives alignment requirements of the type.</param>
		public override void GetTypeRequirements(MosaTypeLayout typeLayout, MosaType type, out int size, out int alignment)
		{
			alignment = type.IsR8 ? 8 : 4;

			size = type.IsValueType ? typeLayout.GetTypeSize(type) : 4;
			size += (alignment - (size % alignment)) % alignment;
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
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source.</param>
		public override void InsertMoveInstruction(Context context, Operand destination, Operand source)
		{
			context.AppendInstruction(BaseTransformationStage.GetMove(destination, source), destination, source);
		}

		/// <summary>
		/// Create platform compound move.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source.</param>
		/// <param name="size">The size.</param>
		public override void InsertCompoundMoveInstruction(Context context, Operand destination, Operand source, int size)
		{
			var type = destination.Type;
			Debug.Assert(size > 0 && size % 4 == 0);

			var src = source;
			var dest = destination;
			Debug.Assert(src.IsMemoryAddress && dest.IsMemoryAddress);

			if (src.EffectiveOffsetBase == GeneralPurposeRegister.ESP)
				src = Operand.CreateMemoryAddress(src.Type.TypeSystem.BuiltIn.I4, Operand.CreateCPURegister(src.Type.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ESP), src.Displacement + 12);
			if (dest.EffectiveOffsetBase == GeneralPurposeRegister.ESP)
				dest = Operand.CreateMemoryAddress(dest.Type.TypeSystem.BuiltIn.I4, Operand.CreateCPURegister(dest.Type.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ESP), dest.Displacement + 12);

			var edi = Operand.CreateCPURegister(destination.Type.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EDI);
			var esi = Operand.CreateCPURegister(destination.Type.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ESI);
			var edx = Operand.CreateCPURegister(destination.Type.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EDX);

			context.AppendInstruction(X86.Lea, edi, src);
			context.AppendInstruction(X86.Lea, esi, dest);
			for (int i = 0; i < size; i += 4)
			{
				context.AppendInstruction(X86.Mov, edx, Operand.CreateMemoryAddress(src.Type.TypeSystem.BuiltIn.I4, edi, i));
				context.AppendInstruction(X86.Mov, Operand.CreateMemoryAddress(dest.Type.TypeSystem.BuiltIn.I4, esi, i), edx);
			}
		}

		/// <summary>
		/// Creates the swap.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source.</param>
		public override void InsertExchangeInstruction(Context context, Operand destination, Operand source)
		{
			if (source.IsR4)
			{
				// TODO
			}
			else if (source.IsR8)
			{
				// TODO
			}
			else
			{
				context.AppendInstruction2(X86.Xchg, destination, source, source, destination);
			}
		}

		/// <summary>
		/// Inserts the jump instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source.</param>
		public override void InsertJumpInstruction(Context context, Operand destination)
		{
			context.AppendInstruction(X86.Jmp, destination);
		}

		/// <summary>
		/// Inserts the jump instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="Destination">The destination.</param>
		public override void InsertJumpInstruction(Context context, BasicBlock destination)
		{
			context.AppendInstruction(X86.Jmp, destination);
		}

		/// <summary>
		/// Inserts the call instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="source">The source.</param>
		public override void InsertCallInstruction(Context context, Operand source)
		{
			context.AppendInstruction(X86.Call, null, source);
		}

		/// <summary>
		/// Inserts the add instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="Destination">The destination.</param>
		/// <param name="Source">The source.</param>
		public override void InsertAddInstruction(Context context, Operand destination, Operand source1, Operand source2)
		{
			Debug.Assert(source1 == destination);
			context.AppendInstruction(X86.Add, destination, source1, source2);
		}

		/// <summary>
		/// Inserts the sub instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="Destination">The destination.</param>
		/// <param name="Source">The source.</param>
		public override void InsertSubInstruction(Context context, Operand destination, Operand source1, Operand source2)
		{
			Debug.Assert(source1 == destination);
			context.AppendInstruction(X86.Sub, destination, source1, source2);
		}

		/// <summary>
		/// Determines whether [is instruction move] [the specified instruction].
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns></returns>
		public override bool IsInstructionMove(BaseInstruction instruction)
		{
			return (instruction == X86.Mov || instruction == X86.Movsd || instruction == X86.Movss);
		}
	}
}