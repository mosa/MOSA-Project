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
using System.IO;
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
	/// optimized jitting and others. BaseMethodCompiler instances are usually
	/// created by invoking CreateMethodCompiler on a specific compiler
	/// instance.
	/// </remarks>
	public class BaseMethodCompiler
	{

		#region Data Members

		/// <summary>
		/// Holds the pipeline of the compiler.
		/// </summary>
		protected readonly CompilerPipeline pipeline;

		/// <summary>
		/// 
		/// </summary>
		private readonly ICompilationScheduler compilationScheduler;

		/// <summary>
		/// The Architecture of the compilation target.
		/// </summary>
		private readonly IArchitecture architecture;

		/// <summary>
		/// Holds the linker used to resolve external symbols
		/// </summary>
		private readonly ILinker linker;

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
		private readonly RuntimeMethod method;

		/// <summary>
		/// Holds the type, which owns the method
		/// </summary>
		private readonly RuntimeType type;

		/// <summary>
		/// Holds the instruction set
		/// </summary>
		private readonly InstructionSet instructionSet;

		/// <summary>
		/// Holds the basic blocks
		/// </summary>
		private readonly BasicBlocks basicBlocks;

		/// <summary>
		/// Holds the type system during compilation
		/// </summary>
		protected readonly ITypeSystem typeSystem;

		/// <summary>
		/// Holds the type layout interface
		/// </summary>
		protected readonly ITypeLayout typeLayout;

		/// <summary>
		/// Holds the modules type system
		/// </summary>
		protected readonly ITypeModule moduleTypeSystem;

		/// <summary>
		/// Holds the internal logging interface
		/// </summary>
		protected readonly IInternalTrace internalTrace;

		/// <summary>
		/// Holds the exception clauses
		/// </summary>
		private readonly ExceptionClauseHeader exceptionClauseHeader = new ExceptionClauseHeader();

		/// <summary>
		/// Holds the compiler
		/// </summary>
		private readonly BaseCompiler compiler;

		/// <summary>
		/// Holds the stack layout
		/// </summary>
		private readonly StackLayout stackLayout;

		/// <summary>
		/// Holds the virtual register layout
		/// </summary>
		private readonly VirtualRegisterLayout virtualRegisterLayout;

		private bool stopMethodCompiler;

		#endregion // Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseMethodCompiler"/> class.
		/// </summary>
		/// <param name="compiler">The assembly compiler.</param>
		/// <param name="method">The method to compile by this instance.</param>
		/// <param name="instructionSet">The instruction set.</param>
		protected BaseMethodCompiler(BaseCompiler compiler, RuntimeMethod method, InstructionSet instructionSet)
		{
			this.compiler = compiler;
			this.method = method;
			this.type = method.DeclaringType;
			this.compilationScheduler = compiler.Scheduler;
			this.moduleTypeSystem = method.Module;
			this.architecture = compiler.Architecture;
			this.typeSystem = compiler.TypeSystem;
			this.typeLayout = Compiler.TypeLayout;
			this.internalTrace = Compiler.InternalTrace;
			this.linker = compiler.Linker;

			this.basicBlocks = new BasicBlocks();

			this.instructionSet = instructionSet ?? new InstructionSet(256);

			this.pipeline = new CompilerPipeline();

			this.stackLayout = new StackLayout(architecture, method.Parameters.Count + (method.Signature.HasThis || method.Signature.HasExplicitThis ? 1 : 0));

			this.virtualRegisterLayout = new VirtualRegisterLayout(architecture, stackLayout);

			EvaluateParameterOperands();

			this.stopMethodCompiler = false;
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
		public IMetadataModule Assembly { get { return moduleTypeSystem.MetadataModule; } }

		/// <summary>
		/// Gets the linker used to resolve external symbols.
		/// </summary>
		public ILinker Linker { get { return linker; } }

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
		public BasicBlocks BasicBlocks { get { return basicBlocks; } }

		/// <summary>
		/// Retrieves the compilation scheduler.
		/// </summary>
		/// <value>The compilation scheduler.</value>
		public ICompilationScheduler Scheduler { get { return compilationScheduler; } }

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
		public IInternalTrace InternalTrace { get { return internalTrace; } }

		/// <summary>
		/// Gets the exception clause header.
		/// </summary>
		/// <value>The exception clause header.</value>
		public ExceptionClauseHeader ExceptionClauseHeader { get { return exceptionClauseHeader; } }

		/// <summary>
		/// Gets the local variables.
		/// </summary>
		public Operand[] LocalVariables { get { return locals; } }

		/// <summary>
		/// Gets the assembly compiler.
		/// </summary>
		public BaseCompiler Compiler { get { return compiler; } }

		/// <summary>
		/// Gets the stack layout.
		/// </summary>
		public StackLayout StackLayout { get { return stackLayout; } }

		/// <summary>
		/// Gets the virtual register layout.
		/// </summary>
		public VirtualRegisterLayout VirtualRegisterLayout { get { return virtualRegisterLayout; } }

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Evaluates the local operands.
		/// </summary>
		protected void EvaluateLocalVariables()
		{
			int index = 0;
			locals = new Operand[localsSig.Locals.Length];

			foreach (var localVariable in localsSig.Locals)
			{
				locals[index++] = stackLayout.AllocateStackOperand(localVariable.Type, true);
				//Scheduler.ScheduleTypeForCompilation(localVariable.Type); // TODO
			}

		}

		/// <summary>
		/// Evaluates the parameter operands.
		/// </summary>
		protected void EvaluateParameterOperands()
		{
			int index = 0;

			if (method.Signature.HasThis || method.Signature.HasExplicitThis)
			{
				var signatureType = Method.DeclaringType.ContainsOpenGenericParameters
					? compiler.GenericTypePatcher.PatchSignatureType(Method.Module, Method.DeclaringType as CilGenericType, type.Token)
					: new ClassSigType(type.Token);

				stackLayout.SetStackParameter(index++, new RuntimeParameter(@"this", 2, ParameterAttributes.In), signatureType);
			}

			for (int paramIndex = 0; paramIndex < method.Signature.Parameters.Length; paramIndex++)
			{
				var parameterType = method.Signature.Parameters[paramIndex];

				if (parameterType is GenericInstSigType && (parameterType as GenericInstSigType).ContainsGenericParameters)
				{
					parameterType = compiler.GenericTypePatcher.PatchSignatureType(typeSystem.InternalTypeModule, Method.DeclaringType, (parameterType as GenericInstSigType).BaseType.Token);
				}

				stackLayout.SetStackParameter(index++, method.Parameters[paramIndex], parameterType);
			}

		}

		/// <summary>
		/// Requests a stream to emit native instructions to.
		/// </summary>
		/// <returns>A stream object, which can be used to store emitted instructions.</returns>
		public virtual Stream RequestCodeStream()
		{
			return linker.Allocate(method.FullName, SectionKind.Text, 0, 0);
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

				if (stopMethodCompiler)
					break;
			}

			EndCompile();
		}

		/// <summary>
		/// Stops the method compiler.
		/// </summary>
		/// <returns></returns>
		public void StopMethodCompiler() { stopMethodCompiler = true; } 

		/// <summary>
		/// Creates a new virtual register operand.
		/// </summary>
		/// <param name="type">The signature type of the virtual register.</param>
		/// <returns>
		/// An operand, which represents the virtual register.
		/// </returns>
		public Operand CreateVirtualRegister(SigType type)
		{
			//return virtualRegisterLayout.AllocateVirtualRegister(type);
			return stackLayout.AllocateStackOperand(type, false);
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
			return stackLayout.GetStackOperand(index);
		}

		/// <summary>
		/// Retrieves the parameter operand at the specified <paramref name="index"/>.
		/// </summary>
		/// <param name="index">The index of the parameter operand to retrieve.</param>
		/// <returns>The operand at the specified index.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">The <paramref name="index"/> is not valid.</exception>
		public Operand GetParameterOperand(int index)
		{
			return stackLayout.GetStackParameter(index);
		}

		/// <summary>
		/// Gets the parameters.
		/// </summary>
		public Operand[] Parameters { get { return stackLayout.Parameters; } }

		/// <summary>
		/// Sets the signature of local variables in the method.
		/// </summary>
		/// <param name="localVariableSignature">The local variable signature of the _method.</param>
		public void SetLocalVariableSignature(LocalVariableSignature localVariableSignature)
		{
			if (localVariableSignature == null)
				throw new ArgumentNullException(@"localVariableSignature");

			localsSig = localVariableSignature;

			EvaluateLocalVariables();
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

		#endregion // Methods

	}
}