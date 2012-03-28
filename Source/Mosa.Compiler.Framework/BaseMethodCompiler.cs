/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Mosa.Compiler.Framework.CIL;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Loader;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;
using Mosa.Compiler.TypeSystem.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Base class of a method compiler.
	/// </summary>
	/// <remarks>
	/// A method compiler is responsible for compiling a single function
	/// of an object. There are various classes derived from BaseMethodCompiler,
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
		/// Holds a list of operands which represent method parameters.
		/// </summary>
		private readonly List<Operand> parameters;

		/// <summary>
		/// 
		/// </summary>
		private readonly ICompilationSchedulerStage compilationScheduler;

		/// <summary>
		/// The Architecture of the compilation target.
		/// </summary>
		private IArchitecture architecture;

		/// <summary>
		/// Holds the linker used to resolve external symbols
		/// </summary>
		private IAssemblyLinker linker;

		/// <summary>
		/// Holds a list of operands which represent local variables
		/// </summary>
		private Operand[] locals;

		/// <summary>
		/// Optional signature of stack local variables
		/// </summary>
		private LocalVariableSignature localsSig;

		/// <summary>
		/// The method definition being compiled
		/// </summary>
		private RuntimeMethod method;

		/// <summary>
		/// Holds the next free stack slot index
		/// </summary>
		protected int nextStackSlot;

		/// <summary>
		/// Holds the type, which owns the method
		/// </summary>
		private RuntimeType type;

		/// <summary>
		/// Holds the instruction set
		/// </summary>
		private InstructionSet instructionSet;

		/// <summary>
		/// Holds the basic blocks
		/// </summary>
		private List<BasicBlock> basicBlocks;

		/// <summary>
		/// Holds the type system during compilation
		/// </summary>
		protected ITypeSystem typeSystem;

		/// <summary>
		/// Holds the type layout interface
		/// </summary>
		protected ITypeLayout typeLayout;

		/// <summary>
		/// Holds the modules type system
		/// </summary>
		protected ITypeModule moduleTypeSystem;

		/// <summary>
		/// Holds the internal logging interface
		/// </summary>
		protected IInternalTrace internalTrace;

		/// <summary>
		/// Holds the blocks indexed by label
		/// </summary>
		private Dictionary<int, BasicBlock> basicBlocksByLabel = new Dictionary<int, BasicBlock>();

		/// <summary>
		/// Holds the exception clauses
		/// </summary>
		private ExceptionClauseHeader exceptionClauseHeader = new ExceptionClauseHeader();

		/// <summary>
		/// Holds the assembly compiler
		/// </summary>
		private AssemblyCompiler assemblyCompiler;

		/// <summary>
		/// Holds the plug system
		/// </summary>
		private IPlugSystem plugSystem;

		#endregion // Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseMethodCompiler"/> class.
		/// </summary>
		/// <param name="assemblyCompiler">The assembly compiler.</param>
		/// <param name="type">The type, which owns the method to compile.</param>
		/// <param name="method">The method to compile by this instance.</param>
		/// <param name="instructionSet">The instruction set.</param>
		/// <param name="compilationScheduler">The compilation scheduler.</param>
		protected BaseMethodCompiler(AssemblyCompiler assemblyCompiler, RuntimeType type, RuntimeMethod method, InstructionSet instructionSet, ICompilationSchedulerStage compilationScheduler)
		{
			if (compilationScheduler == null)
				throw new ArgumentNullException(@"compilationScheduler");

			this.assemblyCompiler = assemblyCompiler;
			this.method = method;
			this.type = type;
			this.compilationScheduler = compilationScheduler;
			this.moduleTypeSystem = method.Module;

			this.architecture = assemblyCompiler.Architecture;
			this.typeSystem = assemblyCompiler.TypeSystem;
			this.typeLayout = AssemblyCompiler.TypeLayout;
			this.internalTrace = AssemblyCompiler.InternalTrace;

			this.linker = assemblyCompiler.Pipeline.FindFirst<IAssemblyLinker>();
			this.plugSystem = assemblyCompiler.Pipeline.FindFirst<IPlugSystem>();

			this.parameters = new List<Operand>(new Operand[method.Parameters.Count]);
			this.nextStackSlot = 0;
			this.basicBlocks = new List<BasicBlock>();

			this.instructionSet = instructionSet ?? new InstructionSet(256);

			this.pipeline = new CompilerPipeline();
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the Architecture to compile for.
		/// </summary>
		public IArchitecture Architecture { get { return architecture; } }

		/// <summary>
		/// Gets the assembly, which contains the method.
		/// </summary>
		public IMetadataModule Assembly { get { return this.moduleTypeSystem.MetadataModule; } }

		/// <summary>
		/// Gets the linker used to resolve external symbols.
		/// </summary>
		public IAssemblyLinker Linker { get { return linker; } }

		/// <summary>
		/// Gets the method implementation being compiled.
		/// </summary>
		public RuntimeMethod Method { get { return method; } }

		/// <summary>
		/// Gets the owner type of the method.
		/// </summary>
		public RuntimeType Type { get { return type; } }

		/// <summary>
		/// Gets the instruction set.
		/// </summary>
		/// <value>The instruction set.</value>
		public InstructionSet InstructionSet { get { return instructionSet; } }

		/// <summary>
		/// Gets the basic blocks.
		/// </summary>
		/// <value>The basic blocks.</value>
		public IList<BasicBlock> BasicBlocks { get { return basicBlocks; } }

		/// <summary>
		/// Retrieves the compilation scheduler.
		/// </summary>
		/// <value>The compilation scheduler.</value>
		public ICompilationSchedulerStage Scheduler { get { return compilationScheduler; } }

		/// <summary>
		/// Provides access to the pipeline of this compiler.
		/// </summary>
		public CompilerPipeline Pipeline { get { return pipeline; } }

		/// <summary>
		/// Gets the type system.
		/// </summary>
		/// <value>The type system.</value>
		public ITypeSystem TypeSystem { get { return typeSystem; } }

		/// <summary>
		/// Gets the type layout.
		/// </summary>
		/// <value>The type layout.</value>
		public ITypeLayout TypeLayout { get { return typeLayout; } }

		/// <summary>
		/// Gets the internal logging interface
		/// </summary>
		/// <value>The log.</value>
		public IInternalTrace InternalLog { get { return internalTrace; } }

		/// <summary>
		/// Gets the exception clause header.
		/// </summary>
		/// <value>The exception clause header.</value>
		public ExceptionClauseHeader ExceptionClauseHeader { get { return exceptionClauseHeader; } }

		/// <summary>
		/// Gets the local variables.
		/// </summary>
		public Operand[] LocalVariables { get { return this.locals; } }

		/// <summary>
		/// Gets the assembly compiler.
		/// </summary>
		public AssemblyCompiler AssemblyCompiler { get { return assemblyCompiler; } }

		/// <summary>
		/// Gets the plug system.
		/// </summary>
		public IPlugSystem PlugSystem { get { return plugSystem; } }

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Requests a stream to emit native instructions to.
		/// </summary>
		/// <returns>A stream object, which can be used to store emitted instructions.</returns>
		public virtual Stream RequestCodeStream()
		{
			return linker.Allocate(method.ToString(), SectionKind.Text, 0, 0);
		}

		/// <summary>
		/// Compiles the method referenced by this method compiler.
		/// </summary>
		public void Compile()
		{
			BeginCompile();

			foreach (IMethodCompilerStage stage in Pipeline)
			{
				stage.Setup(this);
				stage.Run();

				Mosa.Compiler.InternalTrace.InstructionLogger.Run(this, stage);
			}

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
				return architecture.CreateResultOperand(type, nextStackSlot, nextStackSlot++);
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
			int stackSlot = nextStackSlot++;
			return new LocalVariableOperand(architecture.StackFrameRegister, String.Format(@"T_{0}", stackSlot), stackSlot, type);
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

			architecture = null;
			linker = null;
			method = null;
			type = null;
			instructionSet = null;
			basicBlocks = null;
		}

		/// <summary>
		/// Provides access to the instructions of the method.
		/// </summary>
		/// <returns>A stream, which represents the IL of the method.</returns>
		public Stream GetInstructionStream()
		{
			return method.Module.MetadataModule.GetInstructionStream((long)method.Rva);
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
			// PG: Isn't that implemented below? Comment seems out of date with code

			Debug.Assert(localsSig != null, @"Method doesn't have locals.");
			Debug.Assert(index < localsSig.Locals.Length, @"Invalid local index requested.");

			if (localsSig == null || localsSig.Locals.Length < index)
				throw new ArgumentOutOfRangeException(@"index", index, @"Invalid parameter index");

			Operand local = locals[index];

			if (local == null)
			{
				VariableSignature localVariable = localsSig.Locals[index];

				//ScheduleDependencyForCompilation(localVariable.Type);

				local = new LocalVariableOperand(architecture.StackFrameRegister, String.Format("L_{0}", index), index, localVariable.Type);

				locals[index] = local;
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
			MethodSignature sig = method.Signature;

			if (sig.HasThis || sig.HasExplicitThis)
			{
				if (index == 0)
				{
					var classSigType = new ClassSigType(type.Token);
					var signatureType =
						this.Method.DeclaringType.ContainsOpenGenericParameters ?
							assemblyCompiler.GenericTypePatcher.PatchSignatureType(this.Method.Module, this.Method.DeclaringType as CilGenericType, type.Token) :
							classSigType;

					return new ParameterOperand(
						architecture.StackFrameRegister,
						new RuntimeParameter(@"this", 2, ParameterAttributes.In),
						signatureType);
				}
				// Decrement the index, as the caller actually wants a real parameter
				--index;
			}

			// A normal argument, decode it...
			IList<RuntimeParameter> methodParameters = method.Parameters;
			Debug.Assert(methodParameters != null, @"Method doesn't have arguments.");
			Debug.Assert(index < methodParameters.Count, @"Invalid argument index requested.");
			if (methodParameters == null || methodParameters.Count <= index)
				throw new ArgumentOutOfRangeException(@"index", index, @"Invalid parameter index");

			Operand parameter = null;
			if (parameters.Count > index)
				parameter = parameters[index];

			if (parameter == null)
			{
				SigType parameterType = sig.Parameters[index];

				if (parameterType is GenericInstSigType && (parameterType as GenericInstSigType).ContainsGenericParameters)
				{
					var genericInstSigType = parameterType as GenericInstSigType;
					parameterType = assemblyCompiler.GenericTypePatcher.PatchSignatureType(typeSystem.InternalTypeModule, Method.DeclaringType, genericInstSigType.BaseType.Token);
				}
				parameter = new ParameterOperand(architecture.StackFrameRegister, methodParameters[index], parameterType);
				parameters[index] = parameter;
			}

			return parameter;
		}

		/// <summary>
		/// Sets the signature of local variables in the method.
		/// </summary>
		/// <param name="localVariableSignature">The local variable signature of the _method.</param>
		public void SetLocalVariableSignature(LocalVariableSignature localVariableSignature)
		{
			if (localVariableSignature == null)
				throw new ArgumentNullException(@"localVariableSignature");

			localsSig = localVariableSignature;

			locals = new Operand[localsSig.Locals.Length];

			nextStackSlot = locals.Length + 1;
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
		/// Gets the stage.
		/// </summary>
		/// <param name="stageType">Type of the stage.</param>
		/// <returns></returns>
		public IPipelineStage GetStage(Type stageType)
		{
			foreach (IPipelineStage stage in pipeline)
			{
				if (stageType.IsInstanceOfType(stage))
				{
					return stage;
				}
			}

			return null;
		}

		/// <summary>
		/// Retrieves a basic block from its label.
		/// </summary>
		/// <param name="label">The label of the basic block.</param>
		/// <returns>
		/// The basic block with the given label.
		/// </returns>
		public BasicBlock FromLabel(int label)
		{
			BasicBlock basicBlock = null;

			basicBlocksByLabel.TryGetValue(label, out basicBlock);

			return basicBlock;
		}

		/// <summary>
		/// Creates the block.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public BasicBlock CreateBlock(int label, int index)
		{
			BasicBlock basicBlock = new BasicBlock(basicBlocks.Count, label, index);
			basicBlocks.Add(basicBlock);
			basicBlocksByLabel.Add(label, basicBlock);
			return basicBlock;
		}

		#endregion // Methods

	}
}