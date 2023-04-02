// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using static Mosa.Compiler.Framework.CompilerHooks;

namespace Mosa.Compiler.Framework.CLR;

/// <summary>
/// Base class of a method compiler.
/// </summary>
/// <remarks>
/// A method compiler is responsible for compiling a single method of an object.
/// </remarks>
internal sealed class ClrMethodCompiler : MethodCompiler
{
	#region Data Members

	/// <summary>
	/// The empty operand list
	/// </summary>
	private static readonly Operand[] emptyOperandList = Array.Empty<Operand>();

	private readonly Stopwatch Stopwatch;

	private readonly NotifyTraceLogHandler NotifyInstructionTraceHandler;

	private readonly NotifyTraceLogHandler NotifyTranformTraceHandler;

	#endregion Data Members

	#region Construction

	/// <summary>
	/// Initializes a new instance of the <see cref="MethodCompiler" /> class.
	/// </summary>
	/// <param name="compiler">The assembly compiler.</param>
	/// <param name="method">The method to compile by this instance.</param>
	/// <param name="basicBlocks">The basic blocks.</param>
	/// <param name="threadID">The thread identifier.</param>
	public ClrMethodCompiler(Compiler compiler, MosaMethod method, BasicBlocks basicBlocks, int threadID)
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
		IsInSSAForm = false;
		AreCPURegistersAllocated = false;
		IsLocalStackFinalized = false;

		BasicBlocks = basicBlocks ?? new BasicBlocks();
		LocalStack = new List<Operand>();
		VirtualRegisters = new VirtualRegisters();

		Parameters = new Operand[method.Signature.Parameters.Count + (method.HasThis || method.HasExplicitThis ? 1 : 0)];

		Constant32_0 = CreateConstant((uint)0);
		Constant64_0 = CreateConstant((ulong)0);
		ConstantR4_0 = CreateConstant(0.0f);
		ConstantR8_0 = CreateConstant(0.0d);

		ConstantZero = Is32BitPlatform ? Constant32_0 : Constant64_0;

		LocalVariables = emptyOperandList;
		ThreadID = threadID;

		IsStopped = false;
		IsExecutePipeline = true;
		IsMethodInlined = false;
		IsCILStream = !Method.IsCompilerGenerated;
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

		EvaluateParameterOperands();

