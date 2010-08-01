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
using System.Diagnostics;
using System.IO;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Runtime;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Base class of a method compiler.
	/// </summary>
	/// <remarks>
	/// A method compiler is responsible for compiling a single function
	/// of an object. There are various classes derived From MethodCompilerBase,
	/// which provide specific features, such as jit compilation, runtime
	/// optimized jitting and others. MethodCompilerBase instances are usually
	/// created by invoking CreateMethodCompiler on a specific compiler
	/// instance.
	/// </remarks>
	public class BaseMethodCompiler : IMethodCompiler, IDisposable
	{

		#region Data Members

		/// <summary>
		/// Holds the pipeline of the compiler.
		/// </summary>
		protected CompilerPipeline pipeline;

		/// <summary>
		/// Holds a list of operands, which represent method parameters.
		/// </summary>
		private readonly List<Operand> _parameters;

		private readonly ICompilationSchedulerStage compilationScheduler;

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
		/// Holds the next free stack slot index.
		/// </summary>
		protected int _nextStackSlot;

		/// <summary>
		/// Holds the _type, which owns the _method.
		/// </summary>
		private RuntimeType _type;

		/// <summary>
		/// Holds the instruction set
		/// </summary>
		private InstructionSet _instructionSet;

		/// <summary>
		/// Holds the basic blocks
		/// </summary>
		private List<BasicBlock> _basicBlocks;

		/// <summary>
		/// Holds the type system during compilation.
		/// </summary>
		protected ITypeSystem typeSystem;

		/// <summary>
		/// Holds the assembly loader
		/// </summary>
		protected IAssemblyLoader assemblyLoader;

		#endregion // Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseMethodCompiler"/> class.
		/// </summary>
		/// <param name="linker">The _linker.</param>
		/// <param name="architecture">The target compilation Architecture.</param>
		/// <param name="type">The type, which owns the _method to compile.</param>
		/// <param name="method">The method to compile by this instance.</param>
		protected BaseMethodCompiler(
			IAssemblyLinker linker,
			IArchitecture architecture,
			ICompilationSchedulerStage compilationScheduler,
			RuntimeType type,
			RuntimeMethod method,
			ITypeSystem typeSystem,
			IAssemblyLoader assemblyLoader)
		{
			if (architecture == null)
				throw new ArgumentNullException(@"architecture");

			if (linker == null)
				throw new ArgumentNullException(@"linker");

			if (compilationScheduler == null)
				throw new ArgumentNullException(@"compilationScheduler");

			_linker = linker;
			_architecture = architecture;
			this.compilationScheduler = compilationScheduler;
			_method = method;
			_parameters = new List<Operand>(new Operand[_method.Parameters.Count]);
			_type = type;
			_nextStackSlot = 0;
			_basicBlocks = new List<BasicBlock>();
			_instructionSet = null; // this will be set later

			pipeline = new CompilerPipeline();

			this.typeSystem = typeSystem;
			this.assemblyLoader = assemblyLoader;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the Architecture to compile for.
		/// </summary>
		public IArchitecture Architecture
		{
			get { return _architecture; }
		}

		/// <summary>
		/// Gets the assembly, which contains the method.
		/// </summary>
		public IMetadataModule Assembly
		{
			get { return this._method.Module; }
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
		/// Gets the owner type of the method.
		/// </summary>
		public RuntimeType Type
		{
			get { return _type; }
		}

		/// <summary>
		/// Gets the instruction set.
		/// </summary>
		/// <value>The instruction set.</value>
		public InstructionSet InstructionSet
		{
			get { return _instructionSet; }
			set { _instructionSet = value; }
		}

		/// <summary>
		/// Gets the basic Blocks.
		/// </summary>
		/// <value>The basic blocks.</value>
		public List<BasicBlock> BasicBlocks
		{
			get { return _basicBlocks; }
			set { _basicBlocks = value; }
		}

		public ICompilationSchedulerStage Scheduler
		{
			get { return this.compilationScheduler; }
		}

		/// <summary>
		/// Provides access to the pipeline of this compiler.
		/// </summary>
		public CompilerPipeline Pipeline
		{
			get { return pipeline; }
		}

		/// <summary>
		/// Gets the type system.
		/// </summary>
		/// <value>The type system.</value>
		public ITypeSystem TypeSystem { get { return typeSystem; } }

		/// <summary>
		/// Gets the assembly loader.
		/// </summary>
		/// <value>The assembly loader.</value>
		public IAssemblyLoader AssemblyLoader { get { return assemblyLoader; } }

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Requests a stream to emit native instructions to.
		/// </summary>
		/// <returns>A stream object, which can be used to store emitted instructions.</returns>
		public virtual Stream RequestCodeStream()
		{
			return _linker.Allocate(_method.ToString(), SectionKind.Text, 0, 0);
		}

		/// <summary>
		/// Compiles the _method referenced by this method compiler.
		/// </summary>
		public void Compile()
		{
			BeginCompile();

			Pipeline.Execute<IMethodCompilerStage>(delegate(IMethodCompilerStage stage)
				{
					stage.Setup(this);
					stage.Run();
				}
			);

			EndCompile();
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
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			if (pipeline == null)
				throw new ObjectDisposedException(@"MethodCompilerBase");

			foreach (IMethodCompilerStage mcs in pipeline)
			{
				IDisposable d = mcs as IDisposable;
				if (null != d)
					d.Dispose();
			}

			pipeline.Clear();
			pipeline = null;

			_architecture = null;
			_linker = null;
			_method = null;
			_type = null;
			_instructionSet = null;
			_basicBlocks = null;
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
		/// <param name="index">The index of the stack operand to retrieve.</param>
		/// <returns>The operand at the specified index.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">The <paramref name="index"/> is not valid.</exception>
		public Operand GetLocalOperand(int index)
		{
			// HACK: Returning a new instance here breaks object identity. We should reuse operands,
			// which represent the same memory location. If we need to move a variable in an optimization
			// stage to a different memory location, it should actually be a new one so sharing object
			// only saves runtime space/perf.
			Debug.Assert(_localsSig != null, @"Method doesn't have _locals.");
			Debug.Assert(index < _localsSig.Locals.Length, @"Invalid local index requested.");
			if (_localsSig == null || _localsSig.Locals.Length < index)
				throw new ArgumentOutOfRangeException(@"index", index, @"Invalid parameter index");

			Operand local = null;
			if (_locals.Count > index)
				local = _locals[index];

			if (local == null)
			{

				VariableSignature localVariable = _localsSig.Locals[index];
				this.ScheduleDependencyForCompilation(localVariable.Type);

				local = new LocalVariableOperand(_architecture.StackFrameRegister, String.Format("L_{0}", index), index, localVariable.Type);
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
				if (index == 0)
				{
					return new ParameterOperand(
						_architecture.StackFrameRegister,
						new RuntimeParameter(_method.Module, @"this", 2, ParameterAttributes.In),
						new ClassSigType((TokenTypes)_type.Token));
				}
				// Decrement the index, as the caller actually wants a real parameter
				--index;
			}

			// A normal argument, decode it...
			IList<RuntimeParameter> parameters = _method.Parameters;
			Debug.Assert(parameters != null, @"Method doesn't have arguments.");
			Debug.Assert(index < parameters.Count, @"Invalid argument index requested.");
			if (parameters == null || parameters.Count <= index)
				throw new ArgumentOutOfRangeException(@"index", index, @"Invalid parameter index");

			Operand param = null;
			if (_parameters.Count > index)
				param = _parameters[index];

			if (param == null)
			{
				SigType parameterType = sig.Parameters[index];
				param = new ParameterOperand(_architecture.StackFrameRegister, parameters[index], parameterType);
				_parameters[index] = param;
			}

			return param;
		}

		private void ScheduleDependencyForCompilation(SigType signatureType)
		{
			RuntimeType runtimeType = null;

			TypeSigType typeSigType = signatureType as TypeSigType;
			if (typeSigType != null)
			{
				runtimeType = this.LoadDependentType(typeSigType.Token);
			}
			else
			{
				GenericInstSigType genericSignatureType = signatureType as GenericInstSigType;
				if (genericSignatureType != null)
				{
					RuntimeType genericType = this.LoadDependentType(genericSignatureType.BaseType.Token);
					Console.WriteLine(@"Loaded generic type {0}", genericType.FullName);

					runtimeType = new CilGenericType(genericType, this.Assembly, genericSignatureType, this.Method, typeSystem);
				}
			}

			if (runtimeType != null)
			{
				this.compilationScheduler.ScheduleTypeForCompilation(runtimeType);
			}
		}

		private RuntimeType LoadDependentType(TokenTypes tokenType)
		{
			return typeSystem.GetType(this.Method, this.Assembly, tokenType);
		}

		/// <summary>
		/// Sets the signature of local variables in the method.
		/// </summary>
		/// <param name="localVariableSignature">The local variable signature of the _method.</param>
		public void SetLocalVariableSignature(LocalVariableSignature localVariableSignature)
		{
			if (localVariableSignature == null)
				throw new ArgumentNullException(@"localVariableSignature");

			_localsSig = localVariableSignature;

			int count = _localsSig.Locals.Length;
			this._locals = new List<Operand>(count);
			for (int index = 0; index < count; index++)
				this._locals.Add(null);

			_nextStackSlot = _locals.Count + 1;
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

		/// <summary>
		/// Gets the previous stage.
		/// </summary>
		/// <param name="stage">The stage.</param>
		/// <returns>
		/// The previous compilation stage supporting the requested type or null.
		/// </returns>
		public IPipelineStage GetPreviousStage(IPipelineStage stage)
		{
			return GetPreviousStage(typeof(IPipelineStage));
		}

		/// <summary>
		/// Finds a stage, which ran before the current one and supports the specified type.
		/// </summary>
		/// <param name="stageType">The (interface) type to look for.</param>
		/// <returns>The previous compilation stage supporting the requested type.</returns>
		/// <remarks>
		/// This method is used by stages to access the results of a previous compilation stage.
		/// </remarks>
		public IPipelineStage GetPreviousStage(Type stageType)
		{
			IPipelineStage result = null;

			for (int stage = pipeline.CurrentStage - 1; -1 != stage; stage--)
			{
				IPipelineStage temp = pipeline[stage];
				if (stageType.IsInstanceOfType(temp))
				{
					result = temp;
					break;
				}
			}

			return result;
		}

		/// <summary>
		/// Retrieves a basic block From its label.
		/// </summary>
		/// <param name="label">The label of the basic block.</param>
		/// <returns>
		/// The basic block with the given label or null.
		/// </returns>
		public BasicBlock FromLabel(int label)
		{
			return _basicBlocks.Find(delegate(BasicBlock block)
			{
				return (label == block.Label);
			});
		}

		/// <summary>
		/// Creates the block.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public BasicBlock CreateBlock(int label, int index)
		{
			// HACK: BasicBlock.Count for the sequence works for now since blocks are not removed
			BasicBlock basicBlock = new BasicBlock(_basicBlocks.Count, label, index);
			_basicBlocks.Add(basicBlock);
			return basicBlock;
		}

		#endregion // Methods

	}
}