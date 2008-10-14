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
        /// Optional signature of stack local variables.
        /// </summary>
        private LocalVariableSignature _localsSig;

        /// <summary>
        /// Holds a list of operands, which represent local variables.
        /// </summary>
        private List<Operand> _locals;

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
        /// Holds a list of operands, which represent method parameters.
        /// </summary>
        private List<Operand> _parameters;

        /// <summary>
        /// Holds the type, which owns the method.
        /// </summary>
        private RuntimeType _type;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="MethodCompilerBase"/>.
        /// </summary>
        /// <param name="linker">The linker.</param>
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
            _parameters = new List<Operand>(new Operand[_method.Parameters.Count]);
            _type = type;
            _nextStackSlot = 0;
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
            this.BeginCompile();
            this.Pipeline.Execute(delegate(IMethodCompilerStage stage)
            {
                stage.Run(this);
            });
            this.EndCompile();
        }

        /// <summary>
        /// Called when compilation begins
        /// </summary>
        protected virtual void BeginCompile() { }

        /// <summary>
        /// Called when compilation finishes
        /// </summary>
        protected virtual void EndCompile() { }

        /// <summary>
        /// Creates a new temporary local variable operand.
        /// </summary>
        /// <param name="type">The signature type of the temporary.</param>
        /// <returns>An operand, which represents the temporary.</returns>
        public Operand CreateTemporary(SigType type)
        {
            return new LocalVariableOperand(_architecture.StackFrameRegister, @"T_{0}", _nextStackSlot++, type);
        }

        /// <summary>
        /// Creates a result operand for an instruction.
        /// </summary>
        /// <param name="type">The signature type of the operand to be created.</param>
        /// <returns>A new temporary result operand.</returns>
        public Operand CreateResultOperand(SigType type)
        {
            if (type.Type != CilElementType.I8 && type.Type != CilElementType.U8)
            {
                return _architecture.CreateResultOperand(type, _nextStackSlot, _nextStackSlot++);
            }
            else
            {
                return CreateTemporary(type);
            }
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
        /// Retrieves the local stack operand at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="idx">The index of the stack operand to retrieve.</param>
        /// <returns>The operand at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The <paramref name="index"/> is not valid.</exception>
        public Operand GetLocalOperand(int idx)
        {
            // HACK: Returning a new instance here breaks object identity. We should reuse operands,
            // which represent the same memory location. If we need to move a variable in an optimization
            // stage to a different memory location, it should actually be a new one so sharing object
            // only saves runtime space/perf.
            Debug.Assert(null != _localsSig, @"Method doesn't have locals.");
            Debug.Assert(idx <= _localsSig.Types.Length, @"Invalid local index requested.");
            if (null == _localsSig || _localsSig.Types.Length <= idx)
                throw new ArgumentOutOfRangeException(@"index", idx, @"Invalid parameter index");

            Operand local = null;
            if (_locals.Count > idx)
                local = _locals[idx];

            if (null == local)
            {
                local = new LocalVariableOperand(_architecture.StackFrameRegister, String.Format("L_{0}", idx), idx, _localsSig.Types[idx]);
                _locals[idx] = local;
            }

            return local;
        }

        /// <summary>
        /// Retrieves the parameter operand at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="idx">The index of the parameter operand to retrieve.</param>
        /// <returns>The operand at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The <paramref name="index"/> is not valid.</exception>
        public Operand GetParameterOperand(int idx)
        {
            // HACK: Returning a new instance here breaks object identity. We should reuse operands,
            // which represent the same memory location. If we need to move a variable in an optimization
            // stage to a different memory location, it should actually be a new one so sharing object
            // only saves runtime space/perf.
            MethodSignature sig = _method.Signature;
            if (true == sig.HasThis || true == sig.HasExplicitThis)
            {
                if (0 == idx)
                {
                    return new ParameterOperand(_architecture.StackFrameRegister, new RuntimeParameter(_method.Module, @"this", 0, ParameterAttributes.In), new ClassSigType((TokenTypes)_type.Token));
                }
                else
                {
                    // Decrement the index, as the caller actually wants a real parameter
                    idx--;
                }
            }

            // A normal argument, decode it...
            IList<RuntimeParameter> parameters = _method.Parameters;
            Debug.Assert(null != parameters, @"Method doesn't have arguments.");
            Debug.Assert(idx < parameters.Count, @"Invalid argument index requested.");
            if (null == parameters || parameters.Count <= idx)
                throw new ArgumentOutOfRangeException(@"index", idx, @"Invalid parameter index");

            Operand param = null;
            if (_parameters.Count > idx)
                param = _parameters[idx];

            if (null == param)
            {
                param = new ParameterOperand(_architecture.StackFrameRegister, parameters[idx], sig.Parameters[idx]);
                _parameters[idx] = param;
            }

            return param;
        }

        /// <summary>
        /// Requests a stream to emit native instructions to.
        /// </summary>
        /// <returns>A stream object, which can be used to store emitted instructions.</returns>
        public abstract Stream RequestCodeStream();

        /// <summary>
        /// Sets the signature of local variables in the method.
        /// </summary>
        /// <param name="localVariableSignature">The local variable signature of the method.</param>
        public void SetLocalVariableSignature(LocalVariableSignature localVariableSignature)
        {
            if (null == localVariableSignature)
                throw new ArgumentNullException(@"localVariableSignature");

            _localsSig = localVariableSignature;
            _locals = new List<Operand>(new Operand[_localsSig.Types.Length]);
            _nextStackSlot = _locals.Count + 1;
        }

        #endregion // Methods

        #region IDisposable Members

        /// <summary>
        /// Führt anwendungsspezifische Aufgaben durch, die mit der Freigabe, der Zurückgabe oder dem Zurücksetzen von nicht verwalteten Ressourcen zusammenhängen.
        /// </summary>
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
