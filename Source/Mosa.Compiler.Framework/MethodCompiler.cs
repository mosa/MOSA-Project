// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Base class of a method compiler.
/// </summary>
/// <remarks>
/// A method compiler is responsible for compiling a single method of an object.
/// </remarks>
public sealed class MethodCompiler
{
	private const string IntrinsicAttributeName = "System.Runtime.CompilerServices.IntrinsicAttribute";

	#region Data Members

	private readonly Stopwatch Stopwatch;

	private readonly CompilerHooks.NotifyTraceLogHandler NotifyInstructionTraceHandler;

	private readonly CompilerHooks.NotifyTraceLogHandler NotifyTranformTraceHandler;

	public readonly Transform Transform = new();

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
	/// Gets the assembly compiler.
	/// </summary>
	public Compiler Compiler { get; }

	/// <summary>
	/// Gets the local stack.
	/// </summary>
	public LocalStack LocalStack { get; }

	/// <summary>
	/// Gets or sets the size of the stack.
	/// </summary>
	/// <value>
	/// The size of the stack memory.
	/// </value>
	public int StackSize { get; set; }

	/// <summary>
	/// Gets the virtual registers.
	/// </summary>
	public VirtualRegisters VirtualRegisters { get; }

	/// <summary>
	/// Gets the physical registers.
	/// </summary>
	public PhysicalRegisters PhysicalRegisters { get; }

	/// <summary>
	/// Gets the parameters.
	/// </summary>
	public Parameters Parameters { get; }

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
	/// Gets or sets a value indicating whether this instance is execute pipeline.
	/// </summary>
	public bool IsExecutePipeline { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether this method requires CIL decoding .
	/// </summary>
	public bool HasCILStream { get; set; }

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

	public bool Statistics { get; private set; }

	/// <summary>
	/// Gets the linker symbol.
	/// </summary>
	public LinkerSymbol Symbol
	{ get => MethodData.Symbol; set => MethodData.Symbol = value; }

	/// <summary>
	/// Gets the method scanner.
	/// </summary>
	public MethodScanner MethodScanner { get; }

	public CompilerHooks CompilerHooks { get; }

	public bool IsInSSAForm { get; set; }

	public bool AreCPURegistersAllocated { get; set; }

	public bool IsLocalStackFinalized { get; set; }

	public bool Is32BitPlatform { get; }

	public bool Is64BitPlatform { get; }

	public int? MethodTraceLevel { get; }

	public bool IsTraceTransforms => NotifyTranformTraceHandler != null || NotifyInstructionTraceHandler != null;

	#endregion Properties

	#region Properties - Operand

	/// <summary>
	/// The stack frame
	/// </summary>
	public Operand StackFrame { get; }

	/// <summary>
	/// The stack frame
	/// </summary>
	public Operand StackPointer { get; }

	/// <summary>
	/// The program counter
	/// </summary>
	internal Operand ProgramCounter { get; }

	/// <summary>
	/// The link register
	/// </summary>
	internal Operand LinkRegister { get; }

	/// <summary>
	/// The exception register
	/// </summary>
	public Operand ExceptionRegister { get; }

	/// <summary>
	/// The ;eave target register
	/// </summary>
	public Operand LeaveTargetRegister { get; }

	#endregion Properties - Operand

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
		Is32BitPlatform = Architecture.Is32BitPlatform;
		Is64BitPlatform = Architecture.Is64BitPlatform;

		NotifyInstructionTraceHandler = CompilerHooks.NotifyMethodInstructionTrace != null ? CompilerHooks.NotifyMethodInstructionTrace(Method) : null;
		NotifyTranformTraceHandler = CompilerHooks.NotifyMethodTranformTrace != null ? CompilerHooks.NotifyMethodTranformTrace(Method) : null;

		MethodTraceLevel = compiler.CompilerHooks.GetMethodTraceLevel != null ? compiler.CompilerHooks.GetMethodTraceLevel(method) : null;

		Statistics = compiler.Statistics;
		AreCPURegistersAllocated = false;
		IsLocalStackFinalized = false;
		IsInSSAForm = false;

		BasicBlocks = basicBlocks ?? new BasicBlocks();

		LocalStack = new LocalStack(Is32BitPlatform);
		Parameters = new Parameters(Is32BitPlatform);

		VirtualRegisters = new VirtualRegisters(Is32BitPlatform);
		PhysicalRegisters = new PhysicalRegisters(Is32BitPlatform);

		StackFrame = PhysicalRegisters.AllocateNativeInteger(Architecture.StackFrameRegister);
		StackPointer = PhysicalRegisters.AllocateNativeInteger(Architecture.StackPointerRegister);
		ExceptionRegister = PhysicalRegisters.AllocateObject(Architecture.ExceptionRegister);
		LeaveTargetRegister = PhysicalRegisters.AllocateNativeInteger(Architecture.LeaveTargetRegister);
		LinkRegister = Architecture.LinkRegister == null ? null : PhysicalRegisters.AllocateNativeInteger(Architecture.LinkRegister);
		ProgramCounter = Architecture.ProgramCounter == null ? null : PhysicalRegisters.AllocateNativeInteger(Architecture.ProgramCounter);

		ConstantZero = Is32BitPlatform ? Operand.Constant32_0 : Operand.Constant64_0;

		ThreadID = threadID;

		IsStopped = false;
		IsExecutePipeline = true;
		IsMethodInlined = false;
		HasCILStream = !Method.IsCompilerGenerated;
		HasProtectedRegions = Method.ExceptionHandlers.Count != 0;

		MethodData = Compiler.GetMethodData(Method);

		MethodData.Counters.Reset();
		MethodData.HasCode = false;
		MethodData.Version++;
		MethodData.IsMethodImplementationReplaced = IsMethodPlugged;

		IsStackFrameRequired = MethodData.StackFrameRequired;

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

		var methodInfo = TypeLayout.GetMethodInfo(Method);

		MethodData.ParameterSizes = methodInfo.ParameterSizes;
		MethodData.ParameterOffsets = methodInfo.ParameterOffsets;
		MethodData.ParameterStackSize = methodInfo.ParameterStackSize;
		MethodData.ReturnSize = methodInfo.ReturnSize;
		MethodData.ReturnInRegister = methodInfo.ReturnInRegister;

		MethodData.Counters.Set("ExecutionTime.Setup.Milliseconds", (int)(Stopwatch.Elapsed.Ticks / TimeSpan.TicksPerMillisecond));

		Transform.SetCompiler(compiler);
		Transform.SetMethodCompiler(this);
	}

