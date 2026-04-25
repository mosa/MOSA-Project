// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using System.Reflection;
using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.CompilerStages;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Utility.Configuration;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Mosa Compiler
/// </summary>
public sealed class Compiler
{
	private static class Constant
	{
		public const uint MaxThreads = 1024;
		public const long LockContentionThresholdMs = 100;
	}

	#region Data Members

	private Dictionary<string, IntrinsicMethodDelegate> InternalIntrinsicMethods { get; } = new Dictionary<string, IntrinsicMethodDelegate>();

	private Dictionary<string, StubMethodDelegate> InternalStubMethods { get; } = new Dictionary<string, StubMethodDelegate>();

	private readonly Pipeline<BaseMethodCompilerStage>[] MethodStagePipelines;

	private int ActiveThreadCount;
	private long[] ThreadCPUTicks;
	private long[] ThreadWallTicks;

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
	public MosaSettings MosaSettings { get; }

	/// <summary>
	/// Gets the method scanner.
	/// </summary>
	public MethodScanner MethodScanner { get; }

	/// <summary>
	/// Gets the counters.
	/// </summary>
	public Counters GlobalCounters { get; }

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
	public CompilerData CompilerData { get; private set; }

	/// <summary>
	/// Gets or sets a value indicating whether [all stop].
	/// </summary>
	public bool IsStopped { get; private set; }

	public CompilerHooks CompilerHooks { get; }

	public int TraceLevel { get; }

	public bool Statistics { get; }

	public bool FullCheckMode { get; set; }

	public uint ObjectHeaderSize { get; }

	public bool HasError { get; private set; }

	public Stopwatch CompileTime { get; } = new Stopwatch();

	public Stopwatch TotalCompileTime { get; } = new Stopwatch();

	public Stopwatch LinkerTime { get; } = new Stopwatch();

	public LockMonitor LockMonitor { get; }

	#endregion Properties

	#region Static Methods

	private static List<BaseCompilerStage> GetDefaultCompilerPipeline(MosaSettings mosaSettings) => new List<BaseCompilerStage>
	{
		new InlinedSetupStage(),
		new UnitTestStage(),
		new TypeInitializerStage(),
		mosaSettings.Devirtualization ? new DevirtualizationStage() : null,
		new BootOptionStage(),
		new StaticFieldStage(),
		new MethodTableStage(),
		new ExceptionTableStage(),
		new MetadataStage(),
		!string.IsNullOrEmpty(mosaSettings.PreLinkHashFile) ? new PreLinkHashFileStage() : null,
		new LinkerLayoutStage(),
		!string.IsNullOrEmpty(mosaSettings.PostLinkHashFile) ? new PostLinkHashFileStage() : null,
		!string.IsNullOrEmpty(mosaSettings.OutputFile) && mosaSettings.EmitBinary ? new LinkerEmitStage() : null,
		!string.IsNullOrEmpty(mosaSettings.CompileTimeFile) ? new MethodCompileTimeStage() : null,
		!string.IsNullOrEmpty(mosaSettings.MapFile) ? new MapFileStage() : null,
		!string.IsNullOrEmpty(mosaSettings.CounterFile) ? new CounterFileStage() : null,
		!string.IsNullOrEmpty(mosaSettings.DebugFile) ? new DebugFileStage() : null,
		!string.IsNullOrEmpty(mosaSettings.InlinedFile) ? new InlinedFileStage() : null,
	};

