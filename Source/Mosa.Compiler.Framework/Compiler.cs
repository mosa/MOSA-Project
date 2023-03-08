// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.CompilerStages;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Mosa Compiler
/// </summary>
public sealed class Compiler
{
	private const uint MaxThreads = 1024;

	#region Data Members

	private readonly Pipeline<BaseMethodCompilerStage>[] MethodStagePipelines;

	private Dictionary<string, IntrinsicMethodDelegate> InternalIntrinsicMethods { get; } = new Dictionary<string, IntrinsicMethodDelegate>();

	private Dictionary<string, StubMethodDelegate> InternalStubMethods { get; } = new Dictionary<string, StubMethodDelegate>();

	#endregion Data Members

	#region Properties

	/// <summary>
	/// Returns the architecture used by the compiler.
	/// </summary>
	public BaseArchitecture Architecture { get; }

	/// <summary>
	/// Gets the compiler pipeline.
	/// </summary>
	public Pipeline<BaseCompilerStage> CompilerPipeline { get; } = new Pipeline<BaseCompilerStage>();

	/// <summary>
	/// Gets the type system.
	/// </summary>
	public TypeSystem TypeSystem { get; }

	/// <summary>
	/// Gets the type layout.
	/// </summary>
	public MosaTypeLayout TypeLayout { get; }

	/// <summary>
	/// Gets the compiler options.
	/// </summary>
	public CompilerSettings CompilerSettings { get; }

	/// <summary>
	/// Gets the method scanner.
	/// </summary>
	public MethodScanner MethodScanner { get; }

	/// <summary>
	/// Gets the counters.
	/// </summary>
	public Counters GlobalCounters { get; } = new Counters();

	/// <summary>
	/// Gets the scheduler.
	/// </summary>
	public MethodScheduler MethodScheduler { get; }

	/// <summary>
	/// Gets the linker.
	/// </summary>
	public MosaLinker Linker { get; }

	/// <summary>
	/// Gets the plug system.
	/// </summary>
	public PlugSystem PlugSystem { get; }

	/// <summary>
	/// Gets the type of the platform internal runtime.
	/// </summary>
	public MosaType PlatformInternalRuntimeType { get; }

	/// <summary>
	/// Gets the type of the internal runtime.
	/// </summary>
	public MosaType InternalRuntimeType { get; }

	/// <summary>
	/// Gets the compiler data.
	/// </summary>
	public CompilerData CompilerData { get; } = new CompilerData();

	/// <summary>
	/// Gets or sets a value indicating whether [all stop].
	/// </summary>
	public bool IsStopped { get; private set; }

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

	public CompilerHooks CompilerHooks { get; }

	public int TraceLevel { get; }

	public bool Statistics { get; }

	public uint ObjectHeaderSize { get; }

	#endregion Properties

	#region Static Methods

	private static List<BaseCompilerStage> GetDefaultCompilerPipeline(CompilerSettings compilerSettings, bool is64BitPlatform)
	{
		return new List<BaseCompilerStage> {
			new InlinedSetupStage(),
			new UnitTestStage(),
			new TypeInitializerStage(),
			compilerSettings.Devirtualization ? new DevirtualizationStage() : null,
			new StaticFieldStage(),
			new MethodTableStage(),
			new ExceptionTableStage(),
			new MetadataStage(),
			(!string.IsNullOrEmpty(compilerSettings.PreLinkHashFile)) ? new PreLinkHashFileStage() : null,
			new LinkerLayoutStage(),
			(!string.IsNullOrEmpty(compilerSettings.PostLinkHashFile)) ? new PostLinkHashFileStage() : null,
			(!string.IsNullOrEmpty(compilerSettings.CompileTimeFile)) ? new MethodCompileTimeStage() : null,
			(!string.IsNullOrEmpty(compilerSettings.OutputFile) && compilerSettings.EmitBinary) ? new LinkerEmitStage() : null,
			(!string.IsNullOrEmpty(compilerSettings.MapFile)) ? new MapFileStage() : null,
			(!string.IsNullOrEmpty(compilerSettings.DebugFile)) ? new DebugFileStage() : null,
			(!string.IsNullOrEmpty(compilerSettings.InlinedFile)) ? new InlinedFileStage() : null,
		};
	}

