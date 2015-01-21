/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Compiler.Trace;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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
		/// Holds the type initializer scheduler stage
		/// </summary>
		private TypeInitializerSchedulerStage typeInitializer;

		/// <summary>
		/// Holds flag that will stop method compiler
		/// </summary>
		private bool stop;

		/// <summary>
		/// The empty operand list
		/// </summary>
		private static readonly Operand[] emptyOperandList = new Operand[0];

		/// <summary>
		/// The basic block operands
		/// </summary>
		private Dictionary<BasicBlock, Operand> basicBlockOperands;

		#endregion Data Members

		#region Properties

		/// <summary>
		/// Gets the Architecture to compile for.
		/// </summary>
		public BaseArchitecture Architecture { get; private set; }

		/// <summary>
		/// Gets the linker used to resolve external symbols.
		/// </summary>
		public BaseLinker Linker { get; private set; }

		/// <summary>
		/// Gets the method implementation being compiled.
		/// </summary>
		public MosaMethod Method { get; private set; }

		/// <summary>
		/// Gets the owner type of the method.
		/// </summary>
		public MosaType Type { get; private set; }

		/// <summary>
		/// Gets the instruction set.
		/// </summary>
		/// <value>The instruction set.</value>
		public InstructionSet InstructionSet { get; private set; }

		/// <summary>
		/// Gets the basic blocks.
		/// </summary>
		/// <value>The basic blocks.</value>
		public BasicBlocks BasicBlocks { get; private set; }

		/// <summary>
		/// Retrieves the compilation scheduler.
		/// </summary>
		/// <value>The compilation scheduler.</value>
		public CompilationScheduler Scheduler { get; private set; }

		/// <summary>
		/// Provides access to the pipeline of this compiler.
		/// </summary>
		public CompilerPipeline Pipeline { get; private set; }

		/// <summary>
		/// Gets the type system.
		/// </summary>
		/// <value>The type system.</value>
		public TypeSystem TypeSystem { get; private set; }

		/// <summary>
		/// Gets the type layout.
		/// </summary>
		/// <value>The type layout.</value>
		public MosaTypeLayout TypeLayout { get; private set; }

		/// <summary>
		/// Gets the internal logging interface
		/// </summary>
		/// <value>The log.</value>
		public CompilerTrace Trace { get; private set; }

		/// <summary>
		/// Gets the local variables.
		/// </summary>
		public Operand[] LocalVariables { get; private set; }

		/// <summary>
		/// Gets the assembly compiler.
		/// </summary>
		public BaseCompiler Compiler { get; private set; }

		/// <summary>
		/// Gets the stack layout.
		/// </summary>
		public StackLayout StackLayout { get; private set; }

		/// <summary>
		/// Gets the virtual register layout.
		/// </summary>
		public VirtualRegisters VirtualRegisters { get; private set; }

		/// <summary>
		/// Gets the dominance analysis.
		/// </summary>
		public Dominance DominanceAnalysis { get; private set; }

		/// <summary>
		/// Gets the parameters.
		/// </summary>
		public Operand[] Parameters { get { return StackLayout.Parameters; } }

		/// <summary>
		/// Gets the protected regions.
		/// </summary>
		/// <value>
		/// The protected regions.
		/// </value>
		public IList<ProtectedRegion> ProtectedRegions { get; private set; }

		/// <summary>
		/// Gets the thread identifier.
		/// </summary>
		/// <value>
		/// The thread identifier.
		/// </value>
		public int ThreadID { get; private set; }

		#endregion Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseMethodCompiler" /> class.
		/// </summary>
		/// <param name="compiler">The assembly compiler.</param>
		/// <param name="method">The method to compile by this instance.</param>
		/// <param name="basicBlocks">The basic blocks.</param>
		/// <param name="instructionSet">The instruction set.</param>
		/// <param name="threadID">The thread identifier.</param>
		protected BaseMethodCompiler(BaseCompiler compiler, MosaMethod method, BasicBlocks basicBlocks, InstructionSet instructionSet, int threadID)
		{
			this.Compiler = compiler;
			this.Method = method;
			this.Type = method.DeclaringType;
			this.Scheduler = compiler.CompilationScheduler;
			this.Architecture = compiler.Architecture;
			this.TypeSystem = compiler.TypeSystem;
			this.TypeLayout = Compiler.TypeLayout;
			this.Trace = Compiler.CompilerTrace;
			this.Linker = compiler.Linker;
			this.BasicBlocks = basicBlocks ?? new BasicBlocks();
			this.InstructionSet = instructionSet ?? new InstructionSet(256);
			this.Pipeline = new CompilerPipeline();
			this.StackLayout = new StackLayout(Architecture, method.Signature.Parameters.Count + (method.HasThis || method.HasExplicitThis ? 1 : 0));
			this.VirtualRegisters = new VirtualRegisters(Architecture);
			this.LocalVariables = emptyOperandList;
			this.ThreadID = threadID;
			this.DominanceAnalysis = new Dominance(Compiler.CompilerOptions.DominanceAnalysisFactory, this.BasicBlocks);

			EvaluateParameterOperands();

			this.stop = false;

			Debug.Assert(this.Linker != null);
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Evaluates the parameter operands.
		/// </summary>
		protected void EvaluateParameterOperands()
		{
			int index = 0;

			//FIXME! Note: displacement is recalculated later
			int displacement = 4;

			if (Method.HasThis || Method.HasExplicitThis)
			{
				if (Type.IsValueType)
					StackLayout.SetStackParameter(index++, Type.ToManagedPointer(), displacement, "this");
				else
					StackLayout.SetStackParameter(index++, Type, displacement, "this");
			}

			foreach (var parameter in Method.Signature.Parameters)
			{
				StackLayout.SetStackParameter(index++, parameter.ParameterType, displacement, parameter.Name);
			}
		}

		/// <summary>
		/// Compiles the method referenced by this method compiler.
		/// </summary>
		public void Compile()
		{
			BeginCompile();

			foreach (IMethodCompilerStage stage in Pipeline)
			{
				//try
				{
					stage.Initialize(this);
					stage.Execute();

					Mosa.Compiler.Trace.InstructionLogger.Run(this, stage);

					if (stop)
						break;
				}
				//catch (Exception e)
				//{
				//	InternalTrace.TraceListener.SubmitDebugStageInformation(Method, stage.Name + "-Exception", e.ToString());
				//	InternalTrace.CompilerEventListener.SubmitTraceEvent(CompilerEvent.Exception, Method.FullName + " @ " + stage.Name);
				//	return;
				//}
			}

			InitializeType();

			EndCompile();
		}

		/// <summary>
		/// Stops the method compiler.
		/// </summary>
		/// <returns></returns>
		public void Stop()
		{
			stop = true;
		}

		/// <summary>
		/// Creates a new virtual register operand.
		/// </summary>
		/// <param name="type">The signature type of the virtual register.</param>
		/// <returns>
		/// An operand, which represents the virtual register.
		/// </returns>
		public Operand CreateVirtualRegister(MosaType type)
		{
			return VirtualRegisters.Allocate(type);
		}

		/// <summary>
		/// Retrieves the local stack operand at the specified <paramref name="index" />.
		/// </summary>
		/// <param name="index">The index of the stack operand to retrieve.</param>
		/// <returns>
		/// The operand at the specified index.
		/// </returns>
		/// <exception cref="System.ArgumentOutOfRangeException">The <paramref name="index" /> is not valid.</exception>
		public Operand GetLocalOperand(int index)
		{
			return LocalVariables[index];
		}

		/// <summary>
		/// Retrieves the parameter operand at the specified <paramref name="index"/>.
		/// </summary>
		/// <param name="index">The index of the parameter operand to retrieve.</param>
		/// <returns>The operand at the specified index.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">The <paramref name="index"/> is not valid.</exception>
		public Operand GetParameterOperand(int index)
		{
			return StackLayout.GetStackParameter(index);
		}

		public Operand GetBasicBlockOperand(BasicBlock basicBlock)
		{
			// very lazy initialization
			if (basicBlockOperands == null)
			{
				basicBlockOperands = new Dictionary<BasicBlock, Operand>();
			}

			Operand operand = null;

			if (!basicBlockOperands.TryGetValue(basicBlock, out operand))
			{
				operand = Operand.CreateBasicBlock(TypeSystem, basicBlock);
				basicBlockOperands.Add(basicBlock, operand);
			}

			return operand;
		}

		/// <summary>
		/// Allocates the local variable virtual registers.
		/// </summary>
		/// <param name="locals">The locals.</param>
		public void SetLocalVariables(IList<MosaLocal> locals)
		{
			LocalVariables = new Operand[locals.Count];

			for (int index = 0; index < locals.Count; index++)
			{
				var local = locals[index];
				Operand operand;

				if (local.Type.IsValueType)
				{
					operand = StackLayout.AddStackLocal(local.Type);
				}
				else
				{
					var stacktype = local.Type.GetStackType();
					operand = VirtualRegisters.Allocate(stacktype);
				}

				LocalVariables[index] = operand;
			}
		}

		/// <summary>
		/// Sets the protected regions.
		/// </summary>
		/// <param name="protectedRegions">The protected regions.</param>
		public void SetProtectedRegions(IList<ProtectedRegion> protectedRegions)
		{
			ProtectedRegions = protectedRegions;
		}

		/// <summary>
		/// Initializes the type.
		/// </summary>
		protected virtual void InitializeType()
		{
			if (Method.IsSpecialName && Method.IsRTSpecialName && Method.IsStatic && Method.Name == ".cctor")
			{
				typeInitializer = Compiler.PostCompilePipeline.FindFirst<TypeInitializerSchedulerStage>();

				if (typeInitializer == null)
					return;

				typeInitializer.Schedule(Method);
			}
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
			foreach (IPipelineStage stage in Pipeline)
			{
				if (stageType.IsInstanceOfType(stage))
				{
					return stage;
				}
			}

			return null;
		}

		public string FormatStageName(IPipelineStage stage)
		{
			return "[" + Pipeline.GetPosition(stage).ToString("00") + "] " + stage.Name;
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return Method.ToString();
		}

		#endregion Methods
	}
}