		MethodData.Counters.NewCountSkipLock("ExecutionTime.Setup.Ticks", (int)Stopwatch.ElapsedTicks);
		MethodData.Counters.NewCountSkipLock("ExecutionTime.Setup.MicroSeconds", Stopwatch.Elapsed.Microseconds);
	}

	#endregion Construction

	#region Methods

	/// <summary>
	/// Sets the stack parameter.
	/// </summary>
	/// <param name="index">The index.</param>
	/// <param name="type">The type.</param>
	/// <param name="name">The name.</param>
	/// <param name="isThis">if set to <c>true</c> [is this].</param>
	/// <param name="offset">The offset.</param>
	/// <returns></returns>
	private Operand SetStackParameter(int index, MosaType type, string name, int offset)
	{
		var param = Operand.CreateStackParameter(type, index, name, offset);
		Parameters[index] = param;
		return param;
	}

	/// <summary>
	/// Evaluates the parameter operands.
	/// </summary>
	private void EvaluateParameterOperands()
	{
		var offset = Architecture.OffsetOfFirstParameter;

		//offset += MethodData.ReturnInRegister ? MethodData.ReturnSize : 0;

		if (!MosaTypeLayout.IsUnderlyingPrimitive(Method.Signature.ReturnType))
		{
			offset += (int)TypeLayout.GetTypeSize(Method.Signature.ReturnType);
		}

		//Debug.Assert((MethodData.ReturnInRegister ? MethodData.ReturnSize : 0) == TypeLayout.GetTypeSize(Method.Signature.ReturnType));

		var index = 0;

		if (Method.HasThis || Method.HasExplicitThis)
		{
			if (Method.DeclaringType.IsValueType)
			{
				var ptr = Method.DeclaringType.ToManagedPointer();
				SetStackParameter(index++, ptr, "this", offset);

				var size = GetReferenceOrTypeSize(ptr, true);
				offset += (int)size;
			}
			else
			{
				SetStackParameter(index++, Method.DeclaringType, "this", offset);

				var size = GetReferenceOrTypeSize(Method.DeclaringType, true);
				offset += (int)size;
			}
		}

		foreach (var parameter in Method.Signature.Parameters)
		{
			SetStackParameter(index++, parameter.ParameterType, parameter.Name, offset);

			var size = GetReferenceOrTypeSize(parameter.ParameterType, true);
			offset += (int)size;
		}
	}

	public override void ExecutePipeline()
	{
		if (!IsExecutePipeline)
			return;

		var executionTimes = new long[Pipeline.Count];

		var startTick = Stopwatch.ElapsedTicks;
		var startMS = Stopwatch.ElapsedMilliseconds;

		for (var i = 0; i < Pipeline.Count; i++)
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
			var lastMS = Stopwatch.ElapsedMilliseconds;

			MethodData.ElapsedTicks = lastTick;

			MethodData.Counters.NewCountSkipLock("ExecutionTime.StageStart.Ticks", (int)startTick);
			MethodData.Counters.NewCountSkipLock("ExecutionTime.StageStart.Milliseconds", (int)startMS);
			MethodData.Counters.NewCountSkipLock("ExecutionTime.Total.Ticks", (int)lastTick);
			MethodData.Counters.NewCountSkipLock("ExecutionTime.Total.Milliseconds", (int)lastMS);

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

				var entry = $"[{i:00}] {Pipeline[i].Name.PadRight(45)} : {percentage:00.00} % [{string.Empty.PadRight(per, '#').PadRight(20, ' ')}] ({ticks})";

				executionTimeLog.Log(entry);

				MethodData.Counters.NewCountSkipLock($"ExecutionTime.{i:00}.{Pipeline[i].Name}.Ticks", (int)ticks);
			}

			executionTimeLog.Log($"{"****Total Time".PadRight(57)}({lastTick} Ticks)");
			executionTimeLog.Log($"{"****Total Time".PadRight(57)}({lastMS} Milliseconds)");

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

	public override void CreateTranformInstructionTrace(BaseMethodCompilerStage stage, int step)
	{
		if (NotifyTranformTraceHandler == null)
			return;

		var trace = InstructionTrace.Run(stage.FormattedStageName, Method, BasicBlocks, MethodData.Version, null, step);

		NotifyTranformTraceHandler(trace);
	}

	public override void PlugMethod()
	{
		var plugMethod = Compiler.PlugSystem.GetReplacement(Method);

		if (plugMethod == null)
			return;

		MethodData.ReplacedBy = plugMethod;

		Compiler.MethodScanner.MethodInvoked(plugMethod, Method);

		IsMethodPlugged = true;
		IsCILStream = false;
		IsExecutePipeline = false;
		IsStackFrameRequired = false;

		if (NotifyInstructionTraceHandler != null)
		{
			var traceLog = new TraceLog(TraceType.MethodInstructions, Method, "XX-Plugged Method", MethodData.Version);
			traceLog?.Log($"Plugged by {plugMethod.FullName}");

			NotifyInstructionTraceHandler(traceLog);
		}
	}

	public override void ExternalMethod()
	{
		if (!Method.IsExternal)
			return;

		IsCILStream = false;
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

		if (NotifyInstructionTraceHandler != null)
		{
			var traceLog = new TraceLog(TraceType.MethodInstructions, Method, "XX-External Method", MethodData.Version);
			traceLog?.Log($"This method is external linked: {Method.ExternMethodName}");
			NotifyInstructionTraceHandler(traceLog);
		}
	}

	public override void InternalMethod()
	{
		if (!Method.IsInternal)
			return;

		IsCILStream = false;
		IsExecutePipeline = false;
		IsStackFrameRequired = false;

		if (NotifyInstructionTraceHandler != null)
		{
			var traceLog = new TraceLog(TraceType.MethodInstructions, Method, "XX-Internal Method", MethodData.Version);
			traceLog?.Log($"This method is an internal method");
			NotifyInstructionTraceHandler(traceLog);
		}
	}

	public override void StubMethod()
	{
		var intrinsicAttribute = Method.FindCustomAttribute("System.Runtime.CompilerServices.IntrinsicAttribute");

		if (intrinsicAttribute == null)
			return;

		var methodName = $"{Method.DeclaringType.Namespace}.{Method.DeclaringType.Name}::{Method.Name}";
		var stub = Compiler.GetStubMethod(methodName);

		if (stub == null)
			return;

		IsCILStream = false;
		IsExecutePipeline = true;

		var prologueBlock = BasicBlocks.CreatePrologueBlock();
		var startBlock = BasicBlocks.CreateStartBlock();
		var epilogueBlock = BasicBlocks.CreateEpilogueBlock();

		var prologue = new Context(prologueBlock);
		prologue.AppendInstruction(IRInstruction.Prologue);
		prologue.AppendInstruction(IRInstruction.Jmp, startBlock);

		var epilogue = new Context(epilogueBlock);
		epilogue.AppendInstruction(IRInstruction.Epilogue);

		var start = new Context(startBlock);

		stub(start, this);

		if (NotifyInstructionTraceHandler != null)
		{
			var traceLog = new TraceLog(TraceType.MethodInstructions, Method, "XX-Stubbed Method", MethodData.Version);
			traceLog?.Log($"This method is stubbed");
			NotifyInstructionTraceHandler(traceLog);
		}
	}

	#endregion Methods
}
