/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Interface to allow compiler stages to perform architecture specific operations.
	/// </summary>
	/// <remarks>
	/// The functions in this interface are required to perform architecture specific
	/// optimizations in previous stages.
	/// </remarks>
	public interface IArchitecture
	{
		#region Properties

		/// <summary>
		/// Gets a value indicating whether this architecture is little-endian.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is architecture is little-endian; otherwise, <c>false</c>.
		/// </value>
		bool IsLittleEndian { get; }

		/// <summary>
		/// Gets the type of the elf machine.
		/// </summary>
		/// <value>
		/// The type of the elf machine.
		/// </value>
		ushort ElfMachineType { get; }

		/// <summary>
		/// Returns the type, which represents a native (unsigned) on the VES stack.
		/// </summary>
		SigType NativeType { get; }

		/// <summary>
		/// Returns the entire register set of the architecture.
		/// </summary>
		/// <remarks>
		/// Returns an array of Register classes, which represent the register set of
		/// the target machine.
		/// </remarks>
		Register[] RegisterSet { get; }

		/// <summary>
		/// Returns the stack frame register of the architecture.
		/// </summary>
		Register StackFrameRegister { get; }

		/// <summary>
		/// Returns the stack pointer register of the architecture.
		/// </summary>
		Register StackPointerRegister { get; }

		/// <summary>
		/// Gets the name of the platform.
		/// </summary>
		/// <value>
		/// The name of the platform.
		/// </value>
		string PlatformName { get; }

		#endregion Properties

		#region Methods

		/// <summary>
		/// Retrieves an object, that is able to translate the CIL calling convention into appropriate native code.
		/// </summary>
		/// <returns>A calling convention implementation.</returns>
		ICallingConvention CallingConvention { get; }

		/// <summary>
		/// Requests the architecture to add architecture specific compilation stages to the assembly compiler
		/// pipeline. These may depend upon the current state of the pipeline.
		/// </summary>
		/// <param name="compilerPipeline">The compiler pipeline.</param>
		void ExtendCompilerPipeline(CompilerPipeline compilerPipeline);

		/// <summary>
		/// Requests the architecture to add architecture specific compilation stages to the pipeline. These
		/// may depend upon the current state of the pipeline.
		/// </summary>
		/// <param name="methodPipeline">
		/// The pipeline of the method compiler to add architecture specific compilation stages to.
		/// </param>
		void ExtendMethodCompilerPipeline(CompilerPipeline methodPipeline);

		/// <summary>
		/// Gets the type memory requirements.
		/// </summary>
		/// <param name="type">The signature type.</param>
		/// <param name="size">Receives the memory size of the type.</param>
		/// <param name="alignment">Receives alignment requirements of the type.</param>
		void GetTypeRequirements(SigType type, out int size, out int alignment);

		/// <summary>
		/// Gets the code emitter.
		/// </summary>
		/// <returns></returns>
		ICodeEmitter GetCodeEmitter();

		#endregion Methods
	}
}