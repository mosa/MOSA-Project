/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Mosa.Runtime.CompilerFramework.Ir;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Vm;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Base class of a method compiler.
    /// </summary>
    /// <remarks>
    /// A method compiler is responsible for compiling a single function
    /// of an object. There are various classes derived from MethodCompilerBase,
    /// which provide specific features, such as jit compilation, runtime
    /// optimized jitting and others. MethodCompilerBase instances are ussually
    /// created by invoking CreateMethodCompiler on a specific compiler
    /// instance.
    /// </remarks>
    public abstract class MethodCompilerBase : CompilerBase<IMethodCompilerStage>, IDisposable
    {
        #region Data members

        /// <summary>
        /// The architecture of the compilation target.
        /// </summary>
        private IArchitecture _architecture;

        /// <summary>
        /// Holds the linker used to resolve external symbols.
        /// </summary>
        private IAssemblyLinker _linker;

        /// <summary>
        /// The method definition being compiled.
        /// </summary>
        private RuntimeMethod _method;

        /// <summary>
        /// The metadata module, that contains the method.
        /// </summary>
        private IMetadataModule _module;

        /// <summary>
        /// Holds the next free stack slot index.
        /// </summary>
        private int _nextStackSlot;

        /// <summary>
        /// Holds the type, which owns the method.
        /// </summary>
        private RuntimeType _type;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="MethodCompilerBase"/>.
        /// </summary>
        /// <param name="architecture">The target compilation architecture.</param>
        /// <param name="module">The metadata module, that contains the type.</param>
        /// <param name="type">The type, which owns the method to compile.</param>
        /// <param name="method">The method to compile by this instance.</param>
        protected MethodCompilerBase(IAssemblyLinker linker, IArchitecture architecture, IMetadataModule module, RuntimeType type, RuntimeMethod method)
        {
            if (null == architecture)
                throw new ArgumentNullException(@"architecture");
            if (null == linker)
                throw new ArgumentNullException(@"linker");

            _architecture = architecture;
            _linker = linker;
            _method = method;
            _module = module;
            _type = type;

            // HACK: Magic number, remove this once collisions with classical StackOperands are removed.
            _nextStackSlot = 100000;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Retrieves the architecture to compile for.
        /// </summary>
        public IArchitecture Architecture
        {
            get { return _architecture; }
        }

        /// <summary>
        /// Retrieves the assembly, which contains the method.
        /// </summary>
        public IMetadataModule Assembly
        {
            get { return _module; }
        }

        /// <summary>
        /// Retrieves the linker used to resolve external symbols.
        /// </summary>
        public IAssemblyLinker Linker
        {
            get { return _linker; }
        }

        /// <summary>
        /// Retrieves the method implementation being compiled.
        /// </summary>
        public RuntimeMethod Method
        {
            get { return _method; }
        }

        /// <summary>
        /// Holds the owner type of the method.
        /// </summary>
        public RuntimeType Type
        {
            get { return _type; }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Compiles the method referenced by this method compiler.
        /// </summary>
        public void Compile()
        {
            this.Pipeline.Execute(delegate(IMethodCompilerStage stage)
            {
                stage.Run(this);
            });
        }

        /// <summary>
        /// Creates a result operand for an instruction.
        /// </summary>
        /// <param name="type">The signature type of the operand to be created.</param>
        /// <returns>A new temporary result operand.</returns>
        public Operand CreateResultOperand(SigType type)
        {
            return _architecture.CreateResultOperand(type, _nextStackSlot, _nextStackSlot++);
        }

        /// <summary>
        /// Provides access to the instructions of the method.
        /// </summary>
        /// <returns>A stream, which represents the IL of the method.</returns>
        public Stream GetInstructionStream()
        {
            return _method.Module.GetInstructionStream(_method.Rva);
        }

        /// <summary>
        /// Requests a stream to emit native instructions to.
        /// </summary>
        /// <returns>A stream object, which can be used to store emitted instructions.</returns>
        public abstract Stream RequestCodeStream();

        #endregion // Methods

        #region IDisposable Members

        public void Dispose()
        {
            if (null == _pipeline)
                throw new ObjectDisposedException(@"MethodCompilerBase");

            foreach (IMethodCompilerStage mcs in _pipeline)
            {
                IDisposable d = mcs as IDisposable;
                if (null != d)
                {
                    d.Dispose();
                }
            }

            _pipeline.Clear();
            _pipeline = null;

            _architecture = null;
            _linker = null;
            _method = null;
            _module = null;
            _type = null;
        }

        #endregion // IDisposable Members
    }
}
