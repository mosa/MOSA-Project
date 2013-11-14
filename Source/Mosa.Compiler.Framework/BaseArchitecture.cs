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
using Mosa.Compiler.Metadata.Signatures;
using System;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Implements a base framework for architectures.
	/// </summary>
	public abstract class BaseArchitecture
	{
		#region Properties

		/// <summary>
		/// Retrieves an object, that is able to translate the CIL calling convention into appropriate native code.
		/// </summary>
		public ICallingConvention CallingConvention { get; protected set; }

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
		public abstract ushort ElfMachineType { get; }

		/// <summary>
		/// Gets the signature type of the native integer.
		/// </summary>
		public abstract SigType NativeType { get; }

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
		/// Gets the name of the platform.
		/// </summary>
		/// <value>
		/// The name of the platform.
		/// </value>
		public abstract string PlatformName { get; }

		/// <summary>
		/// Gets the jump instruction for the platform.
		/// </summary>
		/// <value>
		/// The jump instruction.
		/// </value>
		public abstract BaseInstruction JumpInstruction { get; }

		/// <summary>
		/// Gets the width of a native integer in bits.
		/// </summary>
		public abstract int NativeIntegerSize { get; }

		#endregion Properties

		#region Methods

		/// <summary>
		/// Extends the compiler pipeline with architecture specific assembly compiler stages.
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
		/// <param name="type">The signature type.</param>
		/// <param name="size">Receives the memory size of the type.</param>
		/// <param name="alignment">Receives alignment requirements of the type.</param>
		public abstract void GetTypeRequirements(SigType type, out int size, out int alignment);

		/// <summary>
		/// Gets the code emitter.
		/// </summary>
		/// <returns></returns>
		public abstract ICodeEmitter GetCodeEmitter();

		/// <summary>
		/// Create platform move.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="Destination">The destination.</param>
		/// <param name="Source">The source.</param>
		public abstract void InsertMove(Context context, Operand Destination, Operand Source);

		/// <summary>
		/// Create platform exchange registers.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="Destination">The destination.</param>
		/// <param name="Source">The source.</param>
		public abstract void InsertExchange(Context context, Operand Destination, Operand Source);

		#endregion Methods
	}
}