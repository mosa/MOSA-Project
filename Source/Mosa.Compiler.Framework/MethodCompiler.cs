// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.RegisterAllocator;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using static Mosa.Compiler.Framework.CompilerHooks;

namespace Mosa.Compiler.Framework;

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

	private readonly NotifyTraceLogHandler NotifyInstructionTraceHandler;

	private readonly NotifyTraceLogHandler NotifyTranformTraceHandler;

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
	public Operand Constant32_0 { get; }

	/// <summary>
	/// Gets the 64-bit constant zero.
	/// </summary>
	public Operand Constant64_0 { get; }

	/// <summary>
	/// Gets the R4 constant zero.
	/// </summary>
	public Operand ConstantR4_0 { get; }

	/// <summary>
	/// Gets the R4 constant zero.
	/// </summary>
	public Operand ConstantR8_0 { get; }

	/// <summary>
	/// Gets or sets a value indicating whether this instance is execute pipeline.
	/// </summary>
	public bool IsExecutePipeline { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether this method requires CIL decoding .
	/// </summary>
	public bool IsCILStream { get; set; }

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

		LocalStack = new LocalStack(Is32BitPlatform);
		VirtualRegisters = new VirtualRegisters(Is32BitPlatform);

		Parameters = new Operand[method.Signature.Parameters.Count + (method.HasThis || method.HasExplicitThis ? 1 : 0)];

		Constant32_0 = CreateConstant((uint)0);
		Constant64_0 = CreateConstant((ulong)0);
		ConstantR4_0 = CreateConstant(0.0f);
		ConstantR8_0 = CreateConstant(0.0d);

		ConstantZero = Is32BitPlatform ? Constant32_0 : Constant64_0;

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

		if (!MosaTypeLayout.IsUnderlyingPrimitive(Method.Signature.ReturnType))
		{
			offset += (int)TypeLayout.GetTypeSize(Method.Signature.ReturnType);
		}

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

	/// <summary>
	/// Compiles the method referenced by this method compiler.
	/// </summary>
	public void Compile2()
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

			log.Log(MethodData.Counters.Export());

			Compiler.PostTraceLog(log);
		}
	}

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

				log.Log(MethodData.Counters.Export());

				Compiler.PostTraceLog(log);
			}
		}
		catch (Exception exception)
		{
			Compiler.PostEvent(CompilerEvent.Exception, $"Method: {Method} -> {exception.Message}");

			var exceptionLog = new TraceLog(TraceType.MethodDebug, Method, "Exception", MethodData.Version);

			exceptionLog.Log(exception.Message);
			exceptionLog.Log("");
			exceptionLog.Log(exception.ToString());

			Compiler.PostTraceLog(exceptionLog);

			Stop();
			Compiler.Stop();
		}
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

		if (!Framework.DelegatePatcher.Patch(this))
			return;

		IsCILStream = false;
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

	private void InternalMethod()
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

	private void StubMethod()
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
	private Operand CreateVirtualRegister(MosaType type)
	{
		return VirtualRegisters.Allocate(type);
	}

	/// <summary>
	/// Splits the long operand.
	/// </summary>
	/// <param name="operand">The long operand.</param>
	public void SplitOperand(Operand operand)
	{
		VirtualRegisters.SplitOperand(operand);
	}

	/// <summary>
	/// Splits the long operand.
	/// </summary>
	/// <param name="operand">The operand.</param>
	/// <param name="operandLow">The operand low.</param>
	/// <param name="operandHigh">The operand high.</param>
	public void SplitOperand(Operand operand, out Operand operandLow, out Operand operandHigh)
	{
		var is64Bit = operand.IsInteger64;

		if (!operand.IsInteger64 && !operand.IsInteger32)
		{
			// figure it out
			var underlyingType = MosaTypeLayout.GetUnderlyingType(operand.Type);

			if (underlyingType.IsUI8)
				is64Bit = true;

			if (Is64BitPlatform)
			{
				if (underlyingType.IsPointer
					|| underlyingType.IsFunctionPointer
					|| underlyingType.IsN
					|| underlyingType.IsManagedPointer
					|| underlyingType.IsReferenceType)
					is64Bit = true;
			}
		}

		if (is64Bit)
		{
			SplitOperand(operand);
			operandLow = operand.Low;
			operandHigh = operand.High;
		}
		else
		{
			operandLow = operand;
			operandHigh = Constant32_0;
		}
	}

	/// <summary>
	/// Allocates the virtual register or stack slot.
	/// </summary>
	/// <param name="type">The type.</param>
	/// <returns></returns>
	public Operand AllocateVirtualRegisterOrStackSlot(MosaType type)
	{
		if (MosaTypeLayout.IsUnderlyingPrimitive(type))
		{
			var resultType = Compiler.GetStackType(type);
			return CreateVirtualRegister(resultType);
		}
		else
		{
			return LocalStack.Allocate(type);
		}
	}

	/// <summary>
	/// Gets the size of the reference or type.
	/// </summary>
	/// <param name="type">The type.</param>
	/// <param name="aligned">if set to <c>true</c> [aligned].</param>
	/// <returns></returns>
	public uint GetReferenceOrTypeSize(MosaType type, bool aligned)
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

	#region Type Conversion Helpers

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

	public ElementType GetElementType(MosaType type)
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
			return Is32BitPlatform ? ElementType.I4 : ElementType.I8;
		else if (type.IsManagedPointer)
			return ElementType.ManagedPointer;
		else if (type.IsPointer)
			return Is32BitPlatform ? ElementType.I4 : ElementType.I8;

		throw new CompilerException($"Cannot translate to ElementType from Type: {type}");
	}

	public PrimitiveType GetPrimitiveType(MosaType type)
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
			return Is32BitPlatform ? PrimitiveType.Int32 : PrimitiveType.Int64;
		else if (type.IsPointer)
			return Is32BitPlatform ? PrimitiveType.Int32 : PrimitiveType.Int64;
		else if (type.IsValueType)
			return PrimitiveType.ValueType;

		throw new CompilerException($"Cannot translate to PrimitiveType from Type: {type}");
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

	public uint GetSize(ElementType elementType)
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
			ElementType.Object => Is32BitPlatform ? 4 : 8u,
			ElementType.ManagedPointer => Is32BitPlatform ? 4 : 8u,
			_ => throw new CompilerException($"Cannot get size of {elementType}"),
		};
	}

	#endregion Type Conversion Helpers

	#region Allocator Helpers

	public Operand AllocateVirtualRegisterOrLocalStack(PrimitiveType primitiveType, MosaType type)
	{
		if (primitiveType == PrimitiveType.ValueType)
			return LocalStack.Allocate(type);
		else
			return VirtualRegisters.Allocate(primitiveType);
	}

	#endregion Allocator Helpers
}
