/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.IO;
using System.Collections.Generic;

using Mosa.Runtime.Vm;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Linker;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Interface provided by method compilers.
    /// </summary>
    public interface IMethodCompiler
    {
        /// <summary>
        /// Retrieves the architecture to compile for.
        /// </summary>
        /// <value>The compilation target architecture. This may differ From the current execution architecture.</value>
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
        /// Gets the previous stage.
        /// </summary>
        /// <typeparam name="T">The type of the previous stage. Usually a public interface.</typeparam>
        /// <returns>The previous compilation stage supporting the requested type or null.</returns>
        T GetPreviousStage<T>();

        /// <summary>
        /// Finds a stage, which ran before the current one and supports the specified type.
        /// </summary>
        /// <param name="stageType">The (interface) type to look for.</param>
        /// <returns>The previous compilation stage supporting the requested type.</returns>
        /// <remarks>
        /// This method is used by stages to access the results of a previous compilation stage.
        /// </remarks>
        object GetPreviousStage(Type stageType);

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
		InstructionSet InstructionSet { get; set;  }

		/// <summary>
		/// Gets the basic Blocks.
		/// </summary>
		/// <value>The basic Blocks.</value>
		List<BasicBlock> BasicBlocks { get; set; }

		/// <summary>
		/// Retrieves a basic block from its label.
		/// </summary>
		/// <param name="label">The label of the basic block.</param>
		/// <returns>The basic block with the given label or null.</returns>
		BasicBlock FromLabel(int label);
    }
}
