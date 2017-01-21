// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Linker.Elf;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	///
	/// </summary>
	public abstract class BaseArchitecture
	{
		#region Properties

		/// <summary>
		/// Retrieves an object, that is able to translate the CIL calling convention into appropriate native code.
		/// </summary>
		public BaseCallingConvention CallingConvention { get; protected set; }

		/// <summary>
		/// Gets the endianness of the target architecture.
		/// </summary>
		/// <value>
		/// The endianness.
		/// </value>
		public abstract Endianness Endianness { get; }

		/// <summary>
		/// Gets the type of the elf machine.
		/// </summary>
		/// <value>
		/// The type of the elf machine.
		/// </value>
		public abstract MachineType MachineType { get; }

		/// <summary>
		/// Gets the register set of the architecture.
		/// </summary>
		public abstract Register[] RegisterSet { get; }

		/// <summary>
		/// Gets the stack frame register of the architecture.
		/// </summary>
		public abstract Register StackFrameRegister { get; }

		/// <summary>
		/// Returns the stack pointer register of the architecture.
		/// </summary>
		public abstract Register StackPointerRegister { get; }

		/// <summary>
		/// Retrieves the program counter register of the architecture.
		/// </summary>
		public abstract Register ProgramCounter { get; }

		/// <summary>
		/// Retrieves the exception register of the architecture.
		/// </summary>
		public abstract Register ExceptionRegister { get; }

		/// <summary>
		/// Gets the finally return block register.
		/// </summary>
		public abstract Register LeaveTargetRegister { get; }

		/// <summary>
		/// Gets the name of the platform.
		/// </summary>
		public abstract string PlatformName { get; }

		/// <summary>
		/// Gets the width of a native integer in bits.
		/// </summary>
		public abstract int NativeIntegerSize { get; }

		/// <summary>
		/// Gets the native alignment of the architecture in bytes.
		/// </summary>
		public abstract int NativeAlignment { get; }

		/// <summary>
		/// Gets the native size of architecture in bytes.
		/// </summary>
		public abstract int NativePointerSize { get; }

		/// <summary>
		/// Gets the size of the native instruction.
		/// </summary>
		/// <value>
		/// The size of the native instruction.
		/// </value>
		public virtual InstructionSize NativeInstructionSize
		{
			get { return NativePointerSize == 4 ? InstructionSize.Size32 : InstructionSize.Size64; }
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Extends the pre-compiler pipeline with architecture specific compiler stages.
		/// </summary>
		/// <param name="compilerPipeline">The pipeline to extend.</param>
		public abstract void ExtendCompilerPipeline(CompilerPipeline compilerPipeline);

		/// <summary>
		/// Requests the architecture to add architecture specific compilation stages to the pipeline. These
		/// may depend upon the current state of the pipeline.
		/// </summary>
		/// <param name="methodPipeline">The pipeline of the method compiler to add architecture specific compilation stages to.</param>
		public abstract void ExtendMethodCompilerPipeline(CompilerPipeline methodPipeline);

		/// <summary>
		/// Gets the type memory requirements.
		/// </summary>
		/// <param name="typeLayout">The type layouts.</param>
		/// <param name="type">The type.</param>
		/// <param name="size">Receives the memory size of the type.</param>
		/// <param name="alignment">Receives alignment requirements of the type.</param>
		public abstract void GetTypeRequirements(MosaTypeLayout typeLayout, MosaType type, out int size, out int alignment);

		/// <summary>
		/// Gets the code emitter.
		/// </summary>
		/// <returns></returns>
		public abstract BaseCodeEmitter GetCodeEmitter();

		/// <summary>
		/// Create platform move.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source.</param>
		public abstract void InsertMoveInstruction(Context context, Operand destination, Operand source);

		/// <summary>
		/// Inserts the store instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		public abstract void InsertStoreInstruction(Context context, Operand destination, Operand offset, Operand value);

		/// <summary>
		/// Inserts the load instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source.</param>
		/// <param name="offset">The offset.</param>
		public abstract void InsertLoadInstruction(Context context, Operand destination, Operand source, Operand offset);

		/// <summary>
		/// Create platform compound move.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="context">The context.</param>
		/// <param name="destinationBase">The destination.</param>
		/// <param name="destination">The destination offset.</param>
		/// <param name="sourceBase">The source.</param>
		/// <param name="source">The source offset.</param>
		/// <param name="size">The size.</param>
		public abstract void InsertCompoundCopy(BaseMethodCompiler compiler, Context context, Operand destinationBase, Operand destination, Operand sourceBase, Operand source, int size);

		/// <summary>
		/// Create platform exchange registers.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source.</param>
		public abstract void InsertExchangeInstruction(Context context, Operand destination, Operand source);

		/// <summary>
		/// Create platform exchange registers.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		public abstract void InsertJumpInstruction(Context context, Operand destination);

		/// <summary>
		/// Inserts the jump instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		public abstract void InsertJumpInstruction(Context context, BasicBlock destination);

		/// <summary>
		/// Inserts the call instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		public abstract void InsertCallInstruction(Context context, Operand destination);

		/// <summary>
		/// Inserts the add instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="source1">The source1.</param>
		/// <param name="source2">The source2.</param>
		public abstract void InsertAddInstruction(Context context, Operand destination, Operand source1, Operand source2);

		/// <summary>
		/// Inserts the sub instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="source1">The source1.</param>
		/// <param name="source2">The source2.</param>
		public abstract void InsertSubInstruction(Context context, Operand destination, Operand source1, Operand source2);

		/// <summary>
		/// Determines whether [is instruction move] [the specified instruction].
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns></returns>
		public abstract bool IsInstructionMove(BaseInstruction instruction);

		#endregion Methods
	}
}