	#endregion Construction

	#region Methods

	/// <summary>
	/// Compiles the method referenced by this method compiler.
	/// </summary>
	public void Compile()
	{
		try
		{
			PlugMethod();

			PatchDelegate();

			ExternalMethod();

			InternalMethod();

			StubMethod();

			ExecutePipeline();

			Symbol.SetReplacementStatus(MethodData.Inlined);

			if (Statistics)
			{
				var log = new TraceLog(TraceType.MethodCounters, Method, string.Empty, MethodData.Version);

				foreach (var counter in MethodData.Counters.GetCounters())
				{
					log.Log($"{counter}");
				}

				Compiler.PostTraceLog(log);
			}
		}
		catch (CompilerException exception)
		{
			exception.Method ??= Method.FullName;

			LogException(exception, exception.Message);
		}
		catch (Exception exception)
		{
			var message = $"Method: {Method} -> {exception.Message}";

			LogException(exception, message);
		}
	}

	private void LogException(Exception exception, string title)
	{
		Compiler.PostEvent(CompilerEvent.Exception, title);
		Compiler.PostEvent(CompilerEvent.Exception, exception.ToString());

		var exceptionLog = new TraceLog(TraceType.MethodDebug, Method, "Exception", MethodData.Version);

		exceptionLog.Log(title);
		exceptionLog.Log(string.Empty);
		exceptionLog.Log(exception.ToString());

		Compiler.PostTraceLog(exceptionLog);

		Stop();
		Compiler.Stop();
	}