	private static List<BaseMethodCompilerStage> GetDefaultMethodPipeline(CompilerSettings compilerSettings, bool is64BitPlatform)
	{
		return new List<BaseMethodCompilerStage>() {
			(!compilerSettings.CILDecodingStageV2) ? new CILDecodingStage() : null,
			(compilerSettings.CILDecodingStageV2) ? new CILDecodingStageV2() : null,
			(!compilerSettings.CILDecodingStageV2) ? new CILOperandAssignmentStage(): null,
			(!compilerSettings.CILDecodingStageV2) ? new CILProtectedRegionStage(): null,
			(!compilerSettings.CILDecodingStageV2) ? new CILTransformationStage(): null,

			new VirtualRegisterRenameStage(),

			new ExceptionStage(),
			compilerSettings.Devirtualization ? new DevirtualizeCallStage() : null,
			new PlugStage(),
			new RuntimeStage(),
			new IRTransformsStage(),
			(compilerSettings.InlineMethods || compilerSettings.InlineExplicit) ? new InlineStage() : null,
			new PromoteTemporaryVariables(),
			new StaticLoadOptimizationStage(),

			(compilerSettings.BasicOptimizations) ? new OptimizationStage(false) : null,
			(compilerSettings.SSA) ? new EdgeSplitStage() : null,
			(compilerSettings.SSA) ? new EnterSSAStage() : null,
			(compilerSettings.BasicOptimizations && compilerSettings.SSA) ? new OptimizationStage(false) : null,
			(compilerSettings.ValueNumbering && compilerSettings.SSA) ? new ValueNumberingStage() : null,
			(compilerSettings.LoopInvariantCodeMotion && compilerSettings.SSA) ? new LoopInvariantCodeMotionStage() : null,
			(compilerSettings.SparseConditionalConstantPropagation && compilerSettings.SSA) ? new SparseConditionalConstantPropagationStage() : null,
			(compilerSettings.BasicOptimizations && compilerSettings.SSA && (compilerSettings.ValueNumbering || compilerSettings.LoopInvariantCodeMotion || compilerSettings.SparseConditionalConstantPropagation)) ? new OptimizationStage(false) : null,
			(compilerSettings.BitTracker) ? new BitTrackerStage() : null,
			(compilerSettings.BasicOptimizations && compilerSettings.BitTracker) ? new OptimizationStage(false) : null,
			(compilerSettings.BasicOptimizations && compilerSettings.LongExpansion) ? new OptimizationStage(compilerSettings.LongExpansion) : null,

			(compilerSettings.TwoPass && compilerSettings.ValueNumbering && compilerSettings.SSA) ? new ValueNumberingStage() : null,
			(compilerSettings.TwoPass && compilerSettings.LoopInvariantCodeMotion && compilerSettings.SSA) ? new LoopInvariantCodeMotionStage() : null,
			(compilerSettings.TwoPass && compilerSettings.SparseConditionalConstantPropagation && compilerSettings.SSA) ? new SparseConditionalConstantPropagationStage() : null,
			(compilerSettings.TwoPass && compilerSettings.BitTracker) ? new BitTrackerStage() : null,
			(compilerSettings.TwoPass && compilerSettings.BasicOptimizations && compilerSettings.SSA) ? new OptimizationStage(compilerSettings.LongExpansion) : null,

			(compilerSettings.SSA) ? new ExitSSAStage() : null,
			new IRCleanupStage(),
			(compilerSettings.InlineMethods || compilerSettings.InlineExplicit) ? new InlineEvaluationStage() : null,

			//new StopStage(),

			new NewObjectStage(),
			new CallStage(),
			new CompoundStage(),
			new PlatformIntrinsicStage(),

			new PlatformEdgeSplitStage(),
			new VirtualRegisterRenameStage(),
			new GreedyRegisterAllocatorStage(),
			new StackLayoutStage(),
			new DeadBlockStage(),
			new BlockOrderingStage(),

			//new PreciseGCStage(),

			new CodeGenerationStage(),
			(compilerSettings.EmitBinary) ? new ProtectedRegionLayoutStage() : null,
		};
	}

	#endregion Static Methods

	#region Methods

