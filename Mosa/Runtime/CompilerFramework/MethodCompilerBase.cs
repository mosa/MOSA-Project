// -----------------------------------------------------------------------------------------------------------
// <copyright file="MethodCompilerBase.cs" company="(C) 2008-2009 MOSA - The Managed Operating System Alliance">
//  
// (c) 2008-2009 MOSA - The Managed Operating System Alliance
// 
// Licensed under the terms of the New BSD License.
//  
// Authors:
//   Michael Ruck (mailto:sharpos@michaelruck.de)
//   
// </copyright>
// -----------------------------------------------------------------------------------------------------------

namespace Mosa.Runtime.CompilerFramework
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using Linker;
    using Loader;
    using Metadata;
    using Metadata.Signatures;
    using Vm;

    /// <summary>
    /// Base class of a _method compiler.
    /// </summary>
    /// <remarks>
    /// A _method compiler is responsible for compiling a single function
    /// of an object. There are various classes derived from MethodCompilerBase,
    /// which provide specific features, such as jit compilation, runtime
    /// optimized jitting and others. MethodCompilerBase instances are ussually
    /// created by invoking CreateMethodCompiler on a specific compiler
    /// instance.
    /// </remarks>
    public class MethodCompilerBase : CompilerBase<IMethodCompilerStage>,
                                      IMethodCompiler,
                                      IDisposable
    {
        /// <summary>
        /// Holds a list of operands, which represent _method _parameters.
        /// </summary>
        private readonly List<Operand> _parameters;

        /// <summary>
        /// The Architecture of the compilation target.
        /// </summary>
        private IArchitecture _architecture;

        /// <summary>
        /// Holds the _linker used to resolve external symbols.
        /// </summary>
        private IAssemblyLinker _linker;

        /// <summary>
        /// Holds a list of operands, which represent local variables.
        /// </summary>
        private List<Operand> _locals;

        /// <summary>
        /// Optional signature of stack local variables.
        /// </summary>
        private LocalVariableSignature _localsSig;

        /// <summary>
        /// The _method definition being compiled.
        /// </summary>
        private RuntimeMethod _method;

        /// <summary>
        /// The metadata _module, that contains the _method.
        /// </summary>
        private IMetadataModule _module;

        /// <summary>
        /// Holds the next free stack slot index.
        /// </summary>
        private int _nextStackSlot;

        /// <summary>
        /// Holds the _type, which owns the _method.
        /// </summary>
        private RuntimeType _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodCompilerBase"/> class.
        /// </summary>
        /// <param name="linker">The _linker.</param>
        /// <param name="architecture">The target compilation Architecture.</param>
        /// <param name="module">The metadata _module, that contains the _type.</param>
        /// <param name="type">The _type, which owns the _method to compile.</param>
        /// <param name="method">The _method to compile by this instance.</param>
        protected MethodCompilerBase(
            IAssemblyLinker linker,
            IArchitecture architecture,
            IMetadataModule module,
            RuntimeType type,
            RuntimeMethod method)
        {
            if (null == architecture)
            {
                throw new ArgumentNullException(@"architecture");
            }

            if (null == linker)
            {
                throw new ArgumentNullException(@"linker");
            }

            _architecture = architecture;
            _linker = linker;
            _method = method;
            _module = module;
            _parameters = new List<Operand>(new Operand[_method.Parameters.Count]);
            _type = type;
            _nextStackSlot = 0;
        }

        /// <summary>
        /// Gets the Architecture to compile for.
        /// </summary>
        public IArchitecture Architecture
        {
            get { return _architecture; }
        }

        /// <summary>
        /// Gets the assembly, which contains the _method.
        /// </summary>
        public IMetadataModule Assembly
        {
            get { return _module; }
        }

        /// <summary>
        /// Gets the _linker used to resolve external symbols.
        /// </summary>
        public IAssemblyLinker Linker
        {
            get { return _linker; }
        }

        /// <summary>
        /// Gets the _method implementation being compiled.
        /// </summary>
        public RuntimeMethod Method
        {
            get { return _method; }
        }

        /// <summary>
        /// Gets the owner _type of the _method.
        /// </summary>
        public RuntimeType Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Requests a stream to emit native instructions to.
        /// </summary>
        /// <returns>A stream object, which can be used to store emitted instructions.</returns>
        public virtual Stream RequestCodeStream()
        {
            return _linker.Allocate(_method, SectionKind.Text, 0, 0);
        }

        /// <summary>
        /// Compiles the _method referenced by this _method compiler.
        /// </summary>
        public void Compile()
        {
            BeginCompile();
            Pipeline.Execute(delegate(IMethodCompilerStage stage) { stage.Run(this); });
            EndCompile();
        }

        /// <summary>
        /// Creates a result operand for an instruction.
        /// </summary>
        /// <param name="type">The signature _type of the operand to be created.</param>
        /// <returns>A new temporary result operand.</returns>
        public Operand CreateResultOperand(SigType type)
        {
            if (type.Type != CilElementType.I8 && type.Type != CilElementType.U8)
            {
                return _architecture.CreateResultOperand(type, _nextStackSlot, _nextStackSlot++);
            }
            return CreateTemporary(type);
        }

        /// <summary>
        /// Creates a new temporary local variable operand.
        /// </summary>
        /// <param name="type">The signature _type of the temporary.</param>
        /// <returns>An operand, which represents the temporary.</returns>
        public Operand CreateTemporary(SigType type)
        {
            int stackSlot = _nextStackSlot++;
            return new LocalVariableOperand(
                _architecture.StackFrameRegister, String.Format(@"T_{0}", stackSlot), stackSlot, type);
        }

        /// <summary>
        /// Führt anwendungsspezifische Aufgaben durch, die mit der Freigabe, der Zurückgabe oder dem Zurücksetzen von nicht verwalteten Ressourcen zusammenhängen.
        /// </summary>
        public void Dispose()
        {
            if (null == _pipeline)
            {
                throw new ObjectDisposedException(@"MethodCompilerBase");
            }

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

        /// <summary>
        /// Provides access to the instructions of the _method.
        /// </summary>
        /// <returns>A stream, which represents the IL of the _method.</returns>
        public Stream GetInstructionStream()
        {
            return _method.Module.GetInstructionStream(_method.Rva);
        }

        /// <summary>
        /// Retrieves the local stack operand at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the stack operand to retrieve.</param>
        /// <returns>The operand at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The <paramref name="index"/> is not valid.</exception>
        public Operand GetLocalOperand(int index)
        {
            // HACK: Returning a new instance here breaks object identity. We should reuse operands,
            // which represent the same memory location. If we need to move a variable in an optimization
            // stage to a different memory location, it should actually be a new one so sharing object
            // only saves runtime space/perf.
            Debug.Assert(null != _localsSig, @"Method doesn't have _locals.");
            Debug.Assert(index <= _localsSig.Types.Length, @"Invalid local index requested.");
            if (null == _localsSig || _localsSig.Types.Length <= index)
            {
                throw new ArgumentOutOfRangeException(@"index", index, @"Invalid parameter index");
            }

            Operand local = null;
            if (_locals.Count > index)
            {
                local = _locals[index];
            }

            if (null == local)
            {
                local = new LocalVariableOperand(
                    _architecture.StackFrameRegister, String.Format("L_{0}", index), index, _localsSig.Types[index]);
                _locals[index] = local;
            }

            return local;
        }

        /// <summary>
        /// Retrieves the parameter operand at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the parameter operand to retrieve.</param>
        /// <returns>The operand at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The <paramref name="index"/> is not valid.</exception>
        public Operand GetParameterOperand(int index)
        {
            // HACK: Returning a new instance here breaks object identity. We should reuse operands,
            // which represent the same memory location. If we need to move a variable in an optimization
            // stage to a different memory location, it should actually be a new one so sharing object
            // only saves runtime space/perf.
            MethodSignature sig = _method.Signature;
            if (sig.HasThis || sig.HasExplicitThis)
            {
                if (0 == index)
                {
                    return new ParameterOperand(
                        _architecture.StackFrameRegister,
                        new RuntimeParameter(_method.Module, @"this", 0, ParameterAttributes.In),
                        new ClassSigType((TokenTypes) _type.Token));
                }
                // Decrement the index, as the caller actually wants a real parameter
                index--;
            }

            // A normal argument, decode it...
            IList<RuntimeParameter> parameters = _method.Parameters;
            Debug.Assert(null != parameters, @"Method doesn't have arguments.");
            Debug.Assert(index < parameters.Count, @"Invalid argument index requested.");
            if (null == parameters || parameters.Count <= index)
            {
                throw new ArgumentOutOfRangeException(@"index", index, @"Invalid parameter index");
            }

            Operand param = null;
            if (_parameters.Count > index)
            {
                param = _parameters[index];
            }

            if (null == param)
            {
                param = new ParameterOperand(
                    _architecture.StackFrameRegister, parameters[index], sig.Parameters[index]);
                _parameters[index] = param;
            }

            return param;
        }

        /// <summary>
        /// Sets the signature of local variables in the _method.
        /// </summary>
        /// <param name="localVariableSignature">The local variable signature of the _method.</param>
        public void SetLocalVariableSignature(LocalVariableSignature localVariableSignature)
        {
            if (null == localVariableSignature)
            {
                throw new ArgumentNullException(@"localVariableSignature");
            }

            _localsSig = localVariableSignature;
            _locals = new List<Operand>(new Operand[_localsSig.Types.Length]);
            _nextStackSlot = _locals.Count + 1;
        }

        /// <summary>
        /// Called before the _method compiler begins compiling the _method.
        /// </summary>
        protected virtual void BeginCompile()
        {
        }

        /// <summary>
        /// Called after the _method compiler has finished compiling the _method.
        /// </summary>
        protected virtual void EndCompile()
        {
        }
    }
}