	private void ExecutePipeline()
	{
		if (!IsExecutePipeline)
			return;

		var executionTimes = new long[Pipeline.Count];

		var startTick = Stopwatch.ElapsedTicks;
		var startMS = Stopwatch.ElapsedMilliseconds;

		for (var i = 0; i < Pipeline.Count; i++)
		{
			var stage = Pipeline[i];

			if (IsStopped)
				break;

			try
			{
				Transform.SetStage(stage);

				stage.Setup(this, i);
				stage.Execute();

				executionTimes[i] = Stopwatch.ElapsedTicks;

				CreateInstructionTrace(stage);

				if (Compiler.FullCheckMode)
					stage.FullCheck(true);

				stage.CleanUp();

				if (IsMethodInlined)
					break;
			}
			catch (CompilerException exception)
			{
				exception.Method ??= Method.FullName;
				exception.Stage ??= stage.FormattedStageName;

				LogException(exception, exception.Message);
			}
			catch (Exception exception)
			{
				var message = $"Method: {Method} -> {exception.Message}";

				LogException(exception, message);
			}
		}

		if (Statistics)
		{
			var lastTick = Stopwatch.ElapsedTicks;
			var lastMS = Stopwatch.ElapsedMilliseconds;

			MethodData.ElapsedTicks = lastTick;

			MethodData.Counters.Set("ExecutionTime.Setup.Milliseconds", (int)startMS);
			MethodData.Counters.Set("ExecutionTime.Execution.Milliseconds", (int)(lastMS - startMS));
			MethodData.Counters.Set("ExecutionTime.Total.Milliseconds", (int)lastMS);

			var executionTimeLog = new TraceLog(TraceType.MethodDebug, Method, "Execution Time/Ticks", MethodData.Version);

			var previousTick = startTick;
			var totalTick = lastTick - startTick;

			for (var i = 0; i < Pipeline.Count; i++)
			{
				var pipelineTick = executionTimes[i];
				var ticks = pipelineTick == 0 ? 0 : pipelineTick - previousTick;
				var percentage = totalTick == 0 ? 0 : ticks * 100 / (double)totalTick;
				previousTick = pipelineTick;

				var per = (int)percentage / 5;

				var entry = $"[{i:00}] {Pipeline[i].Name,-45} : {percentage:00.00} % [{string.Empty.PadRight(per, '#'),-20}] ({ticks})";

				executionTimeLog.Log(entry);

				//MethodData.Counters.Update($"ExecutionTime.{i:00}.{Pipeline[i].Name}.Ticks", (int)ticks);
				MethodData.Counters.Set($"ExecutionTime.{i:00}.{Pipeline[i].Name}.Milliseconds", (int)(ticks / TimeSpan.TicksPerMillisecond));
			}

			executionTimeLog.Log($"{"****Total Time",-57}({lastTick} Ticks)");
			executionTimeLog.Log($"{"****Total Time",-57}({lastMS} Milliseconds)");

			PostTraceLog(executionTimeLog);
		}
	}

	private void CreateInstructionTrace(BaseMethodCompilerStage stage)
	{
		if (NotifyInstructionTraceHandler == null)
			return;

		var trace = InstructionTrace.Run(stage.FormattedStageName, Method, BasicBlocks, MethodData.Version, null, 0);

		NotifyInstructionTraceHandler(trace);
	}

	public void CreateTranformInstructionTrace(BaseMethodCompilerStage stage, int step)
	{
		if (NotifyTranformTraceHandler == null)
			return;

		var trace = InstructionTrace.Run(stage.FormattedStageName, Method, BasicBlocks, MethodData.Version, null, step);

		NotifyTranformTraceHandler(trace);
	}

	private void PlugMethod()
	{
		var plugMethod = Compiler.PlugSystem.GetReplacement(Method);

		if (plugMethod == null)
			return;

		MethodData.ReplacedBy = plugMethod;

		Compiler.MethodScanner.MethodInvoked(plugMethod, Method);

		IsMethodPlugged = true;
		HasCILStream = false;
		IsExecutePipeline = false;
		IsStackFrameRequired = false;

		if (NotifyInstructionTraceHandler != null)
		{
			var traceLog = new TraceLog(TraceType.MethodInstructions, Method, "XX-Plugged Method", MethodData.Version);
			traceLog?.Log($"Plugged by {plugMethod.FullName}");

			NotifyInstructionTraceHandler(traceLog);
		}
	}

