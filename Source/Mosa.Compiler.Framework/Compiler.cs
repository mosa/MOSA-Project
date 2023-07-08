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

	public bool FullCheckMode { get; set; }

	public uint ObjectHeaderSize { get; }

	public bool HasError { get; private set; }

	#endregion Properties

	#region Static Methods

	private static List<BaseCompilerStage> GetDefaultCompilerPipeline(MosaSettings mosaSettings, bool is32BitPlatform) => new List<BaseCompilerStage>
	{
		new InlinedSetupStage(),
		new UnitTestStage(),
		new TypeInitializerStage(),
		mosaSettings.Devirtualization ? new DevirtualizationStage() : null,
		new StaticFieldStage(),
		new MethodTableStage(),
		new ExceptionTableStage(),
		new MetadataStage(),
		!string.IsNullOrEmpty(mosaSettings.PreLinkHashFile) ? new PreLinkHashFileStage() : null,
		new LinkerLayoutStage(),
		!string.IsNullOrEmpty(mosaSettings.PostLinkHashFile) ? new PostLinkHashFileStage() : null,
		!string.IsNullOrEmpty(mosaSettings.CompileTimeFile) ? new MethodCompileTimeStage() : null,
		!string.IsNullOrEmpty(mosaSettings.OutputFile) && mosaSettings.EmitBinary ? new LinkerEmitStage() : null,
		!string.IsNullOrEmpty(mosaSettings.MapFile) ? new MapFileStage() : null,
		!string.IsNullOrEmpty(mosaSettings.DebugFile) ? new DebugFileStage() : null,
		!string.IsNullOrEmpty(mosaSettings.InlinedFile) ? new InlinedFileStage() : null,
	};

	private static List<BaseMethodCompilerStage> GetDefaultMethodPipeline(MosaSettings mosaSettings, bool is64BitPlatform) => new List<BaseMethodCompilerStage>
	{
		new CILDecoderStage(),
		new ExceptionStage(),
		new IRTransformsStage(),
		mosaSettings.Devirtualization ? new DevirtualizeCallStage() : null,
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
		mosaSettings.BasicOptimizations && mosaSettings.BitTracker ? new OptimizationStage(false) : null,
		mosaSettings.BasicOptimizations && mosaSettings.LongExpansion ? new OptimizationStage(mosaSettings.LongExpansion) : null,

		mosaSettings.TwoPassOptimization && mosaSettings.ValueNumbering && mosaSettings.SSA ? new ValueNumberingStage() : null,
		mosaSettings.TwoPassOptimization && mosaSettings.LoopInvariantCodeMotion && mosaSettings.SSA ? new LoopInvariantCodeMotionStage() : null,
		mosaSettings.TwoPassOptimization && mosaSettings.SparseConditionalConstantPropagation && mosaSettings.SSA ? new SparseConditionalConstantPropagationStage() : null,
		mosaSettings.TwoPassOptimization && mosaSettings.BitTracker ? new BitTrackerStage() : null,
		mosaSettings.TwoPassOptimization && mosaSettings.BasicOptimizations && mosaSettings.SSA ? new OptimizationStage(mosaSettings.LongExpansion) : null,

		mosaSettings.SSA ? new ExitSSAStage() : null,

		new IRCleanupStage(),

		mosaSettings.InlineMethods || mosaSettings.InlineExplicit ? new InlineEvaluationStage() : null,
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
		mosaSettings.EmitBinary ? new ProtectedRegionLayoutStage() : null,
	};

	#endregion Static Methods

	#region Methods

	public Compiler(MosaCompiler mosaCompiler)
	{
		TypeSystem = mosaCompiler.TypeSystem;
		TypeLayout = mosaCompiler.TypeLayout;
		MosaSettings = mosaCompiler.MosaSettings;
		Architecture = mosaCompiler.Platform;
		CompilerHooks = mosaCompiler.CompilerHooks;
		TraceLevel = MosaSettings.TraceLevel;
		Statistics = MosaSettings.EmitStatistics;
		FullCheckMode = MosaSettings.FullCheckMode;

		PostEvent(CompilerEvent.CompilerStart);

		Linker = new MosaLinker(this);

		ObjectHeaderSize = Architecture.NativePointerSize + 4 + 4; // Hash Value (32-bit) + Lock & Status (32-bit) + Method Table

		StackFrame = Operand.CreateCPURegisterNativeInteger(Architecture.StackFrameRegister, Architecture.Is32BitPlatform);
		StackPointer = Operand.CreateCPURegisterNativeInteger(Architecture.StackPointerRegister, Architecture.Is32BitPlatform);
		ExceptionRegister = Operand.CreateCPURegisterObject(Architecture.ExceptionRegister);
		LeaveTargetRegister = Operand.CreateCPURegisterNativeInteger(Architecture.LeaveTargetRegister, Architecture.Is32BitPlatform);

		LinkRegister = Architecture.LinkRegister == null ? null : Operand.CreateCPURegisterNativeInteger(Architecture.LinkRegister, Architecture.Is32BitPlatform);
		ProgramCounter = Architecture.ProgramCounter == null ? null : Operand.CreateCPURegisterNativeInteger(Architecture.ProgramCounter, Architecture.Is32BitPlatform);

		MethodStagePipelines = new Pipeline<BaseMethodCompilerStage>[MaxThreads];

		MethodScheduler = new MethodScheduler(this);
		MethodScanner = new MethodScanner(this);

		CollectntrinsicAndStubMethods();

		PlugSystem = new PlugSystem(TypeSystem);

		PlatformInternalRuntimeType = GetPlatformInternalRuntimeType();
		InternalRuntimeType = GeInternalRuntimeType();

		// Build the default compiler pipeline
		CompilerPipeline.Add(GetDefaultCompilerPipeline(MosaSettings, Architecture.Is32BitPlatform));

		// Call hook to allow for the extension of the pipeline
		CompilerHooks.ExtendCompilerPipeline?.Invoke(CompilerPipeline);

		Architecture.ExtendCompilerPipeline(CompilerPipeline, MosaSettings);

		IsStopped = false;
		HasError = false;
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

	private Pipeline<BaseMethodCompilerStage> GetOrCreateMethodStagePipeline(int threadID)
	{
		var pipeline = MethodStagePipelines[threadID];

		if (pipeline == null)
		{
			pipeline = new Pipeline<BaseMethodCompilerStage>();

			MethodStagePipelines[threadID] = pipeline;

			pipeline.Add(GetDefaultMethodPipeline(MosaSettings, Architecture.Is64BitPlatform));

			// Call hook to allow for the extension of the pipeline
			CompilerHooks.ExtendMethodCompilerPipeline?.Invoke(pipeline);

			Architecture.ExtendMethodCompilerPipeline(pipeline, MosaSettings);

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
		PostEvent(CompilerEvent.CompilingMethodsStart);

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

		MethodScanner.Complete();

		// Sum up the counters
		foreach (var methodData in CompilerData.MethodData)
		{
			GlobalCounters.Merge(methodData.Counters);
		}

		EmitCounters();

		PostEvent(CompilerEvent.FinalizationEnd);
		PostEvent(CompilerEvent.CompilerEnd);
	}

	private void LogException(Exception exception, string title, string stage)
	{
		PostEvent(CompilerEvent.Exception, exception.Message);

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
		foreach (var counter in GlobalCounters.Export())
		{
			PostEvent(CompilerEvent.Counter, counter);
		}
	}

	public string SearchPathsForFile(string filename)
	{
		foreach (var path in MosaSettings.SearchPaths)
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
}