	public Compiler(MosaCompiler mosaCompiler)
	{
		TypeSystem = mosaCompiler.TypeSystem;
		TypeLayout = mosaCompiler.TypeLayout;
		CompilerSettings = mosaCompiler.CompilerSettings;
		Architecture = mosaCompiler.Platform;
		CompilerHooks = mosaCompiler.CompilerHooks;
		TraceLevel = CompilerSettings.TraceLevel;
		Statistics = CompilerSettings.Statistics;

		PostEvent(CompilerEvent.CompileStart);

		Linker = new MosaLinker(this);

		ObjectHeaderSize = Architecture.NativePointerSize + 4 + 4; // Hash Value (32-bit) + Lock & Status (32-bit) + Method Table

		StackFrame = Operand.CreateCPURegister(TypeSystem.BuiltIn.Pointer, Architecture.StackFrameRegister);
		StackPointer = Operand.CreateCPURegister(TypeSystem.BuiltIn.Pointer, Architecture.StackPointerRegister);
		ExceptionRegister = Operand.CreateCPURegister(TypeSystem.BuiltIn.Object, Architecture.ExceptionRegister);
		LeaveTargetRegister = Operand.CreateCPURegister(TypeSystem.BuiltIn.Object, Architecture.LeaveTargetRegister);

		LinkRegister = Architecture.LinkRegister == null ? null : Operand.CreateCPURegister(TypeSystem.BuiltIn.Object, Architecture.LinkRegister);
		ProgramCounter = Architecture.ProgramCounter == null ? null : Operand.CreateCPURegister(TypeSystem.BuiltIn.Object, Architecture.ProgramCounter);

		MethodStagePipelines = new Pipeline<BaseMethodCompilerStage>[MaxThreads];

		MethodScheduler = new MethodScheduler(this);
		MethodScanner = new MethodScanner(this);

		CollectntrinsicAndStubMethods();

		PlugSystem = new PlugSystem(TypeSystem);

		PlatformInternalRuntimeType = GetPlatformInternalRuntimeType();
		InternalRuntimeType = GeInternalRuntimeType();

		// Build the default compiler pipeline
		CompilerPipeline.Add(GetDefaultCompilerPipeline(CompilerSettings, Architecture.Is64BitPlatform));

		// Call hook to allow for the extension of the pipeline
		CompilerHooks.ExtendCompilerPipeline?.Invoke(CompilerPipeline);

		Architecture.ExtendCompilerPipeline(CompilerPipeline, CompilerSettings);

		IsStopped = false;
	}

	private void CollectntrinsicAndStubMethods()
	{
		foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
		{
			if (!type.IsClass)
				continue;

			foreach (var method in type.GetRuntimeMethods())
			{
				// Now get all the IntrinsicMethodAttribute attributes
				var intrinsicMethodAttributes = (IntrinsicMethodAttribute[])method.GetCustomAttributes(typeof(IntrinsicMethodAttribute), true);

				for (int i = 0; i < intrinsicMethodAttributes.Length; i++)
				{
					var d = (IntrinsicMethodDelegate)Delegate.CreateDelegate(typeof(IntrinsicMethodDelegate), method);

					// Finally add the dictionary entry mapping the target name and the delegate
					InternalIntrinsicMethods.Add(intrinsicMethodAttributes[i].Target, d);
				}

				// Now get all the StubMethodAttribute attributes
				var stubMethodAttributes = (StubMethodAttribute[])method.GetCustomAttributes(typeof(StubMethodAttribute), true);

				for (int i = 0; i < stubMethodAttributes.Length; i++)
				{
					var d = (StubMethodDelegate)Delegate.CreateDelegate(typeof(StubMethodDelegate), method);

					// Finally add the dictionary entry mapping the target name and the delegate
					InternalStubMethods.Add(stubMethodAttributes[i].Target, d);
				}
			}
		}
	}

	/// <summary>
	/// Compiles the method.
	/// </summary>
	/// <param name="method">The method.</param>
	/// <param name="basicBlocks">The basic blocks.</param>
	/// <param name="threadID">The thread identifier.</param>
	public void CompileMethod(MosaMethod method, BasicBlocks basicBlocks, int threadID = 0)
	{
		PostEvent(CompilerEvent.MethodCompileStart, method.FullName, threadID);

		var pipeline = GetOrCreateMethodStagePipeline(threadID);

		var methodCompiler = new MethodCompiler(this, method, basicBlocks, threadID)
		{
			Pipeline = pipeline
		};

		methodCompiler.Compile();

		PostEvent(CompilerEvent.MethodCompileEnd, method.FullName, threadID);

		CompilerHooks.NotifyMethodCompiled?.Invoke(method);
	}

