// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using System.Reflection;
using Mosa.Compiler.Framework.CLR.Stages;
using Mosa.Compiler.Framework.CompilerStages;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CLR;

/// <summary>
/// Mosa Compiler
/// </summary>
internal sealed class ClrCompiler : Compiler
{
	private const uint MaxThreads = 1024;

	#region Data Members

	private readonly Pipeline<BaseMethodCompilerStage>[] MethodStagePipelines;

	private Dictionary<string, IntrinsicMethodDelegate> InternalIntrinsicMethods { get; } = new Dictionary<string, IntrinsicMethodDelegate>();

	private Dictionary<string, StubMethodDelegate> InternalStubMethods { get; } = new Dictionary<string, StubMethodDelegate>();

	#endregion Data Members

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
			!string.IsNullOrEmpty(compilerSettings.PreLinkHashFile) ? new PreLinkHashFileStage() : null,
			new LinkerLayoutStage(),
			!string.IsNullOrEmpty(compilerSettings.PostLinkHashFile) ? new PostLinkHashFileStage() : null,
			!string.IsNullOrEmpty(compilerSettings.CompileTimeFile) ? new MethodCompileTimeStage() : null,
			!string.IsNullOrEmpty(compilerSettings.OutputFile) && compilerSettings.EmitBinary ? new LinkerEmitStage() : null,
			!string.IsNullOrEmpty(compilerSettings.MapFile) ? new MapFileStage() : null,
			!string.IsNullOrEmpty(compilerSettings.DebugFile) ? new DebugFileStage() : null,
			!string.IsNullOrEmpty(compilerSettings.InlinedFile) ? new InlinedFileStage() : null,
		};
	}

	private static List<BaseMethodCompilerStage> GetDefaultMethodPipeline(CompilerSettings compilerSettings, bool is64BitPlatform)
	{
		return new List<BaseMethodCompilerStage>
		{
			!compilerSettings.CILDecodingStageV2 ? new CILDecodingStage() : null,
			compilerSettings.CILDecodingStageV2 ? new CILDecodingStageV2() : null,
			!compilerSettings.CILDecodingStageV2 ? new CILOperandAssignmentStage(): null,
			!compilerSettings.CILDecodingStageV2 ? new CILProtectedRegionStage(): null,
			!compilerSettings.CILDecodingStageV2 ? new CILTransformationStage(): null,

			new CheckedConversionStage(),
			new VirtualRegisterRenameStage(),

			new ExceptionStage(),
			compilerSettings.Devirtualization ? new DevirtualizeCallStage() : null,
			new PlugStage(),
			new RuntimeStage(),
			new IRTransformsStage(),

			compilerSettings.InlineMethods || compilerSettings.InlineExplicit ? new InlineStage() : null,
			new PromoteTemporaryVariables(),
			new StaticLoadOptimizationStage(),

			compilerSettings.BasicOptimizations ? new OptimizationStage(false) : null,
			compilerSettings.SSA ? new EdgeSplitStage() : null,
			compilerSettings.SSA ? new EnterSSAStage() : null,
			compilerSettings.BasicOptimizations && compilerSettings.SSA ? new OptimizationStage(false) : null,
			compilerSettings.ValueNumbering && compilerSettings.SSA ? new ValueNumberingStage() : null,
			compilerSettings.LoopInvariantCodeMotion && compilerSettings.SSA ? new LoopInvariantCodeMotionStage() : null,
			compilerSettings.SparseConditionalConstantPropagation && compilerSettings.SSA ? new SparseConditionalConstantPropagationStage() : null,
			compilerSettings.BasicOptimizations && compilerSettings.SSA && (compilerSettings.ValueNumbering || compilerSettings.LoopInvariantCodeMotion || compilerSettings.SparseConditionalConstantPropagation) ? new OptimizationStage(false) : null,
			compilerSettings.BitTracker ? new BitTrackerStage() : null,
			compilerSettings.BasicOptimizations && compilerSettings.BitTracker ? new OptimizationStage(false) : null,
			compilerSettings.BasicOptimizations && compilerSettings.LongExpansion ? new OptimizationStage(compilerSettings.LongExpansion) : null,

			compilerSettings.TwoPass && compilerSettings.ValueNumbering && compilerSettings.SSA ? new ValueNumberingStage() : null,
			compilerSettings.TwoPass && compilerSettings.LoopInvariantCodeMotion && compilerSettings.SSA ? new LoopInvariantCodeMotionStage() : null,
			compilerSettings.TwoPass && compilerSettings.SparseConditionalConstantPropagation && compilerSettings.SSA ? new SparseConditionalConstantPropagationStage() : null,
			compilerSettings.TwoPass && compilerSettings.BitTracker ? new BitTrackerStage() : null,
			compilerSettings.TwoPass && compilerSettings.BasicOptimizations && compilerSettings.SSA ? new OptimizationStage(compilerSettings.LongExpansion) : null,

			compilerSettings.SSA ? new ExitSSAStage() : null,
			new IRCleanupStage(),
			compilerSettings.InlineMethods || compilerSettings.InlineExplicit ? new InlineEvaluationStage() : null,

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
			compilerSettings.EmitBinary ? new ProtectedRegionLayoutStage() : null,
		};
	}

	#endregion Static Methods

	#region Methods

	public ClrCompiler(MosaCompiler mosaCompiler)
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

		MethodScheduler = new PriorityMethodScheduler(this);
		MethodScanner = new ClrMethodScanner(this);

		CollectIntrinsicAndStubMethods();

		PlugSystem = new ClrPlugSystem(TypeSystem);

		PlatformInternalRuntimeType = GetPlatformInternalRuntimeType();
		InternalRuntimeType = GeInternalRuntimeType();

		// Build the default compiler pipeline
		CompilerPipeline.Add(GetDefaultCompilerPipeline(CompilerSettings, Architecture.Is64BitPlatform));

		// Call hook to allow for the extension of the pipeline
		CompilerHooks.ExtendCompilerPipeline?.Invoke(CompilerPipeline);

		Architecture.ExtendCompilerPipeline(CompilerPipeline, CompilerSettings);

		IsStopped = false;
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
	public override void CompileMethod(MosaMethod method, BasicBlocks basicBlocks, int threadID = 0)
	{
		PostEvent(CompilerEvent.MethodCompileStart, method.FullName, threadID);

		var pipeline = GetOrCreateMethodStagePipeline(threadID);

		var methodCompiler = new ClrMethodCompiler(this, method, basicBlocks, threadID)
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
	/// Executes the compiler pre-compiler stages.
	/// </summary>
	/// <remarks>
	/// The method iterates the compilation stage chain and runs each
	/// stage on the input.
	/// </remarks>
	public override void Setup()
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

	public override void CompileMethod(MosaMethod method)
	{
		if (!MethodScheduler.IsCompilable(method))
			return;

		Debug.Assert(!method.HasOpenGenericParams);

		CompileMethod(method);
	}

	private MosaMethod CompileMethod(MosaMethod method, int threadID = 0)
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

	public override void ExecuteCompile(int maxThreads = 0)
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
		var success = 0;

		while (!IsStopped)
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
	public override void Finalization()
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

	public override void Stop()
	{
		IsStopped = true;
		PostEvent(CompilerEvent.Stopped);
	}

	public override IntrinsicMethodDelegate GetIntrinsicMethod(string name)
	{
		InternalIntrinsicMethods.TryGetValue(name, out IntrinsicMethodDelegate value);

		return value;
	}

	public override StubMethodDelegate GetStubMethod(string name)
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

	#endregion Methods
}