	public bool IsTraceable(int tracelevel)
	{
		if (MethodTraceLevel.HasValue)
			return MethodTraceLevel.Value >= tracelevel;
		else
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

		if (!DelegatePatcher.Patch(this))
			return;

		HasCILStream = false;
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

		HasCILStream = false;
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
			var bytes = Compiler.SearchPathsForFileAndLoad(filename) ?? throw new CompilerException($"Could not find extern method module: {filename}");
			Symbol.SetData(bytes);
		}

		if (NotifyInstructionTraceHandler != null)
		{
			var traceLog = new TraceLog(TraceType.MethodInstructions, Method, "XX-External Method", MethodData.Version);
			traceLog?.Log($"This method is external linked: {Method.ExternMethodName}");
			NotifyInstructionTraceHandler(traceLog);
		}
	}

	private void InternalMethod()
	{
		if (!Method.IsInternal)
			return;

		HasCILStream = false;
		IsExecutePipeline = false;
		IsStackFrameRequired = false;

		if (NotifyInstructionTraceHandler != null)
		{
			var traceLog = new TraceLog(TraceType.MethodInstructions, Method, "XX-Internal Method", MethodData.Version);
			traceLog?.Log($"This method is an internal method");
			NotifyInstructionTraceHandler(traceLog);
		}
	}

	private void StubMethod()
	{
		var intrinsicAttribute = Method.FindCustomAttribute(IntrinsicAttributeName);

		if (intrinsicAttribute == null)
			return;

		var methodName = $"{Method.DeclaringType.Namespace}.{Method.DeclaringType.Name}::{Method.Name}";
		var stub = Compiler.GetStubMethod(methodName);

		if (stub == null)
			return;

		HasCILStream = false;
		IsExecutePipeline = true;

		var prologueBlock = BasicBlocks.CreatePrologueBlock();
		var startBlock = BasicBlocks.CreateStartBlock();
		var epilogueBlock = BasicBlocks.CreateEpilogueBlock();

		var prologue = new Context(prologueBlock);
		prologue.AppendInstruction(IR.Prologue);
		prologue.AppendInstruction(IR.Jmp, startBlock);

		var epilogue = new Context(epilogueBlock);
		epilogue.AppendInstruction(IR.Epilogue);

		var start = new Context(startBlock);

		stub(start, Transform);

		if (NotifyInstructionTraceHandler != null)
		{
			var traceLog = new TraceLog(TraceType.MethodInstructions, Method, "XX-Stubbed Method", MethodData.Version);
			traceLog?.Log($"This method is stubbed");
			NotifyInstructionTraceHandler(traceLog);
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
	/// Splits the long operand.
	/// </summary>
	/// <param name="operand">The operand.</param>
	/// <param name="operandLow">The operand low.</param>
	/// <param name="operandHigh">The operand high.</param>
	public void SplitOperand(Operand operand, out Operand operandLow, out Operand operandHigh)
	{
		Debug.Assert(Is32BitPlatform);

		if (operand.Low != null)
		{
			operandLow = operand.Low;
			operandHigh = operand.High;
			return;
		}

		if (operand.IsVirtualRegister)
		{
			if (operand.IsInt64)
			{
				VirtualRegisters.SplitOperand(operand);
				operandLow = operand.Low;
				operandHigh = operand.High;
				return;
			}
			else
			{
				operandLow = operand;
				operandHigh = Operand.Constant32_0;
				return;
			}
		}
		else if (operand.IsLocalStack)
		{
			LocalStack.SplitOperand(operand);
			operandLow = operand.Low;
			operandHigh = operand.High;
			return;
		}
		else if (operand.IsParameter)
		{
			Parameters.SplitOperand(operand);
			operandLow = operand.Low;
			operandHigh = operand.High;
			return;
		}
		else if (operand.IsResolvedConstant)
		{
			if (operand.IsInt32)
			{
				operandLow = operand;
				operandHigh = Operand.Constant32_0;
				return;
			}
			else if (operand.IsInt64)
			{
				operandLow = Operand.CreateLow(operand);
				operandHigh = Operand.CreateHigh(operand);
				return;
			}
		}
		else if (operand.IsPhysicalRegister)
		{
			if (operand.IsInt32)
			{
				operandLow = operand;
				operandHigh = Operand.Constant32_0;
				return;
			}
			else if (operand.IsInt64)
			{
				operandLow = Operand.CreateLow(operand);
				operandHigh = Operand.CreateHigh(operand);
				return;
			}
		}
		else if (operand.IsManagedPointer)
		{
			operandLow = operand;
			operandHigh = Operand.Constant32_0;
			return;
		}

		throw new InvalidOperationCompilerException();
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

	public static Operand CreateConstant(byte value)
	{
		return Operand.CreateConstant32(value);
	}

	public Operand CreateConstant(int value)
	{
		return Operand.CreateConstant32((uint)value);
	}

	public Operand CreateConstant(uint value)
	{
		return Operand.CreateConstant32(value);
	}

	public Operand CreateConstant(long value)
	{
		return Operand.CreateConstant64((ulong)value);
	}

	public Operand CreateConstant(ulong value)
	{
		return Operand.CreateConstant64(value);
	}

	public Operand CreateConstant(float value)
	{
		return Operand.CreateConstantR4(value);
	}

	public Operand CreateConstant(double value)
	{
		return Operand.CreateConstantR8(value);
	}

	#endregion Constant Helper Methods

	#region Type Conversion Methods

	public static ElementType GetElementType(PrimitiveType primitiveType)
	{
		return primitiveType switch
		{
			PrimitiveType.Int32 => ElementType.I4,
			PrimitiveType.Int64 => ElementType.I8,
			PrimitiveType.Object => ElementType.Object,
			PrimitiveType.R4 => ElementType.R4,
			PrimitiveType.R8 => ElementType.R8,
			PrimitiveType.ManagedPointer => ElementType.ManagedPointer,
			_ => throw new CompilerException($"Cannot translate to ElementType from PrimitiveType: {primitiveType}")
		};
	}

	public static PrimitiveType GetPrimitiveType(ElementType elementType)
	{
		return elementType switch
		{
			ElementType.I1 => PrimitiveType.Int32,
			ElementType.I2 => PrimitiveType.Int32,
			ElementType.I4 => PrimitiveType.Int32,
			ElementType.I8 => PrimitiveType.Int64,
			ElementType.U1 => PrimitiveType.Int32,
			ElementType.U2 => PrimitiveType.Int32,
			ElementType.U4 => PrimitiveType.Int32,
			ElementType.U8 => PrimitiveType.Int64,
			ElementType.R4 => PrimitiveType.R4,
			ElementType.R8 => PrimitiveType.R8,
			ElementType.Object => PrimitiveType.Object,
			ElementType.ManagedPointer => PrimitiveType.ManagedPointer,
			_ => throw new CompilerException($"Cannot translate to PrimitiveType from ElementType: {elementType}"),
		};
	}

	public static ElementType GetElementType(MosaType type, bool is32BitPlatform)
	{
		if (type.IsReferenceType)
			return ElementType.Object;
		else if (type.IsI1)
			return ElementType.I1;
		else if (type.IsI2)
			return ElementType.I2;
		else if (type.IsI4)
			return ElementType.I4;
		else if (type.IsI8)
			return ElementType.I8;
		else if (type.IsU1)
			return ElementType.U1;
		else if (type.IsU2)
			return ElementType.U2;
		else if (type.IsU4)
			return ElementType.U4;
		else if (type.IsU8)
			return ElementType.U8;
		else if (type.IsR8)
			return ElementType.R8;
		else if (type.IsR4)
			return ElementType.R4;
		else if (type.IsBoolean)
			return ElementType.U1;
		else if (type.IsChar)
			return ElementType.U2;
		else if (type.IsI)
			return is32BitPlatform ? ElementType.I4 : ElementType.I8;
		else if (type.IsManagedPointer)
			return ElementType.ManagedPointer;
		else if (type.IsPointer)
			return is32BitPlatform ? ElementType.I4 : ElementType.I8;
		else if (type.IsTypedRef)
			return is32BitPlatform ? ElementType.I4 : ElementType.I8;
		else if (type.IsValueType)
			return ElementType.ValueType;

		throw new CompilerException($"Cannot translate to ElementType from Type: {type}");
	}

	public static PrimitiveType GetPrimitiveType(MosaType type, bool is32BitPlatform)
	{
		if (type.IsReferenceType)
			return PrimitiveType.Object;
		else if (type.IsManagedPointer)
			return PrimitiveType.ManagedPointer;
		else if (type.IsI1 || type.IsI2 || type.IsI4 || type.IsU1 || type.IsU2 || type.IsU4 || type.IsChar || type.IsBoolean)
			return PrimitiveType.Int32;
		else if (type.IsI8 || type.IsU8)
			return PrimitiveType.Int64;
		else if (type.IsR8)
			return PrimitiveType.R8;
		else if (type.IsR4)
			return PrimitiveType.R4;
		else if (type.IsI)
			return is32BitPlatform ? PrimitiveType.Int32 : PrimitiveType.Int64;
		else if (type.IsPointer)
			return is32BitPlatform ? PrimitiveType.Int32 : PrimitiveType.Int64;
		else if (type.IsValueType)
			return PrimitiveType.ValueType;

		throw new CompilerException($"Cannot translate to PrimitiveType from Type: {type}");
	}

	public PrimitiveType GetPrimitiveType(MosaType type)
	{
		return GetPrimitiveType(type, Is32BitPlatform);
	}

	public ElementType GetElementType(MosaType type)
	{
		return GetElementType(type, Is32BitPlatform);
	}

	#endregion Type Conversion Methods

	#region Size Size Methods

	public static uint GetSize(ElementType elementType, bool is32BitPlatform)
	{
		return elementType switch
		{
			ElementType.I1 => 1,
			ElementType.I2 => 2,
			ElementType.I4 => 4,
			ElementType.I8 => 8,
			ElementType.U1 => 1,
			ElementType.U2 => 2,
			ElementType.U4 => 4,
			ElementType.U8 => 8,
			ElementType.R4 => 4,
			ElementType.R8 => 8,
			ElementType.Object => is32BitPlatform ? 4 : 8u,
			ElementType.ManagedPointer => is32BitPlatform ? 4 : 8u,
			_ => throw new CompilerException($"Cannot get size of {elementType}"),
		};
	}

	public uint GetSize(ElementType elementType)
	{
		return GetSize(elementType, Is32BitPlatform);
	}

	public static uint GetSize(PrimitiveType primitiveType, bool is32BitPlatform)
	{
		return primitiveType switch
		{
			PrimitiveType.Int32 => 4,
			PrimitiveType.Int64 => 8,
			PrimitiveType.R4 => 4,
			PrimitiveType.R8 => 8,
			PrimitiveType.Object => is32BitPlatform ? 4 : 8u,
			PrimitiveType.ManagedPointer => is32BitPlatform ? 4 : 8u,
			_ => throw new CompilerException($"Cannot get size of {primitiveType}"),
		};
	}

	public uint GetSize(PrimitiveType primitiveType)
	{
		return GetSize(primitiveType, Is32BitPlatform);
	}

	public uint GetSize(Operand operand)
	{
		return operand.IsValueType
			? TypeLayout.GetTypeLayoutSize(operand.Type)
			: GetSize(operand.Primitive);
	}

	public uint GetSize(Operand operand, bool aligned)
	{
		var size = GetSize(operand);

		if (aligned)
			size = Alignment.AlignUp(size, Architecture.NativeAlignment);

		return size;
	}

	public uint GetElementSize(MosaType type)
	{
		var underlyingType = MosaTypeLayout.GetUnderlyingType(type);
		var elementType = GetElementType(underlyingType);
		var isPrimitive = IsPrimitive(elementType);

		var size = isPrimitive
			? GetSize(elementType)
			: TypeLayout.GetTypeLayoutSize(underlyingType);

		return size;
	}

	#endregion Size Size Methods

	#region Instruction Maps Methods

	public BaseInstruction GetLoadParamInstruction(ElementType elementType)
	{
		return elementType switch
		{
			ElementType.I1 => IR.LoadParamSignExtend8x32,
			ElementType.U1 => IR.LoadParamZeroExtend8x32,
			ElementType.I2 => IR.LoadParamSignExtend16x32,
			ElementType.U2 => IR.LoadParamZeroExtend16x32,
			ElementType.I4 => IR.LoadParam32,
			ElementType.U4 => IR.LoadParam32,
			ElementType.I8 => IR.LoadParam64,
			ElementType.U8 => IR.LoadParam64,
			ElementType.R4 => IR.LoadParamR4,
			ElementType.R8 => IR.LoadParamR8,
			ElementType.Object => IR.LoadParamObject,
			ElementType.I when Is32BitPlatform => IR.LoadParam32,
			ElementType.I when Is64BitPlatform => IR.LoadParam64,
			ElementType.ManagedPointer => IR.LoadParamManagedPointer,
			_ => throw new InvalidOperationCompilerException(),
		};
	}

	public BaseInstruction GetReturnInstruction(PrimitiveType primitiveType)
	{
		return primitiveType switch
		{
			PrimitiveType.Int32 => IR.SetReturn32,
			PrimitiveType.Int64 => IR.SetReturn64,
			PrimitiveType.R4 => IR.SetReturnR4,
			PrimitiveType.R8 => IR.SetReturnR8,
			PrimitiveType.Object => IR.SetReturnObject,
			PrimitiveType.ValueType => IR.SetReturnCompound,
			PrimitiveType.ManagedPointer => IR.SetReturnManagedPointer,
			_ => throw new InvalidOperationCompilerException(),
		};
	}

	public BaseInstruction GetBoxInstruction(ElementType elementType)
	{
		return elementType switch
		{
			ElementType.R4 => IR.BoxR4,
			ElementType.R8 => IR.BoxR8,
			ElementType.U4 => IR.Box32,
			ElementType.I4 => IR.Box32,
			ElementType.U8 => IR.Box64,
			ElementType.I8 => IR.Box64,
			ElementType.I1 => IR.Box32,
			ElementType.U1 => IR.Box32,
			ElementType.I2 => IR.Box32,
			ElementType.U2 => IR.Box32,
			ElementType.I when Is32BitPlatform => IR.Box32,
			ElementType.I when Is64BitPlatform => IR.Box64,
			ElementType.ManagedPointer when Is32BitPlatform => IR.Box32,
			ElementType.ManagedPointer when Is64BitPlatform => IR.Box64,
			_ => throw new CompilerException($"Invalid ElementType = {elementType}"),
		};
	}

	public BaseInstruction GetLoadInstruction(ElementType elementType)
	{
		return elementType switch
		{
			ElementType.I1 => IR.LoadSignExtend8x32,
			ElementType.U1 => IR.LoadZeroExtend8x32,
			ElementType.I2 => IR.LoadSignExtend16x32,
			ElementType.U2 => IR.LoadZeroExtend16x32,
			ElementType.I4 => IR.Load32,
			ElementType.U4 => IR.Load32,
			ElementType.I8 => IR.Load64,
			ElementType.U8 => IR.Load64,
			ElementType.R4 => IR.LoadR4,
			ElementType.R8 => IR.LoadR8,
			ElementType.Object => IR.LoadObject,
			ElementType.I when Is32BitPlatform => IR.Load32,
			ElementType.I when Is64BitPlatform => IR.Load64,
			ElementType.ManagedPointer => IR.LoadManagedPointer,
			_ => throw new CompilerException($"Invalid ElementType = {elementType}"),
		};
	}

	public BaseInstruction GetMoveInstruction(ElementType elementType)
	{
		return elementType switch
		{
			ElementType.I1 => IR.Move32,
			ElementType.U1 => IR.Move32,
			ElementType.I2 => IR.Move32,
			ElementType.U2 => IR.Move32,
			ElementType.I4 => IR.Move32,
			ElementType.U4 => IR.Move32,
			ElementType.I8 => IR.Move64,
			ElementType.U8 => IR.Move64,
			ElementType.R4 => IR.MoveR4,
			ElementType.R8 => IR.MoveR8,
			ElementType.Object => IR.MoveObject,
			ElementType.I when Is32BitPlatform => IR.Move32,
			ElementType.I when Is64BitPlatform => IR.Move64,
			ElementType.ManagedPointer => IR.MoveManagedPointer,
			_ => throw new CompilerException($"Invalid ElementType = {elementType}"),
		};
	}

	public BaseInstruction GetMoveInstruction(PrimitiveType type)
	{
		return type switch
		{
			PrimitiveType.Object => IR.MoveObject,
			PrimitiveType.Int32 => IR.Move32,
			PrimitiveType.Int64 => IR.Move64,
			PrimitiveType.R4 => IR.MoveR4,
			PrimitiveType.R8 => IR.MoveR8,
			PrimitiveType.ManagedPointer => IR.MoveManagedPointer,
			PrimitiveType.ValueType => IR.MoveCompound,
			_ => throw new CompilerException($"Invalid StackType = {type}"),
		};
	}

	public BaseInstruction GetStoreInstruction(ElementType elementType)
	{
		return elementType switch
		{
			ElementType.I1 => IR.Store8,
			ElementType.U1 => IR.Store8,
			ElementType.I2 => IR.Store16,
			ElementType.U2 => IR.Store16,
			ElementType.I4 => IR.Store32,
			ElementType.U4 => IR.Store32,
			ElementType.I8 => IR.Store64,
			ElementType.U8 => IR.Store64,
			ElementType.R4 => IR.StoreR4,
			ElementType.R8 => IR.StoreR8,
			ElementType.Object => IR.StoreObject,
			ElementType.I when Is32BitPlatform => IR.Store32,
			ElementType.I when Is64BitPlatform => IR.Store64,
			ElementType.ManagedPointer => IR.StoreManagedPointer,
			_ => throw new CompilerException($"Invalid ElementType = {elementType}"),
		};
	}

	public BaseInstruction GetStoreParamInstruction(ElementType elementType)
	{
		return elementType switch
		{
			ElementType.I1 => IR.StoreParam8,
			ElementType.U1 => IR.StoreParam8,
			ElementType.I2 => IR.StoreParam16,
			ElementType.U2 => IR.StoreParam16,
			ElementType.I4 => IR.StoreParam32,
			ElementType.U4 => IR.StoreParam32,
			ElementType.I8 => IR.StoreParam64,
			ElementType.U8 => IR.StoreParam64,
			ElementType.R4 => IR.StoreParamR4,
			ElementType.R8 => IR.StoreParamR8,
			ElementType.Object => IR.StoreParamObject,
			ElementType.I when Is32BitPlatform => IR.StoreParam32,
			ElementType.I when Is64BitPlatform => IR.StoreParam64,
			ElementType.ManagedPointer => IR.StoreParamManagedPointer,
			_ => throw new CompilerException($"Invalid ElementType = {elementType}"),
		};
	}

	#endregion Instruction Maps Methods

	public static bool IsPrimitive(ElementType elementType)
	{
		return elementType != ElementType.ValueType;
	}

	public static bool IsPrimitive(PrimitiveType primitiveType)
	{
		return primitiveType != PrimitiveType.ValueType;
	}

	public Operand AllocateVirtualRegisterOrStackLocal(MosaType type)
	{
		var underlyingType = MosaTypeLayout.GetUnderlyingType(type);
		var primitiveType = GetPrimitiveType(underlyingType);
		var isPrimitive = IsPrimitive(primitiveType);

		var result = isPrimitive
			? VirtualRegisters.Allocate(primitiveType)
			: LocalStack.Allocate(primitiveType, false, type);

		return result;
	}
}
