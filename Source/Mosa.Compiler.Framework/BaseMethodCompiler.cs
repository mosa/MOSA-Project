// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.CompilerStages;
using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Compiler.Trace;
using System;
using System.Collections.Generic;

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
		/// The empty operand list
		/// </summary>
		private static readonly Operand[] emptyOperandList = new Operand[0];

		/// <summary>
		/// The stack frame
		/// </summary>
		public Operand StackFrame;

		/// <summary>
		/// The constant zero
		/// </summary>
		public Operand ConstantZero;

		/// <summary>
		/// Holds flag that will stop method compiler
		/// </summary>
		private bool stop;

		#endregion Data Members

		#region Properties

		/// <summary>
		/// Gets the Architecture to compile for.
		/// </summary>
		public BaseArchitecture Architecture { get; }

		/// <summary>
		/// Gets the linker used to resolve external symbols.
		/// </summary>
		public BaseLinker Linker { get; }

		/// <summary>
		/// Gets the method implementation being compiled.
		/// </summary>
		public MosaMethod Method { get; }

		/// <summary>
		/// Gets the owner type of the method.
		/// </summary>
		public MosaType Type { get; }

		/// <summary>
		/// Gets the basic blocks.
		/// </summary>
		/// <value>The basic blocks.</value>
		public BasicBlocks BasicBlocks { get; }

		/// <summary>
		/// Retrieves the compilation scheduler.
		/// </summary>
		/// <value>The compilation scheduler.</value>
		public CompilationScheduler Scheduler { get; }

		/// <summary>
		/// Provides access to the pipeline of this compiler.
		/// </summary>
		public CompilerPipeline Pipeline { get; }

		/// <summary>
		/// Gets the type system.
		/// </summary>
		/// <value>The type system.</value>
		public TypeSystem TypeSystem { get; }

		/// <summary>
		/// Gets the type layout.
		/// </summary>
		/// <value>The type layout.</value>
		public MosaTypeLayout TypeLayout { get; }

		/// <summary>
		/// Gets the compiler trace handle
		/// </summary>
		/// <value>The log.</value>
		public CompilerTrace Trace { get; }

		/// <summary>
		/// Gets the local variables.
		/// </summary>
		public Operand[] LocalVariables { get; private set; }

		/// <summary>
		/// Gets the assembly compiler.
		/// </summary>
		public BaseCompiler Compiler { get; }

		/// <summary>
		/// Gets the stack.
		/// </summary>
		public List<Operand> LocalStack { get; }

		/// <summary>
		/// Gets or sets the size of the stack.
		/// </summary>
		/// <value>
		/// The size of the stack memory.
		/// </value>
		public int StackSize { get; set; }

		/// <summary>
		/// Gets the virtual register layout.
		/// </summary>
		public VirtualRegisters VirtualRegisters { get; }

		/// <summary>
		/// Gets the dominance analysis.
		/// </summary>
		public Dominance DominanceAnalysis { get; }

		/// <summary>
		/// Gets the parameters.
		/// </summary>
		public Operand[] Parameters { get; }

		/// <summary>
		/// Gets the protected regions.
		/// </summary>
		/// <value>
		/// The protected regions.
		/// </value>
		public IList<ProtectedRegion> ProtectedRegions { get; private set; }

		/// <summary>
		/// Gets a value indicating whether [plugged method].
		/// </summary>
		public MosaMethod PluggedMethod { get; }

		/// <summary>
		/// Gets a value indicating whether this method is plugged.
		/// </summary>
		public bool IsPlugged { get { return PluggedMethod != null; } }

		/// <summary>
		/// Gets the thread identifier.
		/// </summary>
		/// <value>
		/// The thread identifier.
		/// </value>
		public int ThreadID { get; }

		/// <summary>
		/// Gets the method data.
		/// </summary>
		/// <value>
		/// The method data.
		/// </value>
		public CompilerMethodData MethodData { get; }

		#endregion Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseMethodCompiler" /> class.
		/// </summary>
		/// <param name="compiler">The assembly compiler.</param>
		/// <param name="method">The method to compile by this instance.</param>
		/// <param name="basicBlocks">The basic blocks.</param>
		/// <param name="threadID">The thread identifier.</param>
		protected BaseMethodCompiler(BaseCompiler compiler, MosaMethod method, BasicBlocks basicBlocks, int threadID)
		{
			Compiler = compiler;
			Method = method;
			Type = method.DeclaringType;
			Scheduler = compiler.CompilationScheduler;
			Architecture = compiler.Architecture;
			TypeSystem = compiler.TypeSystem;
			TypeLayout = compiler.TypeLayout;
			Trace = compiler.CompilerTrace;
			Linker = compiler.Linker;
			BasicBlocks = basicBlocks ?? new BasicBlocks();
			Pipeline = new CompilerPipeline();
			LocalStack = new List<Operand>();

			ConstantZero = Operand.CreateConstant(TypeSystem, 0);
			StackFrame = Operand.CreateCPURegister(TypeSystem.BuiltIn.Pointer, Architecture.StackFrameRegister);

			Parameters = new Operand[method.Signature.Parameters.Count + (method.HasThis || method.HasExplicitThis ? 1 : 0)];
			VirtualRegisters = new VirtualRegisters();
			LocalVariables = emptyOperandList;
			ThreadID = threadID;
			DominanceAnalysis = new Dominance(Compiler.CompilerOptions.DominanceAnalysisFactory, BasicBlocks);
			PluggedMethod = compiler.PlugSystem.GetPlugMethod(Method);
			stop = false;

			MethodData = compiler.CompilerData.GetCompilerMethodData(Method);
			MethodData.Counters.Clear();

			EvaluateParameterOperands();

			CalculateMethodParameterSize();
		}

		#endregion Construction

		#region Methods

		private void CalculateMethodParameterSize()
		{
			int stacksize = 0;

			if (Method.HasThis)
			{
				stacksize = TypeLayout.NativePointerSize;
			}

			foreach (var parameter in Method.Signature.Parameters)
			{
				var size = parameter.ParameterType.IsValueType ? TypeLayout.GetTypeSize(parameter.ParameterType) : TypeLayout.NativePointerAlignment;
				stacksize += Alignment.AlignUp(size, TypeLayout.NativePointerAlignment);
			}

			MethodData.ParameterStackSize = stacksize;
		}

		/// <summary>
		/// Adds the stack local.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public Operand AddStackLocal(MosaType type)
		{
			return AddStackLocal(type, false);
		}

		/// <summary>
		/// Adds the stack local.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="pinned">if set to <c>true</c> [pinned].</param>
		/// <returns></returns>
		public Operand AddStackLocal(MosaType type, bool pinned)
		{
			var local = Operand.CreateStackLocal(type, LocalStack.Count, pinned);
			LocalStack.Add(local);
			return local;
		}

		/// <summary>
		/// Sets the stack parameter.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="type">The type.</param>
		/// <param name="name">The name.</param>
		/// <param name="isThis">if set to <c>true</c> [is this].</param>
		/// <returns></returns>
		private Operand SetStackParameter(int index, MosaType type, string name, bool isThis)
		{
			var param = Operand.CreateStackParameter(type, index, name, isThis);
			Parameters[index] = param;
			return param;
		}

		/// <summary>
		/// Evaluates the parameter operands.
		/// </summary>
		protected void EvaluateParameterOperands()
		{
			int index = 0;

			if (Method.HasThis || Method.HasExplicitThis)
			{
				if (Type.IsValueType)
					SetStackParameter(index++, Type.ToManagedPointer(), "this", true);
				else
					SetStackParameter(index++, Type, "this", true);
			}

			foreach (var parameter in Method.Signature.Parameters)
			{
				SetStackParameter(index++, parameter.ParameterType, parameter.Name, false);
			}

			LayoutParameters();
		}

		private void LayoutParameters()
		{
			int returnSize = 0;

			if (MosaTypeLayout.IsStoredOnStack(Method.Signature.ReturnType))
			{
				returnSize = TypeLayout.GetTypeSize(Method.Signature.ReturnType);
			}

			LayoutParameters(Architecture.OffsetOfFirstParameter + returnSize);
		}

		private int LayoutParameters(int offsetOfFirst)
		{
			int offset = offsetOfFirst;

			foreach (var operand in Parameters)
			{
				GetTypeRequirements(operand.Type, out int size, out int alignment);

				operand.Offset = offset;
				operand.IsResolved = true;

				size = Alignment.AlignUp(size, alignment);
				offset += size;
			}

			return offset;
		}

		/// <summary>
		/// Compiles the method referenced by this method compiler.
		/// </summary>
		public void Compile()
		{
			BeginCompile();

			foreach (IMethodCompilerStage stage in Pipeline)
			{
				{
					stage.Initialize(this);
					stage.Execute();

					InstructionLogger.Run(this, stage);

					if (stop)
						break;
				}
			}

			InitializeType();

			var log = new TraceLog(TraceType.Counters, this.Method, string.Empty, Trace.TraceFilter.Active);
			log.Log(MethodData.Counters.Export());
			Trace.TraceListener.OnNewTraceLog(log);

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
		/// Allocates the virtual register or stack slot.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public Operand AllocateVirtualRegisterOrStackSlot(MosaType type)
		{
			if (MosaTypeLayout.IsStoredOnStack(type))
			{
				return AddStackLocal(type);
			}
			else
			{
				return CreateVirtualRegister(type.GetStackType());
			}
		}

		/// <summary>
		/// Allocates the local variable virtual registers.
		/// </summary>
		/// <param name="locals">The locals.</param>
		public void SetLocalVariables(IList<MosaLocal> locals)
		{
			LocalVariables = new Operand[locals.Count];

			int index = 0;
			foreach (var local in locals)
			{
				Operand operand = null;

				if (!MosaTypeLayout.IsStoredOnStack(local.Type) && !local.IsPinned)
				{
					var stacktype = local.Type.GetStackType();
					operand = CreateVirtualRegister(stacktype);
				}
				else
				{
					operand = AddStackLocal(local.Type, local.IsPinned);
				}

				LocalVariables[index++] = operand;
			}
		}

		/// <summary>
		/// Gets the type requirements.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="size">The size.</param>
		/// <param name="alignment">The alignment.</param>
		public void GetTypeRequirements(MosaType type, out int size, out int alignment)
		{
			alignment = Architecture.NativeAlignment;

			size = type.IsValueType ? TypeLayout.GetTypeSize(type) : alignment;
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
				typeInitializer = Compiler.CompilePipeline.FindFirst<TypeInitializerSchedulerStage>();

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
