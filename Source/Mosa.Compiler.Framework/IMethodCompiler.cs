/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.IO;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata.Loader;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Interface provided by method compilers.
	/// </summary>
	public interface IMethodCompiler : IDisposable
	{
		/// <summary>
		/// Retrieves the architecture to compile for.
		/// </summary>
		/// <value>The compilation target architecture. This may differ from the current execution architecture.</value>
		IArchitecture Architecture { get; }

		/// <summary>
		/// Gets the metadata module being compiled.
		/// </summary>
		/// <value>The currently compiled module.</value>
		IMetadataModule Assembly { get; }

		/// <summary>
		/// Retrieves the linker used to resolve external symbols.
		/// </summary>
		IAssemblyLinker Linker { get; }

		/// <summary>
		/// Retrieves the method being compiled.
		/// </summary>
		/// <value>The method being compiled.</value>
		RuntimeMethod Method { get; }

		/// <summary>
		/// Retrieves the compilation scheduler.
		/// </summary>
		/// <value>The compilation scheduler.</value>
		ICompilationSchedulerStage Scheduler { get; }

		/// <summary>
		/// Creates a new temporary variable operand.
		/// </summary>
		/// <param name="type">The signature type of the temporary.</param>
		/// <returns>An operand, which represents the temporary.</returns>
		/// <remarks>
		/// Later optimization stages attempt to optimize, reduce or remove the usage
		/// of temporaries in a program. Temporaries may be allocated to physical
		/// registers as part of register allocation strategies.
		/// </remarks>
		Operand CreateTemporary(SigType type);

		/// <summary>
		/// Provides access to the instructions of the method.
		/// </summary>
		/// <returns>A stream, which represents the IL of the method.</returns>
		Stream GetInstructionStream();

		/// <summary>
		/// Retrieves the local stack operand at the specified <paramref name="index"/>.
		/// </summary>
		/// <param name="index">The index of the local variable to retrieve.</param>
		/// <returns>The operand at the specified index.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">The <paramref name="index"/> is not valid.</exception>
		Operand GetLocalOperand(int index);

		/// <summary>
		/// Creates a new temporary local variable operand.
		/// </summary>
		/// <param name="index">The index of the parameter to retrieve.</param>
		/// <returns>An operand, which represents the temporary.</returns>
		Operand GetParameterOperand(int index);

		/// <summary>
		/// Gets the stage.
		/// </summary>
		/// <param name="stageType">Type of the stage.</param>
		/// <returns></returns>
		IPipelineStage GetStage(Type stageType);

		/// <summary>
		/// Requests a stream to emit native instructions to.
		/// </summary>
		/// <returns>A stream object, which can be used to store emitted instructions.</returns>
		Stream RequestCodeStream();

		/// <summary>
		/// Sets the signature of local variables in the method.
		/// </summary>
		/// <param name="localVariableSignature">The local variable signature of the method.</param>
		void SetLocalVariableSignature(LocalVariableSignature localVariableSignature);

		/// <summary>
		/// Gets the instruction set.
		/// </summary>
		/// <value>The instruction set.</value>
		InstructionSet InstructionSet { get; }

		/// <summary>
		/// Gets the basic Blocks.
		/// </summary>
		/// <value>The basic Blocks.</value>
		IList<BasicBlock> BasicBlocks { get; }

		/// <summary>
		/// Retrieves a basic block from its label.
		/// </summary>
		/// <param name="label">The label of the basic block.</param>
		/// <returns>The basic block with the given label or null.</returns>
		BasicBlock FromLabel(int label);

		/// <summary>
		/// Creates the block.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		BasicBlock CreateBlock(int label, int index);

		/// <summary>
		/// Provides access to the pipeline of this compiler.
		/// </summary>
		CompilerPipeline Pipeline { get; }

		/// <summary>
		/// Compiles the method.
		/// </summary>
		void Compile();

		/// <summary>
		/// Gets the type system.
		/// </summary>
		/// <value>The type system.</value>
		ITypeSystem TypeSystem { get; }

		/// <summary>
		/// Gets the type layout.
		/// </summary>
		/// <value>The type layout.</value>
		ITypeLayout TypeLayout { get; }

		/// <summary>
		/// Gets the internal logging interface
		/// </summary>
		/// <value>The log.</value>
		IInternalTrace InternalLog { get; }

		/// <summary>
		/// Gets the exception clause header.
		/// </summary>
		/// <value>The exception clause header.</value>
		ExceptionClauseHeader ExceptionClauseHeader { get; }

		/// <summary>
		/// Gets the local variables.
		/// </summary>
		Operand[] LocalVariables { get; }

		/// <summary>
		/// Gets the assembly compiler.
		/// </summary>
		AssemblyCompiler AssemblyCompiler { get; }

		/// <summary>
		/// Gets the plug.
		/// </summary>
		IPlugSystem PlugSystem { get; }

		/// <summary>
		/// Gets the stack layout.
		/// </summary>
		StackLayout StackLayout { get; }
	}
}
