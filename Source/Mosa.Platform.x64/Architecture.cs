// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Linker.Elf;
using Mosa.Compiler.MosaTypeSystem;

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
		private static readonly Register[] Registers = new Register[]
		{
			//TODO
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
		/// Retrieves the native integer size of the x64 platform.
		/// </summary>
		/// <value>This property always returns 64.</value>
		public override int NativeIntegerSize
		{
			get { return 64; }
		}

		/// <summary>
		/// Gets the native alignment of the architecture in bytes.
		/// </summary>
		/// <value>This property always returns 8.</value>
		public override int NativeAlignment
		{
			get { return 8; }
		}

		/// <summary>
		/// Gets the native size of architecture in bytes.
		/// </summary>
		/// <value>This property always returns 8.</value>
		public override int NativePointerSize
		{
			get { return 8; }
		}

		/// <summary>
		/// Retrieves the register set of the x64 platform.
		/// </summary>
		public override Register[] RegisterSet
		{
			get { return Registers; }
		}

		/// <summary>
		/// Retrieves the stack frame register of the x64.
		/// </summary>
		public override Register StackFrameRegister
		{
			get { return null; /* GeneralPurposeRegister.EBP;*/ }
		}

		/// <summary>
		/// Retrieves the stack pointer register of the x64.
		/// </summary>
		public override Register StackPointerRegister
		{
			get { return null; /* GeneralPurposeRegister.EDX;*/ }
		}

		/// <summary>
		/// Retrieves the scratch register of the x86.
		/// </summary>
		public override Register ScratchRegister
		{
			get { return null; /* TODO */}
		}

		/// <summary>
		/// Gets the return32 bit register.
		/// </summary>
		public override Register Return32BitRegister
		{
			get { return null; /* TODO */}
		}

		/// <summary>
		/// Gets the return64 bit register.
		/// </summary>
		public override Register Return64BitRegister
		{
			get { return null; /* TODO */}
		}

		/// <summary>
		/// Gets the return floating point register.
		/// </summary>
		public override Register ReturnFloatingPointRegister
		{
			get { return null; /* TODO */}
		}

		/// <summary>
		/// Retrieves the exception register of the architecture.
		/// </summary>
		public override Register ExceptionRegister
		{
			get { return null; /* GeneralPurposeRegister.EDI;*/ }
		}

		/// <summary>
		/// Gets the finally return block register.
		/// </summary>
		public override Register LeaveTargetRegister
		{
			get { return null; /* GeneralPurposeRegister.EDX;*/ }
		}

		/// <summary>
		/// Retrieves the stack pointer register of the x64.
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
		public override string PlatformName { get { return "x64"; } }

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
		/// Extends the pre compiler pipeline with x64 specific stages.
		/// </summary>
		/// <param name="compilerPipeline">The pipeline to extend.</param>
		public override void ExtendCompilerPipeline(CompilerPipeline compilerPipeline)
		{
			// TODO
		}

		/// <summary>
		/// Extends the method compiler pipeline with x64 specific stages.
		/// </summary>
		/// <param name="compilerPipeline">The method compiler pipeline to extend.</param>
		public override void ExtendMethodCompilerPipeline(CompilerPipeline compilerPipeline)
		{
			// TODO
		}

		/// <summary>
		/// Gets the type memory requirements.
		/// </summary>
		/// <param name="typeLayout">The type layouts.</param>
		/// <param name="type">The signature type.</param>
		/// <param name="size">Receives the memory size of the type.</param>
		/// <param name="alignment">Receives alignment requirements of the type.</param>
		public override void GetTypeRequirements(MosaTypeLayout typeLayout, MosaType type, out int size, out int alignment)
		{
			alignment = NativeAlignment;

			size = type.IsValueType ? typeLayout.GetTypeSize(type) : NativeAlignment;
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
		/// <param name="compiler">The compiler.</param>
		/// <param name="context">The context.</param>
		/// <param name="destinationBase">The destination.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="sourceBase">The source.</param>
		/// <param name="source">The source.</param>
		/// <param name="size">The size.</param>
		/// <exception cref="NotImplementCompilerException"></exception>
		public override void InsertCompoundCopy(BaseMethodCompiler compiler, Context context, Operand destinationBase, Operand destination, Operand sourceBase, Operand source, int size)
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
		/// <param name="source">The source.</param>
		public override void InsertJumpInstruction(Context context, Operand destination)
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
		/// Inserts the call instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="source">The source.</param>
		/// <exception cref="NotImplementCompilerException"></exception>
		public override void InsertCallInstruction(Context context, Operand source)
		{
			throw new NotImplementCompilerException();
		}

		/// <summary>
		/// Inserts the add instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="source1">The source1.</param>
		/// <param name="source2">The source2.</param>
		/// <exception cref="NotImplementCompilerException"></exception>
		public override void InsertAddInstruction(Context context, Operand destination, Operand source1, Operand source2)
		{
			throw new NotImplementCompilerException();
		}

		/// <summary>
		/// Inserts the sub instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="source1">The source1.</param>
		/// <param name="source2">The source2.</param>
		/// <exception cref="NotImplementCompilerException"></exception>
		public override void InsertSubInstruction(Context context, Operand destination, Operand source1, Operand source2)
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
