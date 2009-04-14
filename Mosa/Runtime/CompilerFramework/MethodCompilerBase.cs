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
    public class MethodCompilerBase : CompilerBase<IMethodCompilerStage>,
                                      IMethodCompiler,
                                      IDisposable
    {
        /// <summary>
        /// Holds a list of operands, which represent method parameters.
        /// </summary>
        private readonly List<Operand> parameters;

        /// <summary>
        /// The architecture of the compilation target.
        /// </summary>
        private IArchitecture architecture;

        /// <summary>
        /// Holds the linker used to resolve external symbols.
        /// </summary>
        private IAssemblyLinker linker;

        /// <summary>
        /// Holds a list of operands, which represent local variables.
        /// </summary>
        private List<Operand> locals;

        /// <summary>
        /// Optional signature of stack local variables.
        /// </summary>
        private LocalVariableSignature localsSig;

        /// <summary>
        /// The method definition being compiled.
        /// </summary>
        private RuntimeMethod method;

        /// <summary>
        /// The metadata module, that contains the method.
        /// </summary>
        private IMetadataModule module;

        /// <summary>
        /// Holds the next free stack slot index.
        /// </summary>
        private int nextStackSlot;

        /// <summary>
        /// Holds the type, which owns the method.
        /// </summary>
        private RuntimeType type;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodCompilerBase"/> class.
        /// </summary>
        /// <param name="linker">The linker.</param>
        /// <param name="architecture">The target compilation architecture.</param>
        /// <param name="module">The metadata module, that contains the type.</param>
        /// <param name="type">The type, which owns the method to compile.</param>
        /// <param name="method">The method to compile by this instance.</param>
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

            this.architecture = architecture;
            this.linker = linker;
            this.method = method;
            this.module = module;
            this.parameters = new List<Operand>(new Operand[this.method.Parameters.Count]);
            this.type = type;
            this.nextStackSlot = 0;
        }

        /// <summary>
        /// Gets the architecture to compile for.
        /// </summary>
        public IArchitecture Architecture
        {
            get { return this.architecture; }
        }

        /// <summary>
        /// Gets the assembly, which contains the method.
        /// </summary>
        public IMetadataModule Assembly
        {
            get { return this.module; }
        }

        /// <summary>
        /// Gets the linker used to resolve external symbols.
        /// </summary>
        public IAssemblyLinker Linker
        {
            get { return this.linker; }
        }

        /// <summary>
        /// Gets the method implementation being compiled.
        /// </summary>
        public RuntimeMethod Method
        {
            get { return this.method; }
        }

        /// <summary>
        /// Gets the owner type of the method.
        /// </summary>
        public RuntimeType Type
        {
            get { return this.type; }
        }

        /// <summary>
        /// Requests a stream to emit native instructions to.
        /// </summary>
        /// <returns>A stream object, which can be used to store emitted instructions.</returns>
        public virtual Stream RequestCodeStream()
        {
            return this.linker.Allocate(this.method, SectionKind.Text, 0, 0);
        }

        /// <summary>
        /// Compiles the method referenced by this method compiler.
        /// </summary>
        public void Compile()
        {
            this.BeginCompile();
            this.Pipeline.Execute(delegate(IMethodCompilerStage stage) { stage.Run(this); });
            this.EndCompile();
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
                return this.architecture.CreateResultOperand(type, this.nextStackSlot, this.nextStackSlot++);
            }
            else
            {
                return this.CreateTemporary(type);
            }
        }

        /// <summary>
        /// Creates a new temporary local variable operand.
        /// </summary>
        /// <param name="type">The signature type of the temporary.</param>
        /// <returns>An operand, which represents the temporary.</returns>
        public Operand CreateTemporary(SigType type)
        {
            int stackSlot = this.nextStackSlot++;
            return new LocalVariableOperand(
                this.architecture.StackFrameRegister, String.Format(@"T_{0}", stackSlot), stackSlot, type);
        }

        /// <summary>
        /// Führt anwendungsspezifische Aufgaben durch, die mit der Freigabe, der Zurückgabe oder dem Zurücksetzen von nicht verwalteten Ressourcen zusammenhängen.
        /// </summary>
        public void Dispose()
        {
            if (null == this._pipeline)
            {
                throw new ObjectDisposedException(@"MethodCompilerBase");
            }

            foreach (IMethodCompilerStage mcs in this._pipeline)
            {
                IDisposable d = mcs as IDisposable;
                if (null != d)
                {
                    d.Dispose();
                }
            }

            this._pipeline.Clear();
            this._pipeline = null;

            this.architecture = null;
            this.linker = null;
            this.method = null;
            this.module = null;
            this.type = null;
        }

        /// <summary>
        /// Provides access to the instructions of the method.
        /// </summary>
        /// <returns>A stream, which represents the IL of the method.</returns>
        public Stream GetInstructionStream()
        {
            return this.method.Module.GetInstructionStream(this.method.Rva);
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
            Debug.Assert(null != this.localsSig, @"Method doesn't have locals.");
            Debug.Assert(idx <= this.localsSig.Types.Length, @"Invalid local index requested.");
            if (null == this.localsSig || this.localsSig.Types.Length <= idx)
            {
                throw new ArgumentOutOfRangeException(@"index", idx, @"Invalid parameter index");
            }

            Operand local = null;
            if (this.locals.Count > idx)
            {
                local = this.locals[idx];
            }

            if (null == local)
            {
                local = new LocalVariableOperand(
                    this.architecture.StackFrameRegister, String.Format("L_{0}", idx), idx, this.localsSig.Types[idx]);
                this.locals[idx] = local;
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
            MethodSignature sig = this.method.Signature;
            if (true == sig.HasThis || true == sig.HasExplicitThis)
            {
                if (0 == idx)
                {
                    return new ParameterOperand(
                        this.architecture.StackFrameRegister,
                        new RuntimeParameter(this.method.Module, @"this", 0, ParameterAttributes.In),
                        new ClassSigType((TokenTypes) this.type.Token));
                }
                else
                {
                    // Decrement the index, as the caller actually wants a real parameter
                    idx--;
                }
            }

            // A normal argument, decode it...
            IList<RuntimeParameter> parameters = this.method.Parameters;
            Debug.Assert(null != parameters, @"Method doesn't have arguments.");
            Debug.Assert(idx < parameters.Count, @"Invalid argument index requested.");
            if (null == parameters || parameters.Count <= idx)
            {
                throw new ArgumentOutOfRangeException(@"index", idx, @"Invalid parameter index");
            }

            Operand param = null;
            if (this.parameters.Count > idx)
            {
                param = this.parameters[idx];
            }

            if (null == param)
            {
                param = new ParameterOperand(
                    this.architecture.StackFrameRegister, parameters[idx], sig.Parameters[idx]);
                this.parameters[idx] = param;
            }

            return param;
        }

        /// <summary>
        /// Sets the signature of local variables in the method.
        /// </summary>
        /// <param name="localVariableSignature">The local variable signature of the method.</param>
        public void SetLocalVariableSignature(LocalVariableSignature localVariableSignature)
        {
            if (null == localVariableSignature)
            {
                throw new ArgumentNullException(@"localVariableSignature");
            }

            this.localsSig = localVariableSignature;
            this.locals = new List<Operand>(new Operand[this.localsSig.Types.Length]);
            this.nextStackSlot = this.locals.Count + 1;
        }

        /// <summary>
        /// Called before the method compiler begins compiling the method.
        /// </summary>
        protected virtual void BeginCompile()
        {
        }

        /// <summary>
        /// Called after the method compiler has finished compiling the method.
        /// </summary>
        protected virtual void EndCompile()
        {
        }
    }
}