	private Pipeline<BaseMethodCompilerStage> GetOrCreateMethodStagePipeline(int threadID)
	{
		var pipeline = MethodStagePipelines[threadID];

		if (pipeline == null)
		{
			pipeline = new Pipeline<BaseMethodCompilerStage>();

			MethodStagePipelines[threadID] = pipeline;

			pipeline.Add(GetDefaultMethodPipeline(CompilerSettings, Architecture.Is64BitPlatform));

			// Call hook to allow for the extension of the pipeline
			CompilerHooks.ExtendMethodCompilerPipeline?.Invoke(pipeline);

			Architecture.ExtendMethodCompilerPipeline(pipeline, CompilerSettings);

			foreach (var stage in pipeline)
			{
				stage.Initialize(this);
			}
		}

		return pipeline;
	}

	/// <summary>
	/// Compiles the linker method.
	/// </summary>
	/// <param name="methodName">Name of the method.</param>
	/// <returns></returns>
	public MosaMethod CreateLinkerMethod(string methodName)
	{
		return TypeSystem.CreateLinkerMethod(methodName, TypeSystem.BuiltIn.Void, false, null);
	}

	/// <summary>
	/// Executes the compiler pre-compiler stages.
	/// </summary>
	/// <remarks>
	/// The method iterates the compilation stage chain and runs each
	/// stage on the input.
	/// </remarks>
	internal void Setup()
	{
		PostEvent(CompilerEvent.SetupStart);

		foreach (var stage in CompilerPipeline)
		{
			stage.ExecuteInitialization(this);
		}

		foreach (var stage in CompilerPipeline)
		{
			PostEvent(CompilerEvent.SetupStageStart, stage.Name);

			// Execute stage
			stage.ExecuteSetup();

			PostEvent(CompilerEvent.SetupStageEnd, stage.Name);
		}

		PostEvent(CompilerEvent.SetupEnd);
	}

	public void ExecuteCompile()
	{
		PostEvent(CompilerEvent.CompilingMethods);

		while (true)
		{
			if (IsStopped)
				return;

			if (ProcessQueue() == null)
				break;
		}

		PostEvent(CompilerEvent.CompilingMethodsCompleted);
	}

	private MosaMethod ProcessQueue(int threadID = 0)
	{
		try
		{
			var methodData = MethodScheduler.GetMethodToCompile();

			if (methodData == null)
				return null;

			CompileMethod(methodData.Method, threadID);

			return methodData.Method;
		}
		catch (Exception e)
		{
			return null;
		}
	}

	public void CompileMethod(MosaMethod method)
	{
		if (!MethodScheduler.IsCompilable(method))
			return;

		Debug.Assert(!method.HasOpenGenericParams);

		CompileMethod(method, 0);
	}

	private MosaMethod CompileMethod(MosaMethod method, int threadID)
	{
		if (method.IsCompilerGenerated)
			return method;

		Debug.Assert(!method.HasOpenGenericParams);

		lock (method)
		{
			CompileMethod(method, null, threadID);
		}

		CompilerHooks.NotifyProgress?.Invoke(MethodScheduler.TotalMethods, MethodScheduler.TotalMethods - MethodScheduler.TotalQueuedMethods);

		return method;
	}

	public void ExecuteCompile(int maxThreads)
	{
		PostEvent(CompilerEvent.CompilingMethods);

		if (maxThreads > 0)
		{
			var threads = Enumerable
				.Range(0, maxThreads)
				.Select(x => new Thread(CompilePass))
				.ToList();

			threads.ForEach(x => x.Start());
			threads.ForEach(x => x.Join());
		}
		else
			CompilePass();

		PostEvent(CompilerEvent.CompilingMethodsCompleted);
	}

	private void CompilePass() //TODO: Add IProgress<> to report progress
	{
		var threadID = Thread.CurrentThread.ManagedThreadId;
		int success = 0;

		while (true)
		{
			var result = ProcessQueue(threadID);

			if (result == null)
				return;

			success++;
		}
	}

