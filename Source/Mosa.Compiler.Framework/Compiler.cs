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
	public MosaSettings MosaSettings { get; }

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

	public CompilerHooks CompilerHooks { get; }

	public int TraceLevel { get; }

	public bool Statistics { get; }

	public bool FullCheckMode { get; set; }

	public uint ObjectHeaderSize { get; }

	public bool HasError { get; private set; }

	public Stopwatch CompileTime { get; } = new Stopwatch();

	public Stopwatch TotalCompileTime { get; } = new Stopwatch();

	public Stopwatch LinkerTime { get; } = new Stopwatch();

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

		Linker = new MosaLinker(this);

		ObjectHeaderSize = Architecture.NativePointerSize + 4 + 4; // Method Table Ptr + Hash Value (32-bit) + Lock & Status (32-bit)

		MethodStagePipelines = new Pipeline<BaseMethodCompilerStage>[MaxThreads];

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

	public void CompileMethod(Transform transform)
	{
		PostEvent(CompilerEvent.MethodCompileStart, transform.Method.FullName, transform.MethodCompiler.ThreadID);

		var pipeline = GetOrCreateMethodStagePipeline(transform.MethodCompiler.ThreadID);

		transform.MethodCompiler.Pipeline = pipeline;
		transform.MethodCompiler.Compile();

		PostEvent(CompilerEvent.MethodCompileEnd, transform.Method.FullName, transform.MethodCompiler.ThreadID);

		CompilerHooks.NotifyMethodCompiled?.Invoke(transform.Method);
	}

	private Pipeline<BaseMethodCompilerStage> GetOrCreateMethodStagePipeline(int threadID)
	{
		var pipeline = MethodStagePipelines[threadID];

		if (pipeline == null)
		{
			pipeline = new Pipeline<BaseMethodCompilerStage>();

			MethodStagePipelines[threadID] = pipeline;

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
		catch (Exception exception)
		{
			LogException(exception, exception.Message, "ProcessQueue");
			Stop();
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
		GlobalCounters.Set("Compiler.MaxThreads", maxThreads);

		PostEvent(CompilerEvent.CompilingMethodsStart);

		CompileTime.Start();
		TotalCompileTime.Start();

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
		{
			CompilePass();
		}

		PostEvent(CompilerEvent.CompilingMethodsCompleted);
	}

	private void CompilePass()
	{
		var threadID = Thread.CurrentThread.ManagedThreadId;
		var success = 0;

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
	/// The method iterates the compilation stage chain and runs each stage on the input.
	/// </remarks>
	internal void Finalization()
	{
		CompileTime.Stop();
		GlobalCounters.Set("Elapsed.Total.Milliseconds", (int)CompileTime.ElapsedMilliseconds);

		PostEvent(CompilerEvent.FinalizationStart);

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
		//Stop();
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
		foreach (var counter in GlobalCounters.GetCounters())
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
	/// <param name="threadID">The thread identifier.</param>
	public void PostEvent(CompilerEvent compilerEvent, string message = null, int threadID = 0)
	{
		CompilerHooks.NotifyEvent?.Invoke(compilerEvent, message ?? string.Empty, threadID);
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

	#endregion Helper Methods
}
