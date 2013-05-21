/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Implements a basic framework for architectures.
	/// </summary>
	public abstract class BasicArchitecture : IArchitecture
	{
		/// <summary>
		/// Holds the calling conversion
		/// </summary>
		public ICallingConvention CallingConvention { get; protected set; }

		/// <summary>
		/// Gets a value indicating whether this architecture is little-endian.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is architecture is little-endian; otherwise, <c>false</c>.
		/// </value>
		public abstract bool IsLittleEndian { get; }

		/// <summary>
		/// Gets the type of the elf machine.
		/// </summary>
		/// <value>
		/// The type of the elf machine.
		/// </value>
		public abstract ushort ElfMachineType { get; }

		/// <summary>
		/// Holds the native type of the architecture.
		/// </summary>
		private SigType nativeType;

		/// <summary>
		/// Gets the width of a native integer in bits.
		/// </summary>
		public abstract int NativeIntegerSize { get; }

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
		/// Gets the name of the platform.
		/// </summary>
		/// <value>
		/// The name of the platform.
		/// </value>
		public abstract string PlatformName { get; }

		/// <summary>
		/// Gets the signature type of the native integer.
		/// </summary>
		public SigType NativeType
		{
			get
			{
				if (nativeType == null)
				{
					switch (NativeIntegerSize)
					{
						case 32:
							nativeType = BuiltInSigType.Int32;
							break;

						case 64:
							nativeType = BuiltInSigType.Int64;
							break;

						default:
							throw new NotSupportedException(@"The native bit width is not supported.");
					}
				}

				return nativeType;
			}
		}

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
	}
}