	/// <summary>
	/// Executes the compiler post compiler stages.
	/// </summary>
	/// <remarks>
	/// The method iterates the compilation stage chain and runs each
	/// stage on the input.
	/// </remarks>
	internal void Finalization()
	{
		PostEvent(CompilerEvent.FinalizationStart);

		foreach (var stage in CompilerPipeline)
		{
			PostEvent(CompilerEvent.FinalizationStageStart, stage.Name);

			try
			{
				// Execute stage
				stage.ExecuteFinalization();
			}
			catch (Exception exception)
			{
				PostEvent(CompilerEvent.Exception, $"Stage: {stage.Name} -> {exception.Message}");

				var exceptionLog = new TraceLog(TraceType.GlobalDebug, null, stage.Name, "Exception");

				exceptionLog.Log(exception.Message);
				exceptionLog.Log("");
				exceptionLog.Log(exception.ToString());

				PostTraceLog(exceptionLog);

				//Stop();
			}

			PostEvent(CompilerEvent.FinalizationStageEnd, stage.Name);
		}

		MethodScanner.Complete();

		// Sum up the counters
		foreach (var methodData in CompilerData.MethodData)
		{
			GlobalCounters.Merge(methodData.Counters);
		}

		EmitCounters();

		PostEvent(CompilerEvent.FinalizationEnd);
		PostEvent(CompilerEvent.CompileEnd);
	}

	public void Stop()
	{
		IsStopped = true;
		PostEvent(CompilerEvent.Stopped);
	}

	public IntrinsicMethodDelegate GetInstrincMethod(string name)
	{
		InternalIntrinsicMethods.TryGetValue(name, out IntrinsicMethodDelegate value);

		return value;
	}

	public StubMethodDelegate GetStubMethod(string name)
	{
		InternalStubMethods.TryGetValue(name, out StubMethodDelegate value);

		return value;
	}

	private void EmitCounters()
	{
		foreach (var counter in GlobalCounters.Export())
		{
			PostEvent(CompilerEvent.Counter, counter);
		}
	}

	public string SearchPathsForFile(string filename)
	{
		foreach (var path in CompilerSettings.SearchPaths)
		{
			var file = Path.Combine(path, filename);

			if (File.Exists(file))
			{
				return file;
			}
		}

		return null;
	}

	public byte[] SearchPathsForFileAndLoad(string filename)
	{
		var file = SearchPathsForFile(filename);

		if (file == null)
			return null;

		return File.ReadAllBytes(file);
	}

	#endregion Methods

	#region Helper Methods

	public bool IsTraceable(int traceLevel)
	{
		return TraceLevel >= traceLevel;
	}

	public void PostTraceLog(TraceLog traceLog)
	{
		if (traceLog == null)
			return;

		CompilerHooks.NotifyTraceLog?.Invoke(traceLog);
	}

	/// <summary>
	/// Traces the specified compiler event.
	/// </summary>
	/// <param name="compilerEvent">The compiler event.</param>
	/// <param name="message">The message.</param>
	/// <param name="threadID">The thread identifier.</param>
	public void PostEvent(CompilerEvent compilerEvent, string message = null, int threadID = 0)
	{
		CompilerHooks.NotifyEvent?.Invoke(compilerEvent, message ?? string.Empty, threadID);
	}

	private MosaType GetPlatformInternalRuntimeType()
	{
		return TypeSystem.GetTypeByName("Mosa.Runtime." + Architecture.PlatformName + ".Internal");
	}

	private MosaType GeInternalRuntimeType()
	{
		return TypeSystem.GetTypeByName("Mosa.Runtime.Internal");
	}

	public MethodData GetMethodData(MosaMethod method)
	{
		return CompilerData.GetMethodData(method);
	}

	#endregion Helper Methods

	#region Type Methods