	private static void InitializeMethodCompilerPipeline(Pipeline<BaseMethodCompilerStage> pipeline, MosaSettings mosaSettings)
	{
		pipeline.Add(
		[
			new CILDecoderStage(),
			new ExceptionStage(),
			new FastBlockOrderingStage(),
			mosaSettings.Devirtualization ? new DevirtualizeCallStage() : null,
			mosaSettings.BasicOptimizations ? new OptimizationStage(false) : null,
			new IRTransformsStage(),
			new PlugStage(),
			new RuntimeStage(),

			mosaSettings.InlineMethods || mosaSettings.InlineExplicit ? new InlineStage() : null,

			mosaSettings.BasicOptimizations ? new OptimizationStage(false) : null,
			mosaSettings.SSA ? new EdgeSplitStage() : null,
			mosaSettings.SSA ? new EnterSSAStage() : null,
			mosaSettings.BasicOptimizations && mosaSettings.SSA ? new OptimizationStage(false) : null,

			mosaSettings.ValueNumbering && mosaSettings.SSA ? new ValueNumberingStage() : null,
			mosaSettings.LoopInvariantCodeMotion && mosaSettings.SSA ? new LoopInvariantCodeMotionStage() : null,
			mosaSettings.SparseConditionalConstantPropagation && mosaSettings.SSA ? new SparseConditionalConstantPropagationStage() : null,
			mosaSettings.BasicOptimizations && mosaSettings.SSA && (mosaSettings.ValueNumbering || mosaSettings.LoopInvariantCodeMotion || mosaSettings.SparseConditionalConstantPropagation) ? new OptimizationStage(false) : null,
			mosaSettings.BitTracker ? new BitTrackerStage() : null,
			mosaSettings.LoopRangeTracker && mosaSettings.SSA ? new LoopRangeTrackerStage() : null,
			mosaSettings.BasicOptimizations ? new OptimizationStage(mosaSettings.LongExpansion) : null,

			mosaSettings.TwoPassOptimization && mosaSettings.ValueNumbering && mosaSettings.SSA ? new ValueNumberingStage() : null,
			mosaSettings.TwoPassOptimization && mosaSettings.LoopInvariantCodeMotion && mosaSettings.SSA ? new LoopInvariantCodeMotionStage() : null,
			mosaSettings.TwoPassOptimization && mosaSettings.SparseConditionalConstantPropagation && mosaSettings.SSA ? new SparseConditionalConstantPropagationStage() : null,
			mosaSettings.TwoPassOptimization && mosaSettings.BitTracker ? new BitTrackerStage() : null,
			mosaSettings.TwoPassOptimization && mosaSettings.LoopRangeTracker && mosaSettings.SSA ? new LoopRangeTrackerStage() : null,
			mosaSettings.TwoPassOptimization && mosaSettings.BasicOptimizations && mosaSettings.SSA ? new OptimizationStage(mosaSettings.LongExpansion) : null,

			new NopRemovalStage(),
			new FastBlockOrderingStage(),

			mosaSettings.SSA ? new ExitSSAStage() : null,

			new DeadBlockStage(),

			mosaSettings.InlineMethods || mosaSettings.InlineExplicit ? new InlineEvaluationStage() : null,
			new NewObjectStage(),
			new CallStage(),
			new CompoundStage(),
			new PlatformIntrinsicStage(),

			new PlatformEdgeSplitStage(),
			new VirtualRegisterReindexStage(),
			new GreedyRegisterAllocatorStage(),
			new StackLayoutStage(),

			new CodeGenerationStage(),
		]);
	}

	private static void ExtendMethodCompilerPipeline(Pipeline<BaseMethodCompilerStage> pipeline, MosaSettings mosaSettings)
	{
		pipeline.InsertBefore<CodeGenerationStage>(
		[
			new DeadBlockStage(),
			new SafePointStage(),
			new AdvancedBlockOrderingStage(),
			new JumpOptimizationStage()
		]);

		pipeline.InsertAfterLast<CodeGenerationStage>(
		[
			new ProtectedRegionLayoutStage(),
			new SafePointLayoutStage(),
		]);
	}

	#endregion Static Methods

	#region Methods

	public Compiler(MosaCompiler mosaCompiler)
	{
		TypeSystem = mosaCompiler.TypeSystem;
		TypeLayout = mosaCompiler.TypeLayout;
		MosaSettings = mosaCompiler.MosaSettings;
		Architecture = mosaCompiler.Platform;
		CompilerHooks = mosaCompiler.CompilerHooks;

		Architecture.UpdateSetting(MosaSettings);

		TraceLevel = MosaSettings.TraceLevel;
		Statistics = MosaSettings.EmitStatistics;
		FullCheckMode = MosaSettings.FullCheckMode;

		PostEvent(CompilerEvent.CompilerStart);

		GlobalCounters = new Counters(this, "Global");
		CompilerData = new CompilerData(this);
		Linker = new MosaLinker(this);
		LockMonitor = new LockMonitor(this);

		ObjectHeaderSize = Architecture.NativePointerSize + 4 + 4; // Method Table Ptr + Hash Value (32-bit) + Lock & Status (32-bit)

		MethodStagePipelines = new Pipeline<BaseMethodCompilerStage>[Constant.MaxThreads];
		ThreadCPUTicks = new long[Constant.MaxThreads];
		ThreadWallTicks = new long[Constant.MaxThreads];

		MethodScheduler = new MethodScheduler(this);
		MethodScanner = new MethodScanner(this);

		CollectIntrinsicAndStubMethods();

		PlugSystem = new PlugSystem(TypeSystem);

		PlatformInternalRuntimeType = GetPlatformInternalRuntimeType();
		InternalRuntimeType = GeInternalRuntimeType();

		// Build the default compiler pipeline
		CompilerPipeline.Add(GetDefaultCompilerPipeline(MosaSettings));

		// Call hook to allow for the extension of the pipeline
		CompilerHooks.ExtendCompilerPipeline?.Invoke(CompilerPipeline);

		Architecture.ExtendCompilerPipeline(CompilerPipeline, MosaSettings);

		IsStopped = false;
		HasError = false;
	}

