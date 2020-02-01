// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Diagnostics;
using static Mosa.Compiler.Framework.CompilerHooks;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Base class of a method compiler.
	/// </summary>
	/// <remarks>
	/// A method compiler is responsible for compiling a single method of an object.
	/// </remarks>
	public sealed class MethodCompiler
	{
		#region Data Members

		/// <summary>
		/// The empty operand list
		/// </summary>
		private static readonly Operand[] emptyOperandList = System.Array.Empty<Operand>();

		private readonly Stopwatch Stopwatch;

		private NotifyTraceLogHandler NotifyTraceLogHandler;

		private readonly bool Statistics;

		#endregion Data Members

		#region Properties

		/// <summary>
		/// Gets the Architecture to compile for.
		/// </summary>
		public BaseArchitecture Architecture { get; }

		/// <summary>
		/// Gets the linker used to resolve external symbols.
		/// </summary>
		public MosaLinker Linker { get; }

		/// <summary>
		/// Gets the method implementation being compiled.
		/// </summary>
		public MosaMethod Method { get; }

		/// <summary>
		/// Gets the basic blocks.
		/// </summary>
		/// <value>The basic blocks.</value>
		public BasicBlocks BasicBlocks { get; }

		/// <summary>
		/// Retrieves the compilation scheduler.
		/// </summary>
		/// <value>The compilation scheduler.</value>
		public MethodScheduler MethodScheduler { get; }

		/// <summary>
		/// Provides access to the pipeline of this compiler.
		/// </summary>
		public Pipeline<BaseMethodCompilerStage> Pipeline { get; set; }

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
		/// Gets the local variables.
		/// </summary>
		public Operand[] LocalVariables { get; private set; }

		/// <summary>
		/// Gets the assembly compiler.
		/// </summary>
		public Compiler Compiler { get; }

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
		/// Gets the parameters.
		/// </summary>
		public Operand[] Parameters { get; }

		/// <summary>
		/// Gets the protected regions.
		/// </summary>
		public List<ProtectedRegion> ProtectedRegions { get; set; }

		public bool HasProtectedRegions { get; private set; }

		/// <summary>
		/// The labels
		/// </summary>
		public Dictionary<int, int> Labels { get; set; }

		/// <summary>
		/// Gets the thread identifier.
		/// </summary>
		public int ThreadID { get; }

		/// <summary>
		/// Gets the compiler method data.
		/// </summary>
		public MethodData MethodData { get; }

		/// <summary>
		/// Gets the platform constant zero
		/// </summary>
		public Operand ConstantZero { get; }

		/// <summary>
		/// Gets the 32-bit constant zero.
		/// </summary>
		public Operand ConstantZero32 { get; }

		/// <summary>
		/// Gets the 64-bit constant zero.
		/// </summary>
		public Operand ConstantZero64 { get; }

		/// <summary>
		/// Gets the R4 constant zero.
		/// </summary>
		public Operand ConstantZeroR4 { get; }

		/// <summary>
		/// Gets the R4 constant zero.
		/// </summary>
		public Operand ConstantZeroR8 { get; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is execute pipeline.
		/// </summary>
		public bool IsExecutePipeline { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this method requires CIL decoding .
		/// </summary>
		public bool IsCILDecodeRequired { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this method is plugged.
		/// </summary>
		public bool IsMethodPlugged { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is stack frame required.
		/// </summary>
		public bool IsStackFrameRequired { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is method inlined.
		/// </summary>
		public bool IsMethodInlined { get; set; }

		/// <summary>
		/// Holds flag that will stop method compiler
		/// </summary>
		public bool IsStopped { get; private set; }

		/// <summary>
		/// Gets the linker symbol.
		/// </summary>
		public LinkerSymbol Symbol { get { return MethodData.Symbol; } set { MethodData.Symbol = value; } }

		/// <summary>
		/// Gets the method scanner.
		/// </summary>
		public MethodScanner MethodScanner { get; }

		public CompilerHooks CompilerHooks { get; }

		#endregion Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodCompiler" /> class.
		/// </summary>
		/// <param name="compiler">The assembly compiler.</param>
		/// <param name="method">The method to compile by this instance.</param>
		/// <param name="basicBlocks">The basic blocks.</param>
		/// <param name="threadID">The thread identifier.</param>
		public MethodCompiler(Compiler compiler, MosaMethod method, BasicBlocks basicBlocks, int threadID)
		{
			Stopwatch = Stopwatch.StartNew();

			Compiler = compiler;
			Method = method;
			MethodScheduler = compiler.MethodScheduler;
			Architecture = compiler.Architecture;
			TypeSystem = compiler.TypeSystem;
			TypeLayout = compiler.TypeLayout;
			Linker = compiler.Linker;
			MethodScanner = compiler.MethodScanner;
			CompilerHooks = compiler.CompilerHooks;

			NotifyTraceLogHandler = GetMethodInstructionTraceHandler();
			Statistics = compiler.Statistics;

			BasicBlocks = basicBlocks ?? new BasicBlocks();
			LocalStack = new List<Operand>();
			VirtualRegisters = new VirtualRegisters();

			Parameters = new Operand[method.Signature.Parameters.Count + (method.HasThis || method.HasExplicitThis ? 1 : 0)];

			ConstantZero32 = CreateConstant((uint)0);
			ConstantZero64 = CreateConstant((ulong)0);
			ConstantZeroR4 = CreateConstant(0.0f);
			ConstantZeroR8 = CreateConstant(0.0d);

			ConstantZero = Architecture.Is32BitPlatform ? ConstantZero32 : ConstantZero64;

			LocalVariables = emptyOperandList;
			ThreadID = threadID;

			IsStopped = false;
			IsExecutePipeline = true;
			IsCILDecodeRequired = true;
			IsStackFrameRequired = true;
			IsMethodInlined = false;
			HasProtectedRegions = Method.ExceptionHandlers.Count != 0;

			MethodData = Compiler.GetMethodData(Method);
			MethodData.Counters.Reset();

			MethodData.Version++;
			MethodData.IsMethodImplementationReplaced = IsMethodPlugged;

			if (Symbol == null)
			{
				Symbol = Linker.DefineSymbol(Method.FullName, SectionKind.Text, 0, 0);
				Symbol.MethodData = MethodData; // for debugging
				Symbol.MosaMethod = Method; // for debugging
			}
			else
			{
				Symbol.RemovePatches();
			}

			EvaluateParameterOperands();

			CalculateMethodParameterSize();

			MethodData.Counters.NewCountSkipLock("ExecutionTime.Setup.Ticks", (int)Stopwatch.ElapsedTicks);
		}

		#endregion Construction

		#region Methods

		private void CalculateMethodParameterSize()
		{
			// Check if already computed
			if (MethodData.ParameterStackSize != 0)
				return;

			int stacksize = 0;

			MethodData.ParameterSizes = new List<int>(Method.Signature.Parameters.Count);
			MethodData.ParameterOffsets = new List<int>(Method.Signature.Parameters.Count);

			if (Method.HasThis)
			{
				stacksize = TypeLayout.NativePointerSize;
			}

			foreach (var parameter in Method.Signature.Parameters)
			{
				var size = parameter.ParameterType.IsValueType ? TypeLayout.GetTypeSize(parameter.ParameterType) : TypeLayout.NativePointerAlignment;

				MethodData.ParameterSizes.Add(size);
				MethodData.ParameterOffsets.Add(stacksize);

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
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		private Operand SetStackParameter(int index, MosaType type, string name, bool isThis, int offset)
		{
			var param = Operand.CreateStackParameter(type, index, name, isThis, offset);
			Parameters[index] = param;
			return param;
		}

		/// <summary>
		/// Evaluates the parameter operands.
		/// </summary>
		private void EvaluateParameterOperands()
		{
			int offset = Architecture.OffsetOfFirstParameter;

			if (!MosaTypeLayout.CanFitInRegister(Method.Signature.ReturnType))
			{
				offset += TypeLayout.GetTypeSize(Method.Signature.ReturnType);
			}

			int index = 0;

			if (Method.HasThis || Method.HasExplicitThis)
			{
				if (Method.DeclaringType.IsValueType)
				{
					var ptr = Method.DeclaringType.ToManagedPointer();
					SetStackParameter(index++, ptr, "this", true, offset);

					var size = GetReferenceOrTypeSize(ptr, true);
					offset += size;
				}
				else
				{
					SetStackParameter(index++, Method.DeclaringType, "this", true, offset);

					var size = GetReferenceOrTypeSize(Method.DeclaringType, true);
					offset += size;
				}
			}

			foreach (var parameter in Method.Signature.Parameters)
			{
				SetStackParameter(index++, parameter.ParameterType, parameter.Name, false, offset);

				var size = GetReferenceOrTypeSize(parameter.ParameterType, true);
				offset += size;
			}
		}

		/// <summary>
		/// Compiles the method referenced by this method compiler.
		/// </summary>
		public void Compile()
		{
			MethodData.HasCode = false;

			if (Method.IsCompilerGenerated)
			{
				IsCILDecodeRequired = false;
				IsStackFrameRequired = false;
			}

			//Debug.WriteLine($"{MethodScheduler.GetTimestamp()} - Compiling: [{MethodData.Version}] {Method}"); //DEBUGREMOVE

			PlugMethod();

			PatchDelegate();

			ExternalMethod();

			InternalMethod();

			ExecutePipeline();

			//Debug.WriteLine($"{MethodScheduler.GetTimestamp()} - Compiled: [{MethodData.Version}] {Method}"); //DEBUGREMOVE

			Symbol.SetReplacementStatus(MethodData.Inlined);

			if (Statistics)
			{
				var log = new TraceLog(TraceType.MethodCounters, Method, string.Empty, MethodData.Version);
				log.Log(MethodData.Counters.Export());

				Compiler.PostTraceLog(log);
			}
		}

		private void ExecutePipeline()
		{
			if (!IsExecutePipeline)
				return;

			var executionTimes = new long[Pipeline.Count];

			var startTick = Stopwatch.ElapsedTicks;

			for (int i = 0; i < Pipeline.Count; i++)
			{
				var stage = Pipeline[i];

				stage.Setup(this, i);
				stage.Execute();

				executionTimes[i] = Stopwatch.ElapsedTicks;

				CreateInstructionTrace(stage);

				if (IsStopped || IsMethodInlined)
					break;
			}

			if (Statistics)
			{
				var lastTick = Stopwatch.ElapsedTicks;

				MethodData.ElapsedTicks = lastTick;

				MethodData.Counters.NewCountSkipLock("ExecutionTime.StageStart.Ticks", (int)startTick);
				MethodData.Counters.NewCountSkipLock("ExecutionTime.Total.Ticks", (int)lastTick);

				var executionTimeLog = new TraceLog(TraceType.MethodDebug, Method, "Execution Time/Ticks", MethodData.Version);

				long previousTick = startTick;
				var totalTick = lastTick - startTick;

				for (int i = 0; i < Pipeline.Count; i++)
				{
					var pipelineTick = executionTimes[i];
					var ticks = pipelineTick == 0 ? 0 : pipelineTick - previousTick;
					var percentage = totalTick == 0 ? 0 : (ticks * 100) / (double)(totalTick);
					previousTick = pipelineTick;

					int per = (int)percentage / 5;

					var entry = $"[{i:00}] {Pipeline[i].Name.PadRight(45)} : {percentage:00.00} % [{string.Empty.PadRight(per, '#').PadRight(20, ' ')}] ({ticks})";

					executionTimeLog.Log(entry);

					MethodData.Counters.NewCountSkipLock($"ExecutionTime.{i:00}.{Pipeline[i].Name}.Ticks", (int)ticks);
				}

				executionTimeLog.Log($"{"****Total Time".PadRight(57)}({lastTick})");

				PostTraceLog(executionTimeLog);
			}
		}

		private void CreateInstructionTrace(BaseMethodCompilerStage stage)
		{
			if (NotifyTraceLogHandler == null)
				return;

			InstructionTrace.Run(this, stage, NotifyTraceLogHandler);
		}

		private NotifyTraceLogHandler GetMethodInstructionTraceHandler()
		{
			if (CompilerHooks.NotifyMethodInstructionTrace == null)
				return null;

			return CompilerHooks.NotifyMethodInstructionTrace.Invoke(Method);
		}

		private void PlugMethod()
		{
			var plugMethod = Compiler.PlugSystem.GetReplacement(Method);

			if (plugMethod == null)
				return;

			MethodData.ReplacedBy = plugMethod;

			Compiler.MethodScanner.MethodInvoked(plugMethod, Method);

			IsMethodPlugged = true;
			IsCILDecodeRequired = false;
			IsExecutePipeline = false;
			IsStackFrameRequired = false;

			if (NotifyTraceLogHandler != null)
			{
				var traceLog = new TraceLog(TraceType.MethodInstructions, Method, "XX-Plugged Method", MethodData.Version);
				traceLog?.Log($"Plugged by {plugMethod.FullName}");

				NotifyTraceLogHandler.Invoke(traceLog);
			}
		}

		public bool IsTraceable(int tracelevel)
		{
			return Compiler.IsTraceable(tracelevel);
		}

		private void PostTraceLog(TraceLog traceLog)
		{
			Compiler.PostTraceLog(traceLog);
		}

		private void PatchDelegate()
		{
			if (Method.HasImplementation)
				return;

			if (!Method.DeclaringType.IsDelegate)
				return;

			if (!DelegatePatcher.PatchDelegate(this))
				return;

			IsCILDecodeRequired = false;
			IsExecutePipeline = true;

			if (IsTraceable(5))
			{
				var traceLog = new TraceLog(TraceType.MethodDebug, Method, "XX-Delegate Patched", MethodData.Version);
				traceLog?.Log("This delegate method was patched");
				PostTraceLog(traceLog);
			}
		}

		private void ExternalMethod()
		{
			if (!Method.IsExternal)
				return;

			IsCILDecodeRequired = false;
			IsExecutePipeline = false;
			IsStackFrameRequired = false;
			MethodData.IsMethodImplementationReplaced = false;

			var intrinsic = Architecture.GetInstrinsicMethod(Method.ExternMethodModule);

			if (intrinsic != null)
				return;

			Symbol.ExternalSymbolName = Method.ExternMethodName;
			Symbol.IsExternalSymbol = true;

			var filename = Method.ExternMethodModule;

			if (filename != null)
			{
				var bytes = Compiler.SearchPathsForFileAndLoad(filename);

				// TODO: Generate an error if the file is not found
				// CompilerException.FileNotFound

				Symbol.SetData(bytes);
			}

			if (NotifyTraceLogHandler != null)
			{
				var traceLog = new TraceLog(TraceType.MethodInstructions, Method, "XX-External Method", MethodData.Version);
				traceLog?.Log($"This method is external linked: {Method.ExternMethodName}");
				NotifyTraceLogHandler.Invoke(traceLog);
			}
		}

		private void InternalMethod()
		{
			if (!Method.IsInternal)
				return;

			IsCILDecodeRequired = false;
			IsExecutePipeline = false;
			IsStackFrameRequired = false;

			if (NotifyTraceLogHandler != null)
			{
				var traceLog = new TraceLog(TraceType.MethodInstructions, Method, "XX-External Method", MethodData.Version);
				traceLog?.Log($"This method is an internal method");
				NotifyTraceLogHandler.Invoke(traceLog);
			}
		}

		/// <summary>
		/// Stops the method compiler.
		/// </summary>
		/// <returns></returns>
		public void Stop()
		{
			IsStopped = true;
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
		/// Splits the long operand.
		/// </summary>
		/// <param name="longOperand">The long operand.</param>
		public void SplitLongOperand(Operand longOperand)
		{
			VirtualRegisters.SplitLongOperand(TypeSystem, longOperand);
		}

		/// <summary>
		/// Splits the long operand.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <param name="operandLow">The operand low.</param>
		/// <param name="operandHigh">The operand high.</param>
		public void SplitLongOperand(Operand operand, out Operand operandLow, out Operand operandHigh)
		{
			//if (operand.IsParameter)
			//{
			//	operandLow = Operand.CreateStackParameter(TypeSystem.BuiltIn.U4, operand.Index, operand.Name + " (Low)", false, (int)operand.Offset);
			//	operandHigh = Operand.CreateStackParameter(TypeSystem.BuiltIn.U4, operand.Index, operand.Name + " (High)", false, (int)operand.Offset + 4);
			//}
			//else
			if (operand.Is64BitInteger)
			{
				SplitLongOperand(operand);
				operandLow = operand.Low;
				operandHigh = operand.High;
			}
			else
			{
				operandLow = operand;
				operandHigh = ConstantZero32;
			}
		}

		/// <summary>
		/// Allocates the virtual register or stack slot.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public Operand AllocateVirtualRegisterOrStackSlot(MosaType type)
		{
			if (!MosaTypeLayout.CanFitInRegister(type))
			{
				return AddStackLocal(type);
			}
			else
			{
				var resultType = Compiler.GetStackType(type);
				return CreateVirtualRegister(resultType);
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
				if (!MosaTypeLayout.CanFitInRegister(local.Type) || local.IsPinned)
				{
					LocalVariables[index++] = AddStackLocal(local.Type, local.IsPinned);
				}
				else
				{
					var stacktype = Compiler.GetStackType(local.Type);
					LocalVariables[index++] = CreateVirtualRegister(stacktype);
				}
			}
		}

		/// <summary>
		/// Gets the size of the reference or type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="aligned">if set to <c>true</c> [aligned].</param>
		/// <returns></returns>
		public int GetReferenceOrTypeSize(MosaType type, bool aligned)
		{
			if (type.IsValueType)
			{
				if (aligned)
					return Alignment.AlignUp(TypeLayout.GetTypeSize(type), Architecture.NativeAlignment);
				else
					return TypeLayout.GetTypeSize(type);
			}
			else
			{
				return Architecture.NativeAlignment;
			}
		}

		/// <summary>
		/// Gets the position.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <returns></returns>
		public int GetPosition(int label)
		{
			return Labels[label];
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

		#region Constant Helper Methods

		public Operand CreateConstant(byte value)
		{
			return Operand.CreateConstant(TypeSystem.BuiltIn.U1, value);
		}

		public Operand CreateConstant(int value)
		{
			return Operand.CreateConstant(TypeSystem.BuiltIn.I4, value);
		}

		public Operand CreateConstant(uint value)
		{
			return Operand.CreateConstant(TypeSystem.BuiltIn.U4, value);
		}

		public Operand CreateConstant(long value)
		{
			return Operand.CreateConstant(TypeSystem.BuiltIn.I8, value);
		}

		public Operand CreateConstant(ulong value)
		{
			return Operand.CreateConstant(TypeSystem.BuiltIn.U8, value);
		}

		public Operand CreateConstant(float value)
		{
			return Operand.CreateConstant(value, TypeSystem);
		}

		public Operand CreateConstant(double value)
		{
			return Operand.CreateConstant(value, TypeSystem);
		}

		#endregion Constant Helper Methods
	}
}