	public MosaType GetTypeFromTypeCode(MosaTypeCode code)
	{
		return code switch
		{
			MosaTypeCode.Void => TypeSystem.BuiltIn.Void,
			MosaTypeCode.Boolean => TypeSystem.BuiltIn.Boolean,
			MosaTypeCode.Char => TypeSystem.BuiltIn.Char,
			MosaTypeCode.I1 => TypeSystem.BuiltIn.I1,
			MosaTypeCode.U1 => TypeSystem.BuiltIn.U1,
			MosaTypeCode.I2 => TypeSystem.BuiltIn.I2,
			MosaTypeCode.U2 => TypeSystem.BuiltIn.U2,
			MosaTypeCode.I4 => TypeSystem.BuiltIn.I4,
			MosaTypeCode.U4 => TypeSystem.BuiltIn.U4,
			MosaTypeCode.I8 => TypeSystem.BuiltIn.I8,
			MosaTypeCode.U8 => TypeSystem.BuiltIn.U8,
			MosaTypeCode.R4 => TypeSystem.BuiltIn.R4,
			MosaTypeCode.R8 => TypeSystem.BuiltIn.R8,
			MosaTypeCode.I => TypeSystem.BuiltIn.I,
			MosaTypeCode.U => TypeSystem.BuiltIn.U,
			MosaTypeCode.String => TypeSystem.BuiltIn.String,
			MosaTypeCode.TypedRef => TypeSystem.BuiltIn.TypedRef,
			MosaTypeCode.Object => TypeSystem.BuiltIn.Object,
			_ => throw new CompilerException("Can't convert type code {code} to type")
		};
	}

	public StackTypeCode GetStackTypeCode(MosaType type)
	{
		switch (type.IsEnum ? type.GetEnumUnderlyingType().TypeCode : type.TypeCode)
		{
			case MosaTypeCode.Boolean:
			case MosaTypeCode.Char:
			case MosaTypeCode.I1:
			case MosaTypeCode.U1:
			case MosaTypeCode.I2:
			case MosaTypeCode.U2:
			case MosaTypeCode.I4:
			case MosaTypeCode.U4:
				if (Architecture.Is32BitPlatform)
					return StackTypeCode.Int32;
				else
					return StackTypeCode.Int64;

			case MosaTypeCode.I8:
			case MosaTypeCode.U8:
				return StackTypeCode.Int64;

			case MosaTypeCode.R4:
			case MosaTypeCode.R8:
				return StackTypeCode.F;

			case MosaTypeCode.I:
			case MosaTypeCode.U:
				if (Architecture.Is32BitPlatform)
					return StackTypeCode.Int32;
				else
					return StackTypeCode.Int64;

			case MosaTypeCode.ManagedPointer:
				return StackTypeCode.ManagedPointer;

			case MosaTypeCode.UnmanagedPointer:
			case MosaTypeCode.FunctionPointer:
				return StackTypeCode.UnmanagedPointer;

			case MosaTypeCode.String:
			case MosaTypeCode.ValueType:
			case MosaTypeCode.ReferenceType:
			case MosaTypeCode.Array:
			case MosaTypeCode.Object:
			case MosaTypeCode.SZArray:
			case MosaTypeCode.Var:
			case MosaTypeCode.MVar:
				return StackTypeCode.O;

			case MosaTypeCode.Void:
				return StackTypeCode.Unknown;
		}

		throw new CompilerException($"Can't transform Type {type} to StackTypeCode");
	}

	public MosaType GetStackType(MosaType type)
	{
		switch (GetStackTypeCode(type))
		{
			case StackTypeCode.Int32:
				return type.TypeSystem.BuiltIn.I4;

			case StackTypeCode.Int64:
				return type.TypeSystem.BuiltIn.I8;

			case StackTypeCode.N:
				return type.TypeSystem.BuiltIn.I;

			case StackTypeCode.F:
				if (type.IsR4)
					return type.TypeSystem.BuiltIn.R4;
				else
					return type.TypeSystem.BuiltIn.R8;

			case StackTypeCode.O:
				return type;

			case StackTypeCode.UnmanagedPointer:
			case StackTypeCode.ManagedPointer:
				return type;
		}

		throw new CompilerException($"Can't convert {type.FullName} to stack type");
	}

	public MosaType GetStackTypeFromCode(StackTypeCode code)
	{
		return code switch
		{
			StackTypeCode.Int32 => TypeSystem.BuiltIn.I4,
			StackTypeCode.Int64 => TypeSystem.BuiltIn.I8,
			StackTypeCode.N => TypeSystem.BuiltIn.I,
			StackTypeCode.F => TypeSystem.BuiltIn.R8,
			StackTypeCode.O => TypeSystem.BuiltIn.Object,
			StackTypeCode.UnmanagedPointer => TypeSystem.BuiltIn.Pointer,
			StackTypeCode.ManagedPointer => TypeSystem.BuiltIn.Object.ToManagedPointer(),
			_ => throw new CompilerException($"Can't convert stack type code {code} to type")
		};
	}

	#endregion Type Methods
}