	private void CollectIntrinsicAndStubMethods()
	{
		foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
		{
			if (!type.IsClass)
				continue;

			foreach (var method in type.GetRuntimeMethods())
			{
				// Now get all the IntrinsicMethodAttribute attributes
				var intrinsicMethodAttributes = (IntrinsicMethodAttribute[])method.GetCustomAttributes(typeof(IntrinsicMethodAttribute), true);

				for (var i = 0; i < intrinsicMethodAttributes.Length; i++)
				{
					var d = (IntrinsicMethodDelegate)Delegate.CreateDelegate(typeof(IntrinsicMethodDelegate), method);

					// Finally add the dictionary entry mapping the target name and the delegate
					InternalIntrinsicMethods.Add(intrinsicMethodAttributes[i].Target, d);
				}

				// Now get all the StubMethodAttribute attributes
				var stubMethodAttributes = (StubMethodAttribute[])method.GetCustomAttributes(typeof(StubMethodAttribute), true);

				for (var i = 0; i < stubMethodAttributes.Length; i++)
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
	/// <param name="threadSlot">The thread slot.</param>
	public void CompileMethod(MosaMethod method, BasicBlocks basicBlocks, int threadSlot = 0)
	{
		PostEvent(CompilerEvent.MethodCompileStart, method.FullName, threadSlot);

		var pipeline = GetMethodStagePipeline(threadSlot);

		var methodCompiler = new MethodCompiler(this, method, basicBlocks, threadSlot)
		{
			Pipeline = pipeline
		};

		methodCompiler.Compile();

		PostEvent(CompilerEvent.MethodCompileEnd, method.FullName, threadSlot);

		CompilerHooks.NotifyMethodCompiled?.Invoke(method);
	}

	public void CompileMethod(Transform transform)
	{
		PostEvent(CompilerEvent.MethodCompileStart, transform.Method.FullName, transform.MethodCompiler.ThreadID);

		var pipeline = GetMethodStagePipeline(transform.MethodCompiler.ThreadID);

		transform.MethodCompiler.Pipeline = pipeline;
		transform.MethodCompiler.Compile();

		PostEvent(CompilerEvent.MethodCompileEnd, transform.Method.FullName, transform.MethodCompiler.ThreadID);

		CompilerHooks.NotifyMethodCompiled?.Invoke(transform.Method);
	}

	private Pipeline<BaseMethodCompilerStage> GetMethodStagePipeline(int threadSlot)
	{
		var pipeline = MethodStagePipelines[threadSlot];

		if (pipeline != null)
			return pipeline;

		pipeline = new Pipeline<BaseMethodCompilerStage>();

		MethodStagePipelines[threadSlot] = pipeline;

		// Setup the initial pipeline
		InitializeMethodCompilerPipeline(pipeline, MosaSettings);

		// Call hook to allow for the extension of the pipeline
		CompilerHooks.ExtendMethodCompilerPipeline?.Invoke(pipeline, MosaSettings);

		// Extend pipeline with architecture stages
		Architecture.ExtendMethodCompilerPipeline(pipeline, MosaSettings);

		// Extend pipeline after all hooks and architecture stages are added
		ExtendMethodCompilerPipeline(pipeline, MosaSettings);

		foreach (var stage in pipeline)
		{
			stage.Initialize(this);
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
	/// The method iterates the compilation stage chain and runs each stage on the input.
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
		PostEvent(CompilerEvent.CompilingMethodsStart);

		while (true)
		{
			if (IsStopped)
				return;

			if (ProcessQueue(0) == null)
				break;
		}

		PostEvent(CompilerEvent.CompilingMethodsCompleted);
	}

	private MosaMethod ProcessQueue(int threadSlot)
	{
		var methodData = MethodScheduler.Get();

		return CompileMethod(methodData, threadSlot);
	}

	public MosaMethod CompileMethod(MethodData methodData, int threadSlot)
	{
		if (methodData == null)
			return null;

		try
		{
			CompileMethod(methodData.Method, threadSlot);

			return methodData.Method;
		}
		catch (Exception exception)
		{
			LogException(exception, exception.Message, "ProcessQueue");
			Stop();
			return null;
		}
		finally
		{
			MethodScheduler.MarkCompleted(methodData);
		}
	}

	public void CompileMethod(MosaMethod method)
	{
		if (!MethodScheduler.IsCompilable(method))
			return;

		Debug.Assert(!method.HasOpenGenericParams);

		CompileMethod(method, 0);
	}

	private MosaMethod CompileMethod(MosaMethod method, int threadSlot)
	{
		if (method.IsCompilerGenerated)
			return method;

		Debug.Assert(!method.HasOpenGenericParams);

		var lockTimer = Stopwatch.StartNew();
		lock (method)
		{
			LockMonitor.RecordLockWait(lockTimer, method, null, location: "Method");

			CompileMethod(method, null, threadSlot);
		}

		CompilerHooks.NotifyProgress?.Invoke(MethodScheduler.TotalMethods, MethodScheduler.TotalMethods - MethodScheduler.TotalQueuedMethods);

		return method;
	}

	public void ExecuteCompile(int maxThreads)
	{
		ActiveThreadCount = maxThreads;

		GlobalCounters.Set("Compiler.MaxThreads", maxThreads);

		PostEvent(CompilerEvent.CompilingMethodsStart);

		CompileTime.Start();
		TotalCompileTime.Start();

		if (maxThreads > 1)
		{
			var pool = new PipelinePool(MethodScheduler, this, maxThreads);

			// Connect the pool to the scheduler for profiling
			MethodScheduler.SetPipelinePool(pool);

			// subscribe scheduler -> pool signal
			var schedulerSubscription = MethodScheduler.Subscribe(pool.NotifyWorkAdded);

			// wait until done
			pool.Completion.GetAwaiter().GetResult();

			schedulerSubscription.Dispose();

			pool.DisposeAsync().AsTask().GetAwaiter().GetResult();
		}
		else
		{
			while (true)
			{
				var result = ProcessQueue(0);

				if (result == null)
					return;
			}
		}

		PostEvent(CompilerEvent.CompilingMethodsCompleted);
	}

	/// <summary>
	/// Executes the compiler post compiler stages.
	/// </summary>
	/// <remarks>
	/// The method iterates the compilation stage chain and runs each stage on the input.
	/// </remarks>
	internal void Finalization()
	{
		CompileTime.Stop();
		GlobalCounters.Set("Elapsed.Total.Milliseconds", (int)CompileTime.ElapsedMilliseconds);

		PostEvent(CompilerEvent.FinalizationStart);

		// Emit per-thread performance metrics to GlobalCounters
		//EmitThreadPerformanceMetrics();

		// Sum up the counters
		foreach (var methodData in CompilerData.MethodData)
		{
			GlobalCounters.Update(methodData.Counters);
		}

		foreach (var stage in CompilerPipeline)
		{
			PostEvent(CompilerEvent.FinalizationStageStart, stage.Name);

			try
			{
				stage.ExecuteFinalization();
			}
			catch (CompilerException exception)
			{
				exception.Stage ??= stage.Name;

				LogException(exception, exception.Message, stage.Name);
			}
			catch (Exception exception)
			{
				LogException(exception, $"Stage: {stage} -> {exception.Message}", stage.Name);
			}

			PostEvent(CompilerEvent.FinalizationStageEnd, stage.Name);
		}

		TotalCompileTime.Stop();
		GlobalCounters.Set("Elapsed.TotalCompile.Milliseconds", (int)TotalCompileTime.ElapsedMilliseconds);
		GlobalCounters.Set("Compiler.TotalMethods", MethodScheduler.TotalMethods);

		MethodScanner.Complete();

		EmitCounters();

		// Output lock contention summary
		LockMonitor.GetLockContentionSummary(Constant.LockContentionThresholdMs);

		PostEvent(CompilerEvent.FinalizationEnd);
		PostEvent(CompilerEvent.CompilerEnd);
	}

	private void LogException(Exception exception, string title, string stage)
	{
		PostEvent(CompilerEvent.Exception, exception.Message);
		PostEvent(CompilerEvent.Exception, exception.ToString());

		var exceptionLog = new TraceLog(TraceType.GlobalDebug, null, stage, "Exception");

		exceptionLog.Log(exception.Message);
		exceptionLog.Log(string.Empty);
		exceptionLog.Log(exception.ToString());

		PostTraceLog(exceptionLog);

		HasError = true;
		Stop();
	}

	public void Stop()
	{
		IsStopped = true;
		HasError = true;
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
		foreach (var counter in GlobalCounters.GetSortedCounters())
		{
			PostEvent(CompilerEvent.Counter, counter.ToString());
		}
	}

	public byte[] SearchPathsForFileAndLoad(string filename)
	{
		var file = FileFinder.Find(filename, MosaSettings.SearchPaths);

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
	/// <param name="threadSlot">The thread identifier.</param>
	public void PostEvent(CompilerEvent compilerEvent, string message = null, int threadSlot = 0)
	{
		CompilerHooks.NotifyEvent?.Invoke(compilerEvent, message ?? string.Empty, threadSlot);
	}

	private MosaType GetPlatformInternalRuntimeType()
	{
		return TypeSystem.GetType($"Mosa.Runtime.{Architecture.PlatformName}.Internal");
	}

	private MosaType GeInternalRuntimeType()
	{
		return TypeSystem.GetType("Mosa.Runtime.Internal");
	}

	public MethodData GetMethodData(MosaMethod method)
	{
		return CompilerData.GetMethodData(method);
	}

	private void EmitThreadPerformanceMetrics()
	{
		if (ActiveThreadCount == 0)
			return;

		double wallClockMs = CompileTime.ElapsedMilliseconds;

		// Per-thread metrics
		for (var threadId = 0; threadId < ActiveThreadCount; threadId++)
		{
			long processingTicks = ThreadCPUTicks[threadId];
			long wallTicks = ThreadWallTicks[threadId];
			double processingMs = processingTicks / 10000.0;
			double wallMs = wallTicks / 10000.0;
			double utilityPercent = wallMs > 0 ? (processingMs / wallMs) * 100 : 0;

			GlobalCounters.Set($"Compiler.Performance.Thread.{threadId}.CPUTime.Milliseconds", (int)processingMs);
			GlobalCounters.Set($"Compiler.Performance.Thread.{threadId}.WallTime.Milliseconds", (int)wallMs);
			GlobalCounters.Set($"Compiler.Performance.Thread.{threadId}.Utility.Percent", (int)utilityPercent);
		}

		// Aggregate metrics
		long totalProcessingTicks = 0;
		for (var i = 0; i < ActiveThreadCount; i++)
		{
			totalProcessingTicks += ThreadCPUTicks[i];
		}

		double totalProcessingMs = totalProcessingTicks / 10000.0;
		double avgProcessingMs = ActiveThreadCount > 0 ? totalProcessingMs / ActiveThreadCount : 0;

		GlobalCounters.Set("Compiler.Performance.TotalProcessingTime.Milliseconds", (int)totalProcessingMs);
		GlobalCounters.Set("Compiler.Performance.WallClockTime.Milliseconds", (int)wallClockMs);
		GlobalCounters.Set("Compiler.Performance.AveragePerThread.Milliseconds", (int)avgProcessingMs);

		// Min/Max thread analysis
		if (ActiveThreadCount > 0)
		{
			var maxThreadId = 0;
			var minThreadId = 0;
			var maxTicks = ThreadCPUTicks[0];
			var minTicks = ThreadCPUTicks[0];

			for (var i = 1; i < ActiveThreadCount; i++)
			{
				var ticks = ThreadCPUTicks[i];
				if (ticks > maxTicks)
				{
					maxTicks = ticks;
					maxThreadId = i;
				}
				if (ticks < minTicks)
				{
					minTicks = ticks;
					minThreadId = i;
				}
			}

			double maxThreadMs = maxTicks / 10000.0;
			double minThreadMs = minTicks / 10000.0;
			double imbalancePercent = minTicks > 0 ? ((maxTicks - minTicks) / (double)maxTicks) * 100 : 0;

			GlobalCounters.Set("Compiler.Performance.MaxThread.Milliseconds", (int)maxThreadMs);
			GlobalCounters.Set("Compiler.Performance.MinThread.Milliseconds", (int)minThreadMs);
			GlobalCounters.Set("Compiler.Performance.MaxThread.ID", maxThreadId);
			GlobalCounters.Set("Compiler.Performance.MinThread.ID", minThreadId);
			GlobalCounters.Set("Compiler.Performance.ThreadImbalance.Percent", (int)imbalancePercent);
		}
	}

	#endregion Helper Methods
}
